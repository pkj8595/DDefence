using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts;
using System;

public class PawnAnimationController : MonoBehaviour
{
    public Character _pawn;
    public CharacterBuilder _pawnBuilder;

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

    public void Init(Data.CharacterData data)
    {
        _pawnBuilder.Head = data.Head;
        _pawnBuilder.Ears = data.Ears;
        _pawnBuilder.Eyes = data.Eyes;
        _pawnBuilder.Body = data.Body;
        _pawnBuilder.Hair = data.Hair;
        _pawnBuilder.Armor = data.Armor;
        _pawnBuilder.Helmet = data.Helmet;
        _pawnBuilder.Weapon = data.Weapon;
        _pawnBuilder.Shield = data.Shield;
        _pawnBuilder.Cape = data.Cape;
        _pawnBuilder.Back = data.Back;
        _pawnBuilder.Mask = data.Mask;
        _pawnBuilder.Horns = data.Horns;
        _pawnBuilder.Rebuild();
    }

    public void Init(int characterDataNum)
    {
        Init(Managers.Data.CharacterDict[characterDataNum]);
    }

    public void SetAniState(Define.State state)
    {
        Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts.AnimationState pawnState = 0;
        switch (state)
        {
            case Define.State.Die:
                break;
            case Define.State.Moving:
                break;
            case Define.State.Idle:
                break;
            case Define.State.Skill:
                break;
            default: throw new NotSupportedException();

        }
        _pawn.SetState(pawnState);

    }
}