using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand 
{
    bool CanMove();
    bool CanAttack();
    bool CanBuild();
    bool CanGather();

    void MoveTo(Vector3 targetPosition);
    void Attack(Vector3 targetPosition);
    void Build(Vector3 targetPosition);
    void GatherResource(Vector3 targetPosition);
}
