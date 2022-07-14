using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRLMeter : MonoBehaviour
{
    private const float METER_NORMALIZATION_VALUE = 10f;
    private const int MAX_METER_MAGNITUDE = 10;

    private static int currentMeterStage = 0;

    private static float currentMaxTimer = 0f;
    private static float currentTimer = 0f;

    void Update()
    {
        if (currentTimer > 0f)
        {
            currentTimer -= Time.deltaTime;

            if (currentTimer > currentMaxTimer)
            {
                currentTimer = currentMaxTimer;
            }

            if (currentTimer < 0f)
            {
                currentTimer = 0f;
            }

            Debug.Log(currentTimer);
        }
    }

    public static void StartTimer(float baseTimer)
    {
        currentMaxTimer = (baseTimer * GetTimerModifier());
        currentTimer = currentMaxTimer;
    }

    public static bool IsTimerActive()
    {
        return currentTimer > 0f;
    }

    public static void ChangeCurrentMeterStateBy(int change)
    {
        currentMeterStage += change;
        if (currentMeterStage > MAX_METER_MAGNITUDE)
        {
            currentMeterStage = MAX_METER_MAGNITUDE;
        }
        else if (currentMeterStage < -MAX_METER_MAGNITUDE)
        {
            currentMeterStage = -MAX_METER_MAGNITUDE;
        }
        else {/* Nothing */}
        currentMaxTimer *= GetTimerModifier();
    }

    private static float GetTimerModifier()
    {
        float numerator;
        float denominator;

        if (currentMeterStage > 0)
        {
            numerator = (float)((int)METER_NORMALIZATION_VALUE + currentMeterStage);
            denominator = METER_NORMALIZATION_VALUE;
        }
        else
        {
            numerator = METER_NORMALIZATION_VALUE;
            denominator = (float)((int)METER_NORMALIZATION_VALUE - currentMeterStage);
        }

        return (numerator / denominator);
    }

    
}
