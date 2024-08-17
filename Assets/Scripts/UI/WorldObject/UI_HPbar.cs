using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_HPbar : MonoBehaviour
{
    [SerializeField] private Image imgHp;
    [SerializeField] private Image imgMp;
    [SerializeField] private RectTransform rectTransform;
    public IDamageable Unit { get; private set; }
    public void Init(IDamageable unit)
    {
        Unit = unit;
    }

    public void OnUpdate()
    {
        rectTransform.position = Unit.GetTransform().position;
        imgHp.rectTransform.sizeDelta = 
            new Vector2(Utils.Percent(Unit.GetStat().Hp, Unit.GetStat().MaxHp), imgHp.rectTransform.sizeDelta.y);
        imgMp.rectTransform.sizeDelta = 
            new Vector2(Utils.Percent(Unit.GetStat().Mana, Unit.GetStat().MaxMana), imgMp.rectTransform.sizeDelta.y);
    }
    
    public void Clear()
    {
        Unit = null;
    }
}
