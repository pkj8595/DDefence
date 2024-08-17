using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune 
{
    private Data.RuneData _runeData;

    public string Name { get => _runeData.name; }
    public string Desc { get => _runeData.desc; }


    public void Init(Data.RuneData runeData)
    {
        _runeData = runeData;
    }


}
