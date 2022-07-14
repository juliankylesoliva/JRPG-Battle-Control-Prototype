using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EightWaySprite", menuName = "ScriptableObjects/EightWaySprite", order = 2)]
public class EightWaySprite : ScriptableObject
{
    [SerializeField] string spriteName = "";
    public string SpriteName { get { return spriteName; } }

    [SerializeField] Sprite forwardSprite;
    [SerializeField] Sprite forwardRightSprite;
    [SerializeField] Sprite rightSprite;
    [SerializeField] Sprite rightBackwardSprite;
    [SerializeField] Sprite backwardSprite;
    [SerializeField] Sprite backwardLeftSprite;
    [SerializeField] Sprite leftSprite;
    [SerializeField] Sprite leftForwardSprite;
    public Sprite[] SpriteList { get { return new Sprite[] { forwardSprite, forwardRightSprite, rightSprite, rightBackwardSprite, backwardSprite, backwardLeftSprite, leftSprite, leftForwardSprite }; } }
}
