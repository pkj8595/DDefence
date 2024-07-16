using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseController : MonoBehaviour, IWorldObject
{
    private Data.CharacterData _characterData;

    [SerializeField] protected Vector3 _destPos;
    [SerializeField] protected GameObject _lockTarget;
    [SerializeField] protected Stat _stat;
    [SerializeField] protected PawnAnimationController _pawnController;
    [SerializeField] protected Define.PawnState _state = Define.PawnState.Idle;

    public CircleCollider2D _colliderAttackRange;
    public NavMeshAgent _navAgent;

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;
    public Define.ERelationShip Relationship { get; set; } = Define.ERelationShip.Friendly;

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
        if (_pawnController == null)
            _pawnController = gameObject.GetOrAddComponent<PawnAnimationController>();

        if (_stat == null)
            _stat = gameObject.GetOrAddComponent<Stat>();

        _navAgent.updateRotation = false;
        _navAgent.updateUpAxis = false;
    }


    public virtual void Update()
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
            case Define.PawnState.Attack:
                UpdateAttack();
                break;
            case Define.PawnState.Take_Damage:
                UpdateTakeDamage();
                break;
        }

    }

    public void LateUpdate()
    {
        OnLateUpdate();
    }

    public virtual void Init(int characterNum)
    {
        //table data setting
        _characterData = Managers.Data.CharacterDict[characterNum];
        _pawnController.Init(_characterData);
        _stat.Init(_characterData.statDataNum);

        //component setting

        //_colliderAttackRange.radius = _stat.AttackRange;
        _navAgent.speed = _stat.MoveSpeed;

        Init();
    }

    protected virtual void Init() { }
    protected virtual void UpdateDie() { }
    protected virtual void UpdateMove() 
    {
        switch (_navAgent.pathStatus)
        {
            case NavMeshPathStatus.PathComplete:
                //Debug.Log("경로가 완전히 생성.");
                break;
            case NavMeshPathStatus.PathPartial:
                //Debug.Log("일부만 생성.");
                if (_navAgent.path.corners != null)
                    _destPos = _navAgent.path.corners[_navAgent.path.corners.Length - 1];
                break;
            case NavMeshPathStatus.PathInvalid:
                //Debug.Log("유효하지 않는 경로");
                _navAgent.isStopped = true;
                _navAgent.ResetPath();
                State = Define.PawnState.Idle;
                return;
        }

        //naviAgent가 이동을 마쳤을 경우 idle로 돌아감
        if (_navAgent.velocity == Vector3.zero && Vector3.Distance(_destPos, transform.position) < 0.1f)
            State = Define.PawnState.Idle;
        /*else
            _pawnController.Turn(_navAgent.velocity.x);*/
    }

    protected virtual void OnLateUpdate() {}

    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateAttack() { }
    protected virtual void UpdateTakeDamage() { }

    public void DoNoting() { }


}
