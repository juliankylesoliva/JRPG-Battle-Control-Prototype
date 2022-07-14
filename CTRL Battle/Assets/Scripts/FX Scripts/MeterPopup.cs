using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterPopup : MonoBehaviour
{
    public static MeterPopup Create(Vector3 position, float startXScale, float endXScale, bool isMP = false, float time = 1f, bool isHover = false)
    {
        GameObject tempObj = Instantiate(TextPopups.GetMeterPopupPrefab(), position, Quaternion.identity);
        MeterPopup meterPopup = tempObj.GetComponent<MeterPopup>();
        meterPopup.Setup(startXScale, endXScale, isMP, time, isHover);
        return meterPopup;
    }

    [SerializeField] SpriteRenderer meterFilling;
    [SerializeField] Color hpColor;
    [SerializeField] Color mpColor;

    private float startRatio;
    private float endRatio;
    private float lerpRate;
    private float currentLerp = 0f;
    private float expireTimer = 1f;
    private bool hoveringHealthbar = false;

    void Update()
    {
        if (!hoveringHealthbar)
        {
            TimerCountdown();
        }
    }

    public void Setup(float startXScale, float endXScale, bool isMP = false, float time = 1f, bool isHover = false)
    {
        startRatio = startXScale;
        endRatio = endXScale;
        lerpRate = (1f / time);
        hoveringHealthbar = isHover;

        if (isMP)
        {
            meterFilling.color = mpColor;
        }
        else
        {
            meterFilling.color = hpColor;
        }

        meterFilling.transform.localScale = new Vector3(startRatio, 1f, 1f);
    }

    private void TimerCountdown()
    {
        if (currentLerp < 1f)
        {
            currentLerp += (Time.deltaTime * lerpRate);
            if (currentLerp > 1f) { currentLerp = 1f; }
            meterFilling.transform.localScale = new Vector3(Mathf.Lerp(startRatio, endRatio, currentLerp), 1f, 1f);
        }
        else
        {
            expireTimer -= Time.deltaTime;
            if (expireTimer <= 0f)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
