using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] Sprite forwardSprite;
    [SerializeField] Sprite forwardRightSprite;
    [SerializeField] Sprite rightSprite;
    [SerializeField] Sprite rightBackwardSprite;
    [SerializeField] Sprite backwardSprite;
    [SerializeField] Sprite backwardLeftSprite;
    [SerializeField] Sprite leftSprite;
    [SerializeField] Sprite leftForwardSprite;

    void Awake()
    {
        parentSpriteHolder = this.transform.parent;
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        Vector2 cameraForward = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
        Vector2 parentForward = new Vector2(parentSpriteHolder.forward.x, parentSpriteHolder.forward.z);
        float angleBetween = Vector2.SignedAngle(cameraForward, parentForward);

        if (angleBetween > (FORWARD_ANGLE - angleMargin) && angleBetween < (FORWARD_ANGLE + angleMargin))
        {
            spriteRenderer.sprite = forwardSprite;
        }
        else if (angleBetween > (FORWARD_RIGHT_ANGLE - angleMargin) && angleBetween < (FORWARD_RIGHT_ANGLE + angleMargin))
        {
            spriteRenderer.sprite = forwardRightSprite;
        }
        else if (angleBetween > (RIGHT_ANGLE - angleMargin) && angleBetween < (RIGHT_ANGLE + angleMargin))
        {
            spriteRenderer.sprite = rightSprite;
        }
        else if (angleBetween > (RIGHT_BACKWARD_ANGLE - angleMargin) && angleBetween < (RIGHT_BACKWARD_ANGLE + angleMargin))
        {
            spriteRenderer.sprite = rightBackwardSprite;
        }
        else if (angleBetween > (BACKWARD_ANGLE_ALT - angleMargin) || angleBetween < (BACKWARD_ANGLE + angleMargin))
        {
            spriteRenderer.sprite = backwardSprite;
        }
        else if (angleBetween > (BACKWARD_LEFT_ANGLE - angleMargin) && angleBetween < (BACKWARD_LEFT_ANGLE + angleMargin))
        {
            spriteRenderer.sprite = backwardLeftSprite;
        }
        else if (angleBetween > (LEFT_ANGLE - angleMargin) && angleBetween < (LEFT_ANGLE + angleMargin))
        {
            spriteRenderer.sprite = leftSprite;
        }
        else if (angleBetween > (LEFT_FORWARD_ANGLE - angleMargin) && angleBetween < (LEFT_FORWARD_ANGLE + angleMargin))
        {
            spriteRenderer.sprite = leftForwardSprite;
        }
        else {/* Nothing */}
    }
}
