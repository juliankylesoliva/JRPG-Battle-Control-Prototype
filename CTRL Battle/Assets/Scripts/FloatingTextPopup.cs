using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextPopup : MonoBehaviour
{
    public static FloatingTextPopup Create(Vector3 position, string text, Color color, float size, float time = 1f)
    {
        GameObject tempObj = Instantiate(TextPopups.GetFloatingTextPopupPrefab(), position, Quaternion.identity);
        FloatingTextPopup textPopup = tempObj.GetComponent<FloatingTextPopup>();
        textPopup.Setup(text, color, size, time);
        return textPopup;
    }

    [SerializeField, Range(0f, 10f)] float initialVerticalVelocity = 5f;
    [SerializeField, Range(0f, 1f)] float verticalVelocityEasingRate = 0.95f;
    [SerializeField, Range(0f, 10f)] float fadeOutRate = 0.01f;

    private TextMeshPro floatingText;
    private new Rigidbody rigidbody;

    private float expireTimer;
    private Color textColor;

    private static int sortOrder = 0;

    void Awake()
    {
        floatingText = this.gameObject.GetComponent<TextMeshPro>();
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y * verticalVelocityEasingRate, rigidbody.velocity.z);
        ExpireTimerCountdown();
    }

    public void Setup(string text, Color color, float size = 8f, float time = 1f)
    {
        floatingText.SetText(text);
        expireTimer = time;
        rigidbody.velocity = new Vector3(0f, initialVerticalVelocity, 0f);

        textColor = color;
        floatingText.color = textColor;
        floatingText.fontSize = size;

        floatingText.sortingOrder = sortOrder;
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
                floatingText.color = textColor;
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
