using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    //Dictionary<int, GameObject> _monster = new Dictionary<int, GameObject>();
    //Dictionary<int, GameObject> _eventObj = new Dictionary<int, GameObject>(); // 상호작용 오브젝트

    HashSet<GameObject> _monster = new HashSet<GameObject>();
    HashSet<PawnGroup> _pawnGroup = new HashSet<PawnGroup>();

    public Action<int> OnSpawnEvent;

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Unknown:
                break;
            case Define.WorldObject.PlayerPawn:
                break;
        }
        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        PawnBase bc = go.GetComponent<PawnBase>();
        if (bc == null)
            return Define.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);
        switch (type)
        {
            case Define.WorldObject.Unknown:
                {
                    if (_monster.Contains(go))
                    {
                        _monster.Remove(go);
                        OnSpawnEvent?.Invoke(-1);
                    }
                }
                break;
            case Define.WorldObject.PlayerPawn:
                {
                }
                break;
        }
    }


}