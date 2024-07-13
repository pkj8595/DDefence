using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager 
{
    //Dictionary<int, GameObject> _monster = new Dictionary<int, GameObject>();
    //Dictionary<int, GameObject> _player = new Dictionary<int, GameObject>();
    //Dictionary<int, GameObject> _eventObj = new Dictionary<int, GameObject>(); // 상호작용 오브젝트

    GameObject _player;
    HashSet<GameObject> _monster = new HashSet<GameObject>();
    public CombatSystem _combatSystem = new CombatSystem();

    public Action<int> OnSpawnEvent;
    public GameObject GetPlayer() { return _player; }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Unknown:
                break;
            case Define.WorldObject.Player:
                _player = go;
                break;
        }
        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
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
            case Define.WorldObject.Player:
                {
                    if (_player == go)
                        _player = null;
                }
                break;
        }
    }

    /// <summary>
    /// 1. 공격자가 attack class를 상속받은 공격 클래스를 combatSystem에 보낸다.(공격스킬 확정)
    /// 2. 공격자가 대상으로한 타겟(근접) 및 position(원거리 공격)를 인수로 받는다.
    /// 3. 방어자의 stat를 계산하고 데미지를 준다.
    /// </summary>
    /// <param name="attack"></param>
    public void SetAttack(Attack attack, GameObject obj)
    {
        //_combatSystem.SetAttack(attack);
    }
    

}