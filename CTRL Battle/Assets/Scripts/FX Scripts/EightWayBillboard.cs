using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BillboardDirection { F, FR, R, RB, B, BL, L, LF }

public class EightWayBillboard : MonoBehaviour
{
    private Transform parentSpriteHolder;
    private SpriteRenderer spriteRenderer;

    private const float FORWARD_ANGLE = 0f;
    private const float FORWARD_RIGHT_ANGLE = -45f;
    private const float RIGHT_ANGLE = -90f;
    private const float RIGHT_BACKWARD_ANGLE = -135f;
    private const float BACKWARD_ANGLE = -180f;
    private const float BACKWARD_ANGLE_ALT = 180f;
    private const float BACKWARD_LEFT_ANGLE = 135f;
    private const float LEFT_ANGLE = 90f;
    private const float LEFT_FORWARD_ANGLE = 45f;

    [SerializeField, Range(0f, 22.5f)] float angleMargin = 20f;

    [SerializeField] string startingSpriteName = "";
    [SerializeField] EightWaySprite[] spriteLibrary;

    /*
    [SerializeField] Sprite forwardSprite;
    [SerializeField] Sprite forwardRightSprite;
    [SerializeField] Sprite rightSprite;
    [SerializeField] Sprite rightBackwardSprite;
    [SerializeField] Sprite backwardSprite;
    [SerializeField] Sprite backwardLeftSprite;
    [SerializeField] Sprite leftSprite;
    [SerializeField] Sprite leftForwardSprite;
    */

    private Dictionary<string, Sprite[]> spriteDictionary = null;
    private BillboardDirection currentSpriteDirection = BillboardDirection.F;
    private Sprite[] currentSpriteList = null;

    void Awake()
    {
        parentSpriteHolder = this.transform.parent;
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        InitializeSpriteDictionary();
        SetSprite(startingSpriteName);
    }

    void Update()
    {
        UpdateSpriteDirection();
    }

    private void InitializeSpriteDictionary()
    {
        if (spriteDictionary != null) { return; }
        spriteDictionary = new Dictionary<string, Sprite[]>();
        foreach (EightWaySprite ews in spriteLibrary)
        {
            spriteDictionary.Add(ews.SpriteName, ews.SpriteList);
        }
    }

    public void SetSprite(string spriteName)
    {
        if (spriteDictionary.ContainsKey(spriteName))
        {
            currentSpriteList = spriteDictionary[spriteName];
        }
    }

    private void UpdateSpriteDirection()
    {
        Vector2 cameraForward = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
        Vector2 parentForward = new Vector2(parentSpriteHolder.forward.x, parentSpriteHolder.forward.z);
        float angleBetween = Vector2.SignedAngle(cameraForward, parentForward);

        if (angleBetween > (FORWARD_ANGLE - angleMargin) && angleBetween < (FORWARD_ANGLE + angleMargin))
        {
            currentSpriteDirection = BillboardDirection.F;
        }
        else if (angleBetween > (FORWARD_RIGHT_ANGLE - angleMargin) && angleBetween < (FORWARD_RIGHT_ANGLE + angleMargin))
        {
            currentSpriteDirection = BillboardDirection.FR;
        }
        else if (angleBetween > (RIGHT_ANGLE - angleMargin) && angleBetween < (RIGHT_ANGLE + angleMargin))
        {
            currentSpriteDirection = BillboardDirection.R;
        }
        else if (angleBetween > (RIGHT_BACKWARD_ANGLE - angleMargin) && angleBetween < (RIGHT_BACKWARD_ANGLE + angleMargin))
        {
            currentSpriteDirection = BillboardDirection.RB;
        }
        else if (angleBetween > (BACKWARD_ANGLE_ALT - angleMargin) || angleBetween < (BACKWARD_ANGLE + angleMargin)) // SignedAngle only returns values between -180 and 180 degrees
        {
            currentSpriteDirection = BillboardDirection.B;
        }
        else if (angleBetween > (BACKWARD_LEFT_ANGLE - angleMargin) && angleBetween < (BACKWARD_LEFT_ANGLE + angleMargin))
        {
            currentSpriteDirection = BillboardDirection.BL;
        }
        else if (angleBetween > (LEFT_ANGLE - angleMargin) && angleBetween < (LEFT_ANGLE + angleMargin))
        {
            currentSpriteDirection = BillboardDirection.L;
        }
        else if (angleBetween > (LEFT_FORWARD_ANGLE - angleMargin) && angleBetween < (LEFT_FORWARD_ANGLE + angleMargin))
        {
            currentSpriteDirection = BillboardDirection.LF;
        }
        else {/* Nothing */}

        spriteRenderer.sprite = currentSpriteList[(int)currentSpriteDirection];
    }
}
