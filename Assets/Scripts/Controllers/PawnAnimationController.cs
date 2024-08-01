using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelHero = Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts;
using System;

public class PawnAnimationController : MonoBehaviour
{
    public PixelHero.CharacterBuilder _pawnBuilder;
    public Define.DamageType _attackType = Define.DamageType.MeleeAttack;
    private BaseController _baseController;
    public Animator _animator;

    //todo : particleManager 만들기
    //[SerializeField] private ParticleSystem _moveDust;

    private void Awake()
    {
        if (_baseController == null)
            _baseController = transform.parent.GetComponent<BaseController>();
    }

    /// <summary>
    /// 캐릭터 생성시 테이블 데이터로 CharacterSprite와 attackType 셋팅
    /// </summary>
    /// <param name="data"></param>
    public void Init(Data.CharacterData data)
    {
        _attackType = (Define.DamageType)data.attackType;

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


    public void SetAniState(Define.EPawnAniState state)
    {
        if (Define.EPawnAniState.Moving == state)
        {
            _animator.Play("Running");

            //if (_baseController._navAgent.speed < 3)
            //    _animator.Play("Waking");
            //else
            //    _animator.Play("Running");
            //return;
        }

        _animator.Play(state.ToString());
    }

    public void SetAniTrigger(Define.EPawnAniTriger trigger)
    {
        switch (trigger)
        {
            case Define.EPawnAniTriger.Attack:
                if (_attackType == Define.DamageType.MeleeAttack)
                    _animator.SetTrigger("Slash");
                else
                    _animator.SetTrigger("Shot");
                return;
            case Define.EPawnAniTriger.Skill:
                if (_attackType == Define.DamageType.MeleeAttack)
                    _animator.SetTrigger("Slash");
                else
                    _animator.SetTrigger("Shot");
                return;
        }

        _animator.SetTrigger(trigger.ToString());
    }

    public void OnHitEvent()
    {
        _baseController.ApplyAttack();
    }

    public void OnShotEvent()
    {
        _baseController.ApplyShot();
    }

    public SpriteRenderer Body;
    private static Material DefaultMaterial;
    private static Material BlinkMaterial;

    public void Blink()
    {
        if (DefaultMaterial == null) DefaultMaterial = Body.sharedMaterial;
        if (BlinkMaterial == null) BlinkMaterial = new Material(Shader.Find("GUI/Text Shader"));

        StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        Body.material = BlinkMaterial;

        yield return new WaitForSeconds(0.1f);

        Body.material = DefaultMaterial;
    }

}