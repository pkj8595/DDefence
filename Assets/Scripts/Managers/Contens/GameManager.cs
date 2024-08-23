using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private Dictionary<Define.GoodsType, int> _goods = new();
    public Dictionary<Define.GoodsType, int> Goods { get => _goods; set => _goods = value; }

    HashSet<IDamageable> _enumyPawnGroup = new HashSet<IDamageable>();
    HashSet<IDamageable> _pawnGroup = new HashSet<IDamageable>();
    //HashSet<IDamageable> _buildingGroup { get => BoardManager.Instance._constructedBuildingList; }
    UIStateBarGroup _uiStatebarGroup;
    public Action<int> OnSpawnEvent;

    public void Init()
    {
        //_uiStatebarGroup = Managers.UI.ShowUI<UIStateBarGroup>() as UIStateBarGroup;

        foreach(var data in Managers.Data.GoodsDict)
        {
            _goods.Add((Define.GoodsType)data.Key, 0);
        }
    }

    public PawnBase SpawnPawn(int tableNum, Define.ETeam team)
    {
        GameObject go = Managers.Resource.Instantiate("Pawn/Pawn",
                    Managers.Scene.CurrentScene.GetParentObj(Define.EParentObj.Pawn).transform);

        PawnBase pawn = go.GetComponent<PawnBase>();
        pawn.Init(tableNum, team);

        if (team == Define.ETeam.Playable)
            _pawnGroup.Add(pawn);
        else
            _enumyPawnGroup.Add(pawn);

        return pawn;
    }

    public IDamageable Spawn(int tableNum, Define.ETeam team, Define.WorldObject worldType)
    {
        switch (worldType)
        {
            case Define.WorldObject.Unknown:
                break;
            case Define.WorldObject.Pawn:
                return SpawnPawn(tableNum, team);
            case Define.WorldObject.Building:
                break;
            default:
                break;
        }
        return null;
    }

    public void Despawn(IDamageable go)
    {
        switch (go.WorldObjectType)
        {
            case Define.WorldObject.Unknown:
                {
                    if (_enumyPawnGroup.Contains(go))
                    {
                        _enumyPawnGroup.Remove(go);
                        OnSpawnEvent?.Invoke(-1);
                    }
                }
                break;
            case Define.WorldObject.Pawn:
                {
                    if (go.Team == Define.ETeam.Enemy)
                    {
                        if (_enumyPawnGroup.Contains(go))
                        {
                           _enumyPawnGroup.Remove(go);
                            OnSpawnEvent?.Invoke(-1);
                        }
                    }
                    else
                    {
                        if (_pawnGroup.Contains(go))
                            _pawnGroup.Remove(go);
                    }
                }
                break;
            case Define.WorldObject.Building:
                {
                   
                }
                break;
        }
    }

    public void SetPawnInScene(PawnController pawn)
    {
        pawn.Init(pawn._testCharacterNum);
        _pawnGroup.Add(pawn);
    }

}