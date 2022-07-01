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

    [SerializeField] GameObject floatingTextPopupPrefab;
    private static GameObject _floatingTextPopupPrefab;

    [SerializeField] GameObject meterPopupPrefab;
    private static GameObject _meterPopupPrefab;

    private static string popupText = "";

    void Awake()
    {
        _damagePopupPrefab = damagePopupPrefab;
        _floatingTextPopupPrefab = floatingTextPopupPrefab;
        _meterPopupPrefab = meterPopupPrefab;
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

    public static GameObject GetFloatingTextPopupPrefab()
    {
        return _floatingTextPopupPrefab;
    }

    public static GameObject GetMeterPopupPrefab()
    {
        return _meterPopupPrefab;
    }
}
