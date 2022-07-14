using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealingPopup : MonoBehaviour
{
    public static HealingPopup Create(Vector3 position, int healingNumber, float time = 1f)
    {
        GameObject tempObj = Instantiate(TextPopups.GetHealingPopupPrefab(), position, Quaternion.identity);
        HealingPopup healingPopup = tempObj.GetComponent<HealingPopup>();
        healingPopup.Setup(healingNumber, time);
        return healingPopup;
    }

    [SerializeField, Range(0f, 10f)] float verticalVelocity = 5f;
    [SerializeField, Range(0f, 5f)] float horizontalAmplitude = 1f;
    [SerializeField] float radiansPerSecond = 3.14f;
    [SerializeField, Range(0f, 10f)] float fadeOutRate = 0.01f;
    
    private TextMeshPro healingNumberText;
    private new Rigidbody rigidbody;

    private float expireTimer;
    private Color textColor;
    private static int sortOrder = 0;

    private float currentRadians = 0f;
    private float initialZPosition;

    void Awake()
    {
        healingNumberText = this.gameObject.GetComponent<TextMeshPro>();
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        currentRadians += (radiansPerSecond * Time.deltaTime);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, initialZPosition + (horizontalAmplitude * Mathf.Sin(currentRadians)));
        ExpireTimerCountdown();
    }

    public void Setup(int healingNumber, float time = 1f)
    {
        healingNumberText.SetText(healingNumber.ToString());
        expireTimer = time;
        rigidbody.velocity = (Vector3.up * verticalVelocity);

        initialZPosition = this.transform.position.z;
        textColor = healingNumberText.color;

        healingNumberText.sortingOrder = sortOrder;
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
                healingNumberText.color = textColor;
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
}
