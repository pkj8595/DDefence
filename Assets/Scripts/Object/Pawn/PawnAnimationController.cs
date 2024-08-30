using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelHero = Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts;
using System;
using UnityEngine.U2D.Animation;

public class PawnAnimationController : MonoBehaviour
{
    public PixelHero.CharacterBuilder _pawnBuilder;
    public Animator _animator;
    private PawnBase _pawnBase;
    [SerializeField] private UnityEngine.U2D.Animation.SpriteLibrary spriteLibrary;

    public SpriteRenderer Body;
    private static Material DefaultMaterial;
    private static Material BlinkMaterial;
    //todo : particleManager 만들기
    //[SerializeField] private ParticleSystem _moveDust;

    private void Awake()
    {
        if (_pawnBase == null)
            _pawnBase = transform.parent.GetComponent<PawnBase>();
    }

    /// <summary>
    /// 캐릭터 생성시 테이블 데이터로 CharacterSprite와 attackType 셋팅
    /// </summary>
    /// <param name="data"></param>
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

    public void SetAniState(Define.EPawnAniState state)
    {
        _animator.Play(state.ToStr());
    }

    public void SetAniTrigger(Define.EPawnAniTriger trigger)
    {
        _animator.SetTrigger(trigger.ToStr());
    }

    public void OnBeginAttack()
    {
        _pawnBase.BegineAniAttack();
    }

    public void OnEndAttack()
    {
        _pawnBase.EndAniAttack();
    }

    public Sprite GetIdleSprite()
    {
        return spriteLibrary.GetSprite("Idle", "0");
    }


    public void Blink()
    {
        if (DefaultMaterial == null) DefaultMaterial = Body.sharedMaterial;
        if (BlinkMaterial == null) BlinkMaterial = new Material(Shader.Find("GUI/Text Shader"));

        StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        Body.material = BlinkMaterial;

        yield return YieldCache.WaitForSeconds(0.1f);

        Body.material = DefaultMaterial;
    }

}