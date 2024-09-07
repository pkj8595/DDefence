using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand 
{
    void Execute(Unit unit);
}

public class MoveCommand : ICommand
{
    private Vector3 targetPosition;

    public MoveCommand SetPosition(Vector3 position)
    {
        targetPosition = position;
        return this;
    }

    public void Execute(Unit unit)
    {
        PawnBase pawn = unit as PawnBase;
        pawn.SetDestination(targetPosition);
    }
}