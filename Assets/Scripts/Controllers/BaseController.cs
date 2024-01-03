using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour, IWorldObject
{
    [SerializeField] protected Vector3 _destPos;
    [SerializeField] protected GameObject _lockTarget;
    [SerializeField] protected Stat _stat;
    [SerializeField] protected PawnAnimationController _pawnController;
    [SerializeField] protected Define.PawnState _state = Define.PawnState.Idle;

    private Data.CharacterData _characterData;

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;
    public virtual Define.PawnState State
    {
        get { return _state; }
        set
        {
            _state = value;
            _pawnController.SetAniState(_state);
        }
    }

    private void Awake()
    {
        if(_pawnController == null)
            _pawnController = gameObject.GetOrAddComponent<PawnAnimationController>();

        if(_stat == null)
            _stat = gameObject.GetOrAddComponent<Stat>();

    }


    void Update()
    {
        switch (State)
        {
            case Define.PawnState.Die:
                UpdateDie();
                break;
            case Define.PawnState.Moving:
                UpdateMove();
                break;
            case Define.PawnState.Idle:
                UpdateIdle();
                break;
            case Define.PawnState.Skill:
                UpdateSkill();
                break;
        }
    }

    public virtual void Init(int characterNum)
    {
        Init();
        _characterData = Managers.Data.CharacterDict[characterNum];
        _pawnController.Init(_characterData);
        _stat.Init(_characterData.statDataNum);
    }

    protected virtual void Init() { }
    protected virtual void UpdateDie() { }
    protected virtual void UpdateMove() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }

    public void DoNoting()
    {

    }
}
