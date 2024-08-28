using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

public class EnemyFindTarget : MonoBehaviour
{
    PawnBase _pawnBase;
    NavMeshAgent _agent;
    
    public void Init()
    {
        _pawnBase = GetComponent<PawnBase>();
        _agent = GetComponent<NavMeshAgent>();
        StartSearchTask().Forget();
    }

    async UniTaskVoid StartSearchTask()
    {
        while (true)
        {
            if (_pawnBase.IsDead())
                return;

            if (!_pawnBase.HasTarget &&
                (_pawnBase.State == Define.EPawnAniState.Idle ||
                 _pawnBase.State == Define.EPawnAniState.Ready))
            {
                if (SearchTargetPosition(out Vector3 retPosition))
                {
                    _pawnBase.SetDestination(retPosition);
                }
            }

            await UniTask.Delay(1000,cancellationToken: gameObject.GetCancellationTokenOnDestroy());
        }
    }


    public bool SearchTargetPosition(out Vector3 retPosition)
    {
        bool isFind = false;
        retPosition = Vector3.zero;
        float distance = float.MaxValue;
        NavMeshPath path = new NavMeshPath();
        var buildingList = GameView.Instance.ConstructedBuildingList;
        foreach(var building in buildingList)
        {
            if (building.TryGetComponent(out IDamageable damageable) && !damageable.IsDead())
            {
                if (BoardManager.Instance.GetMoveablePosition(damageable.GetTransform().position,
                                                              out Vector3 moveablePosition,3f))
                {
                    _agent.CalculatePath(moveablePosition, path);
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        float pathLength = GetPathLength(path);
                        if (pathLength < distance)
                        {
                            distance = pathLength;
                            retPosition = moveablePosition;
                            isFind = true;
                        }
                    }

                }
            }
        }
        return isFind;
    }

    float GetPathLength(NavMeshPath path)
    {
        float length = 0.0f;
        if (path.corners.Length < 2)
            return length;

        for (int i = 1; i < path.corners.Length; i++)
        {
            length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return length;
    }

}
