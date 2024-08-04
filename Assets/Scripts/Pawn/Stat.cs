using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    private int _statDataBaseNum;
    private Data.StatData _statData;

    //todo stat클래스 정리
    private int _exp;
    [field : SerializeField] public int Hp { get; set; }
    [field: SerializeField] public float CoolAtk { get; set; }
    [field: SerializeField] public float CoolSkill { get; set; }
    [field : SerializeField] public int Level { get; set; }
    [field : SerializeField] public int Mana { get; set; }

    //exp 를 
    public int Exp
    {
        get => _exp;
        private set
        {
            _exp = value;
            int level = Level;
            while (true)
            {
                if (Managers.Data.StatDict.TryGetValue(CalculateStatDataNum(), out Data.StatData stat) == false)
                    break;
                if (_exp < stat.totalExp)
                    break;
                level++;
            }
            if (level != Level)
            {
                Debug.Log($"Level Up {Level}");
                Level = level;
                SetStat(CalculateStatDataNum());
            }
        }
    }


    public int MaxHp { get => _statData.maxHp; }
    public int MaxMana { get => _statData.maxMana; }
    public int Attack { get => _statData.attack; }
    public int Defense { get => _statData.defense; }
    public float MoveSpeed { get => _statData.moveSpeed; }
    public float AttackSpeed { get => _statData.attackSpeed; }
    public float AttackRange { get => _statData.attackRange; }
    public float Bellruns { get => _statData.bellruns; }
    public int TotalExp { get => _statData.totalExp; }
    public int DropExp { get => _statData.dropExp; }
    public bool IsDead { get; set; }

    public void Init(int statDataNum)
    {
        _statDataBaseNum = Utils.CalculateTableBaseNumber(statDataNum);
        Level = 0;
        _exp = 0;
        SetStat(CalculateStatDataNum());
    }

    private void SetStat(int statDataNum)
    {
        SetStat(Managers.Data.StatDict[statDataNum]);
    }

    private void SetStat(Data.StatData statData)
    {
        _statData = statData;
        Hp = statData.maxHp;
        Mana = statData.maxMana;
        IsDead = false;
    }

    //어택 데미지 산출
    public virtual float OnAttack()
    {
        const float bellMaxValue = 1.0f;
        const float bellMinValue = 0.01f;

        float bell = Mathf.Clamp(Bellruns, bellMinValue, bellMaxValue);
        return Random.Range(Attack * bell, Attack);
    }

    //받는 데미지 계산
    public virtual void OnAttacked(Stat attacker)
    {
        int damage = (int)Mathf.Max(0, attacker.OnAttack() - Defense);
        Hp -= damage;
        if (Hp < 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }

    private void OnDead(Stat attacker)
    {
        if (attacker != null)
        {
            attacker.Exp += DropExp;
        }
        IsDead = true;
        Managers.Game.Despawn(gameObject);
    }

   
    private int CalculateStatDataNum()
    {
        return _statDataBaseNum + Level;
    }

    /// <summary>
    /// 피격시 실행되는 로직
    /// </summary>
    /// <param name="msg"></param>
    public virtual void OnAttacked(DamageMessage msg)
    {
        //todo 
        
    }

}
