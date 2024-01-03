using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelHero = Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts;
using System;

public class PawnAnimationController : MonoBehaviour
{
    public PixelHero.Character _pawn;
    public PixelHero.CharacterBuilder _pawnBuilder;
    public Define.AttackType _attackType = Define.AttackType.MeleeAttack;

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
        _attackType = (Define.AttackType)data.attackType;

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

    public void SetAniState(Define.PawnState state)
    {
        PixelHero.AnimationState pawnState = 0;
        switch (state)
        {
            case Define.PawnState.Idle:
                pawnState = PixelHero.AnimationState.Idle;
                break;
            case Define.PawnState.Moving:
                pawnState = PixelHero.AnimationState.Idle;
                break;
            case Define.PawnState.Attack:
                if(_attackType == Define.AttackType.MeleeAttack)
                    _pawn.Animator.SetTrigger("Slash");
                else
                    _pawn.Animator.SetTrigger("Shot");
                return;
            case Define.PawnState.Skill:
                pawnState = PixelHero.AnimationState.Jumping;
                break;
            case Define.PawnState.Take_Damage:
                pawnState = PixelHero.AnimationState.Blocking;
                break;
            case Define.PawnState.Die:
                pawnState = PixelHero.AnimationState.Dead;
                break;
            default: throw new NotSupportedException();

        }
        _pawn.SetState(pawnState);

    }
}