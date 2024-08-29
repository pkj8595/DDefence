using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_UnitInfo : MonoBehaviour
{
    [SerializeField] private Image _unitIcon;
    [SerializeField] private Text _unitName;
    [SerializeField] private Text _unitDesc;
    [SerializeField] private List<Text> _stat;
    [SerializeField] private List<Text> _propertyName;
    [SerializeField] private List<UI_ImageText> _skill;
    [SerializeField] private List<UI_ImageText> _rune;
    [SerializeField] private UI_ImageText _upgrade;
    [SerializeField] private Text _ignoreAttribute;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

[Serializable]
public class UI_ImageText 
{
    public GameObject Obj;
    public Image Icon;
    public Text Name;
}
