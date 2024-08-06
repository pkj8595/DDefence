using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class PawnBase : MonoBehaviour, IWorldObject, IDamageable, ISelectedable
{
    private Data.CharacterData _characterData;

    [SerializeField] private Define.WorldObject worldObjectType = Define.WorldObject.Unknown;
    [SerializeField] protected Define.EPawnAniState _state = Define.EPawnAniState.Idle;
    [SerializeField] protected PawnAnimationController _aniController;
    [SerializeField] public NavMeshAgent _navAgent;

    [SerializeField] protected Vector3 _destPos;
    [SerializeField] protected GameObject _lockTarget;
    [SerializeField] protected PawnStat _pawnStat;
    [SerializeField] private bool _isSelected;


    public Action OnDeadAction;

    public Define.WorldObject WorldObjectType { get => worldObjectType; set => worldObjectType = value; }

    public virtual Define.EPawnAniState State
    {
        get { return _state; }
        set
        {
            _state = value;
            _aniController.SetAniState(_state);
        }
    }

    public virtual void SetTriggerAni(Define.EPawnAniTriger trigger)
    {
        _aniController.SetAniTrigger(trigger);
    }


    private void Awake()
    {
        if (_aniController == null)
            _aniController = gameObject.GetOrAddComponent<PawnAnimationController>();

        if (_pawnStat == null)
            _pawnStat = gameObject.GetOrAddComponent<PawnStat>();

        _navAgent.updateRotation = false;
        _navAgent.updateUpAxis = false;
        
    }


    public virtual void Update()
    {
        switch (State)
        {
            case Define.EPawnAniState.Dead:
                UpdateDie();
                break;
            case Define.EPawnAniState.Running:
                UpdateMove();
                break;
            case Define.EPawnAniState.Idle:
                UpdateIdle();
                break;
            case Define.EPawnAniState.Ready:
                UpdateReady();
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
        _aniController.Init(_characterData);
        _pawnStat.Init(_characterData.statDataNum);

        //component setting

        //_colliderAttackRange.radius = _stat.AttackRange;
        _navAgent.speed = _pawnStat.MoveSpeed;

        Init();
    }

    protected virtual void Init() { }

    #region update
    protected virtual void UpdateDie() 
    {
        OnDeadAction?.Invoke();
    }

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
                State = Define.EPawnAniState.Idle;
                return;
        }

        //naviAgent가 이동을 마쳤을 경우 idle로 돌아감
        if (_navAgent.velocity == Vector3.zero && Vector3.Distance(_destPos, transform.position) < 0.1f)
            State = Define.EPawnAniState.Idle;


        if (_navAgent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_navAgent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1);
        }
    }

    protected virtual void OnLateUpdate() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateReady() { }
    #endregion


#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(gameObject.transform.position, gameObject.transform.forward);
    }

#endif

    #region combat system
    /// <summary>
    /// 피격시 실행되는 함수 전달받은 매개변수로 캐릭터 데이터 갱신
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public virtual bool ApplyTakeDamege(DamageMessage message)
    {
        //todo : 스탯과 스킬데이터로 정보 갱신
        SetTriggerAni(Define.EPawnAniTriger.Hit);
        _pawnStat.OnAttacked(message);
        return false;
    }

    public virtual void StartAttack()
    {

    }

    /// <summary>
    /// 타격타이밍에 실행 -> animation에서 OnHitEvent 호출시 
    /// </summary>
    public virtual void ApplyAttack()
    {
        
    }


    public virtual void ReadyShop()
    {
        //적 감지후 거리거리 내에 있을때
        //shot포인트에 캐릭터에 맞는 projectile 스폰
        //target, skill정보, damageMessage 생성
    }
    /// <summary>
    /// shot Animation Event 함수 실행시
    /// </summary>
    public virtual void ApplyShot()
    {
        //projectile 발사
    }

    #endregion

    public void DoNoting() { }

    public bool IsDead()
    {
        return _pawnStat.IsDead;
    }

    public void SetDestination(Vector3 position)
    {
        if (BoardManager.Instance.GetMoveablePosition(position, out Vector3 moveablePosition))
        {
            OnMove(moveablePosition);
        }
    }

    public virtual void OnMove(Vector3 destPosition)
    {
        _destPos = destPosition;
        State = Define.EPawnAniState.Running;
        _navAgent.SetDestination(_destPos);
    }

    /// <summary>
    /// 길찾기 종료
    /// </summary>
    public void OnStop()
    {
        _destPos = gameObject.transform.position;
        _destPos.z = 0;
        _navAgent.ResetPath();

    }

    public virtual void OnSelect()
    {
        _isSelected = true;
    }

    public virtual void OnDeSelect()
    {
        _isSelected = false;
    }

    public virtual bool IsSelected()
    {
        return _isSelected;
    }
}
