using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterPopup : MonoBehaviour
{
    public static MeterPopup Create(Vector3 position, float startXScale, float endXScale, bool isMP = false, float time = 1f)
    {
        GameObject tempObj = Instantiate(TextPopups.GetMeterPopupPrefab(), position, Quaternion.identity);
        MeterPopup meterPopup = tempObj.GetComponent<MeterPopup>();
        meterPopup.Setup(startXScale, endXScale, isMP, time);
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

    void Update()
    {
        TimerCountdown();
    }

    public void Setup(float startXScale, float endXScale, bool isMP = false, float time = 1f)
    {
        startRatio = startXScale;
        endRatio = endXScale;
        lerpRate = (1f / time);

        if (isMP)
        {
            meterFilling.color = mpColor;
        }
        else
        {
            meterFilling.color = hpColor;
        }
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
