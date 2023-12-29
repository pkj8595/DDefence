using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour, IWorldObject
{
    [SerializeField] protected Vector3 _destPos;
    [SerializeField] protected GameObject _lockTarget;
    [SerializeField] protected Define.State _state = Define.State.Idle;
    
    private Stat _stat;
    private PawnAnimationController _pawnController;

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;
    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;
            
            switch (_state)
            {
                case Define.State.Die:
                    break;
                case Define.State.Moving:
                    break;
                case Define.State.Idle:
                    break;
                case Define.State.Skill:
                    break;
            }
        }
    }

    private void Awake()
    {
        _pawnController = gameObject.GetComponent<PawnAnimationController>();
        _stat = gameObject.GetOrAddComponent<Stat>();

    }


    void Update()
    {
        switch (State)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
                UpdateMove();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
        }
    }

    public virtual void Init(int characterNum)
    {
        Init();
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
