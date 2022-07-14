using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextPopups : MonoBehaviour
{
    [SerializeField] GameObject textBox;
    [SerializeField] TMP_Text announceText;

    [SerializeField] GameObject damagePopupPrefab;
    private static GameObject _damagePopupPrefab;

    [SerializeField] GameObject healingPopupPrefab;
    private static GameObject _healingPopupPrefab;

    [SerializeField] GameObject floatingTextPopupPrefab;
    private static GameObject _floatingTextPopupPrefab;

    [SerializeField] GameObject meterPopupPrefab;
    private static GameObject _meterPopupPrefab;

    private static string popupText = "";

    private static List<FloatingTextPopupParameters> floatingTextQueue = new List<FloatingTextPopupParameters>();

    void Awake()
    {
        _damagePopupPrefab = damagePopupPrefab;
        _healingPopupPrefab = healingPopupPrefab;
        _floatingTextPopupPrefab = floatingTextPopupPrefab;
        _meterPopupPrefab = meterPopupPrefab;
        StartCoroutine(FloatingTextPopupQueueHandler());
    }

    void Update()
    {
        if (popupText.Equals(""))
        {
            announceText.text = "";
            textBox.gameObject.SetActive(false);
            announceText.gameObject.SetActive(false);
        }
        else
        {
            textBox.gameObject.SetActive(true);
            announceText.gameObject.SetActive(true);
            announceText.text = popupText;
        }
    }

    private IEnumerator FloatingTextPopupQueueHandler()
    {
        while (true)
        {
            if (floatingTextQueue.Count > 0)
            {
                FloatingTextPopupParameters parameters = floatingTextQueue[0];
                FloatingTextPopup.Create(parameters.position, parameters.text, parameters.color, parameters.size, parameters.time);
                yield return new WaitForSeconds(parameters.time);
                floatingTextQueue.RemoveAt(0);
            }
            yield return null;
        }
    }

    public static void AddPopupParametersToQueue(FloatingTextPopupParameters parameters)
    {
        floatingTextQueue.Add(parameters);
    }

    public static bool IsPopupQueueEmpty()
    {
        return floatingTextQueue.Count <= 0;
    }

    public static IEnumerator AnnounceForSeconds(string text, float time = 0f)
    {
        popupText = text;
        yield return new WaitForSeconds(time);
        popupText = "";
    }

    public static void Announce(string text)
    {
        popupText = text;
    }

    public static void ClearPopup()
    {
        popupText = "";
    }

    public static GameObject GetDamagePopupPrefab()
    {
        return _damagePopupPrefab;
    }

    public static GameObject GetHealingPopupPrefab()
    {
        return _healingPopupPrefab;
    }

    public static GameObject GetFloatingTextPopupPrefab()
    {
        return _floatingTextPopupPrefab;
    }

    public static GameObject GetMeterPopupPrefab()
    {
        return _meterPopupPrefab;
    }
}

public class FloatingTextPopupParameters
{
    public Vector3 position { get; }
    public string text { get; }
    public Color color { get; }
    public float size { get; }
    public float time { get; }

    public FloatingTextPopupParameters(Vector3 position, string text, Color color, float size, float time = 1f)
    {
        this.position = position;
        this.text = text;
        this.color = color;
        this.size = size;
        this.time = time;
    }
}
