using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SCENE_TYPE { get; protected set; } = Define.Scene.Unknown;

    void Awake()
    {
        Managers.InitManagers();
        Init();
    }

    protected virtual void Init()
    {
       
    }

    public abstract void Clear();

    public virtual GameObject GetParentObj(Define.EParentObj obj)
    {
        return null;
    }

}
