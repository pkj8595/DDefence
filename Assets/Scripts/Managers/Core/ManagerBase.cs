using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBase
{
    public virtual void Init() { }
    public virtual void OnUpdate() { }
    public virtual void Clear() { }
}
