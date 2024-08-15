using System.Collections.Generic;
using UnityEngine;

// 기본 스킬 클래스
public class Skill
{
    private Data.SkillData data;
    public string Icon { get => data.icon; }
    public string Name { get => data.name; }
    public string Desc { get => data.desc; }
    public Define.ESkillType SkillType { get => data.skillType; }
    public Define.ETargetType TargetType { get => data.targetType; }
    public float ManaAmount { get => data.manaAmount; }
    public float MinRange { get => data.minRange; }
    public float MaxRange { get => data.maxRange; }
    public float SplashRange { get => data.splashRange; }
    public float CoolTime { get => data.coolTime; }
    public Define.EPawnAniState MotionAni { get => data.motionAni; }
    public Define.EPawnAniTriger AniTriger { get => data.aniTriger; }
    public float MotionDuration { get => data.motionDuration; }
    public string EffectStr { get => data.effectStr; }
    public string ProjectileStr { get => data.projectile; }

    public List<AffectBase> AffectList { get; } = new List<AffectBase>(Define.Affect_Count);
    public float LastRunTime { get; set; }
    private PawnStat _attacker;

    public Skill(int skillTableNum)
    {
        data = Managers.Data.SkillDict[skillTableNum];
        LastRunTime = 0f;

        for (int i = 0; i < data.arr_affect.Length; i++)
        {
            if (data.arr_affect[i] != 0)
            {
                Data.SkillAffectData affectData = Managers.Data.SkillAffectDict[data.arr_affect[i]];
                AffectList.Add(AffectFactory.CreateAffect(affectData));
            }
        }
    }

    /// <summary>
    /// 스킬이 실행 가능한지 체크
    /// </summary>
    /// <param name="attacker"></param>
    /// <returns>실행 가능하면 자원을 소모하고 true 반환</returns>
    public bool ReadySkill(PawnStat attacker)
    {
        if (IsReady(attacker.Mana))
        {
            LastRunTime = Time.time;
            _attacker = attacker;
            _attacker.Mana -= ManaAmount;
            return true;
        }
        return false;
    }

    public bool DetectTargetInSkillRange(Unit obj, out List<Unit> units)
    {
        if (SkillType == Define.ESkillType.one)
        {
            units = new List<Unit>(1);
            int layer = (int)Define.Layer.Building | (int)Define.Layer.Pawn;
            var colliders = Physics.OverlapSphere(obj.transform.position, MaxRange, layer);
            foreach (Collider coll in colliders)
            {
                //minRange보다 작으면 공격 범위에서 벗어남
                if (Vector3.Distance(obj.transform.position, coll.transform.position) <= MinRange)
                    continue;

                Unit unit =  coll.GetComponent<Unit>();
                if (unit.GetTargetType(obj.Team) == TargetType)
                {
                    units.Add(unit);
                    return true;
                }
            }
        }
        else /*if (SkillType == Define.ESkillType.Range)*/
        {
            units = new List<Unit>(10);
            int layer = (int)Define.Layer.Building | (int)Define.Layer.Pawn;
            var colliders = Physics.OverlapSphere(obj.transform.position, MaxRange, layer);
            foreach (Collider coll in colliders)
            {
                Unit unit = coll.GetComponent<Unit>();
                if (unit.GetTargetType(obj.Team) == TargetType)
                {
                    units.Add(unit);
                }
            }
        }

        return units.Count > 0;
    }

    public bool IsReady(float mana)
    {
        return !IsCooltime() && IsUseableMana(mana);
    }

    public bool IsCooltime()
    {
        bool isCooltime = Time.time < LastRunTime + CoolTime;

        return isCooltime;
    }

    public bool IsUseableMana(float mana)
    {
        return mana >= ManaAmount; 
    }

    public Define.ESkillDistanceType IsExecuteableRange(float distance)
    {
        if (distance < MinRange)
            return Define.ESkillDistanceType.LessMin;
        else if (distance > MaxRange)
            return Define.ESkillDistanceType.MoreMax;
        else
            return Define.ESkillDistanceType.Excuteable;
    }


    public ProjectileBase MakeProjectile(Transform parent = null)
    {
        if (string.IsNullOrEmpty(data.projectile))
            return null;

        var obj = Managers.Resource.Instantiate(Define.Path.Prefab_Bullet + data.projectile, parent);
        return obj.GetComponent<ProjectileBase>();
    }
}