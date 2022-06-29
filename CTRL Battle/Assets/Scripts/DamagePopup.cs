using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Vector3 position, int damageNumber, bool isCrit, float time = 1f)
    {
        GameObject tempObj = Instantiate(TextPopups.GetDamagePopupPrefab(), position, Quaternion.identity);
        DamagePopup damagePopup = tempObj.GetComponent<DamagePopup>();
        damagePopup.Setup(damageNumber, isCrit, time);
        return damagePopup;
    }

    [SerializeField, Range(0f, 10f)] float initialVerticalVelocity = 5f;
    [SerializeField, Range(0f, 1f)] float verticalVelocityDrop = 0.075f;
    [SerializeField, Range(0f, 10f)] float maxHorizontalVelocity = 4f;

    [SerializeField, Range(1f, 3f)] float critVelocityMultiplier = 1.5f;

    [SerializeField] float normalFontSize = 8;
    [SerializeField] float critFontSize = 12;

    [SerializeField] Color normalColor;
    [SerializeField] Color critColor;
    [SerializeField, Range(0f, 10f)] float fadeOutRate = 0.01f;

    private TextMeshPro damageNumberText;
    private new Rigidbody rigidbody;

    private float expireTimer;
    private Color textColor;

    private static int sortOrder = 0;

    void Awake()
    {
        damageNumberText = this.gameObject.GetComponent<TextMeshPro>();
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y - verticalVelocityDrop, rigidbody.velocity.z);
        ExpireTimerCountdown();
    }

    public void Setup(int damageNumber, bool isCrit, float time = 1f)
    {
        damageNumberText.SetText(damageNumber.ToString());
        expireTimer = time;
        rigidbody.velocity = new Vector3(HorizontalVelocityRandomizer(maxHorizontalVelocity * (isCrit ? critVelocityMultiplier : 1f)), initialVerticalVelocity * (isCrit ? critVelocityMultiplier : 1f), 0f);

        if (!isCrit)
        {
            damageNumberText.fontSize = normalFontSize;
            textColor = normalColor;
        }
        else
        {
            damageNumberText.fontSize = critFontSize;
            textColor = critColor;
        }
        damageNumberText.color = textColor;

        damageNumberText.sortingOrder = sortOrder;
        sortOrder++;
    }

    private void ExpireTimerCountdown()
    {
        expireTimer -= Time.deltaTime;
        if (expireTimer <= 0f)
        {
            expireTimer = 0f;
            if (fadeOutRate > 0f)
            {
                textColor.a -= fadeOutRate * Time.deltaTime;
                damageNumberText.color = textColor;
                if (textColor.a <= 0f)
                {
                    GameObject.Destroy(this.gameObject);
                }
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    private float HorizontalVelocityRandomizer(float magnitude)
    {
        return (Random.Range(0f, 1f) < 0.5f ? -magnitude : magnitude);
    }
}
