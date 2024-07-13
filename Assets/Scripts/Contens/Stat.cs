using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    private int _statDataBaseNum;
    private Data.StatData _statData;

    //todo stat클래스 정리
    [SerializeField] protected int  _level;
    [SerializeField] protected int  _exp;
    [SerializeField] protected int  _hp;
    [SerializeField] protected int  _mana;

    public int Level { get => _level; set => _level = value; }
    public int Hp { get => _hp; set => _hp = value; }
    public int Mana { get => _mana; set => _mana = value; }
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

    //어택 데미지 산출
    public virtual float OnAttack()
    {
        const float bellMaxValue = 1.0f;
        const float bellMinValue = 0.01f;

        float bell = Mathf.Clamp(Bellruns, bellMinValue, bellMaxValue);
        return Random.Range(Attack * bell, Attack);
    }

    private void OnDead(Stat attacker)
    {
        if (attacker != null)
        {
            attacker.Exp += DropExp;
        }

        Managers.Game.Despawn(gameObject);
    }

    public void Init(int statDataNum)
    {
        _statDataBaseNum = Utils.CalculateTableBaseNumber(statDataNum);
        _level = 0;
        _exp = 0;
        SetStat(CalculateStatDataNum());
        //todo init이랑 
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

    }

    private int CalculateStatDataNum()
    {
        return _statDataBaseNum + _level;
    }

}
