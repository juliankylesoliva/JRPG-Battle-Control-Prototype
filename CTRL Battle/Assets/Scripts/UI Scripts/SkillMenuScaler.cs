using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenuScaler : MonoBehaviour
{
    private RectTransform rectTransform;
    private GridLayoutGroup gridLayoutGroup;

    void Awake()
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
        gridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }

    void Update()
    {
        rectTransform.sizeDelta = new Vector2(gridLayoutGroup.cellSize.x, gridLayoutGroup.cellSize.y * this.transform.childCount);
    }
}
