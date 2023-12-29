using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts;

public class PawnAnimationController : MonoBehaviour
{
    public Character _pawn;
    public CharacterBuilder _pawnBuilder;
    private Data.CharacterTable _pawnData;

#if UNITY_EDITOR
    public int CharacterDataNum;
#endif

    void Start()
    {
#if UNITY_EDITOR
        Init(CharacterDataNum);
#endif
    }

    void Update()
    {

    }

    public void Init(Data.CharacterTable data)
    {
        _pawnData = data;
        _pawnBuilder.Head = _pawnData.Head;
        _pawnBuilder.Ears = _pawnData.Ears;
        _pawnBuilder.Eyes = _pawnData.Eyes;
        _pawnBuilder.Body = _pawnData.Body;
        _pawnBuilder.Hair = _pawnData.Hair;
        _pawnBuilder.Armor = _pawnData.Armor;
        _pawnBuilder.Helmet = _pawnData.Helmet;
        _pawnBuilder.Weapon = _pawnData.Weapon;
        _pawnBuilder.Shield = _pawnData.Shield;
        _pawnBuilder.Cape = _pawnData.Cape;
        _pawnBuilder.Back = _pawnData.Back;
        _pawnBuilder.Mask = _pawnData.Mask;
        _pawnBuilder.Horns = _pawnData.Horns;
        _pawnBuilder.Rebuild();
    }

    public void Init(int characterDataNum)
    {
        Init(Managers.Data.CharacterDict[characterDataNum]);
    }


}
