using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_RelicDesc : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Text _txtContent;
    public void Init(Vector2 position, string desc)
    {
        _rect.anchoredPosition = position;
        _txtContent.text = desc;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

}
