using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public Define.ETeam Team { get; set; }
    public Define.WorldObject WorldObjectType { get; set; }

    public abstract bool ApplyTakeDamege(DamageMessage message);
    public abstract Transform GetTransform();
    public abstract bool IsDead();
    public Stat GetStat();

    public Vector3 StateBarOffset { get; }
}

