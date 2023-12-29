using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{

    [SerializeField] protected int _level;
    [SerializeField] protected int _maxHp;
    [SerializeField] protected int _maxMana;
    [SerializeField] protected int _attack;
    [SerializeField] protected int _bellruns;
    [SerializeField] protected int _defense;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected int _attackSpeed;
    [SerializeField] protected int _exp;

    [SerializeField] protected int _hp;
    [SerializeField] protected int _mana;
    [SerializeField] protected int _gold;

    public int Level { get => _level; set => _level = value; }
    public int Hp { get => _hp; set => _hp = value; }
    public int MaxHp { get => _maxHp; set => _maxHp = value; }
    public int Attack { get => _attack; set => _attack = value; }
    public int Defense { get => _defense; set => _defense = value; }
    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    public int Gold { get => _gold; set => _gold = value; }


    public int Exp
    {
        get => _exp;
        set
        {
            _exp = value;
            int level = Level;
            while (true)
            {
                if (Managers.Data.StatDict.TryGetValue(level + 1, out Data.Stat stat) == false)
                    break;
                if (_exp < stat.totalExp)
                    break;
                level++;
            }
            if (level != Level)
            {
                Debug.Log($"Level Up {Level}");
                Level = level;
                //SetStat(Level);
            }
        }
    }

    private void Start()
    {
        _level = 1;
        _hp = 100;
        _maxHp = 100;
        _attack = 10;
        _defense = 5;
        _moveSpeed = 5.0f;

        _exp = 0;
        _gold = 0;

        //SetStat(_level);
    }

    public virtual void OnAttacked(Stat attacker)
    {
        int damage = Mathf.Max(0, attacker.Attack - Defense);
        Hp -= damage;
        if (Hp < 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }

    protected virtual void OnDead(Stat attacker)
    {
        PlayerStat playerStat = attacker as PlayerStat;
        
        if (playerStat != null)
        {
            playerStat.Exp += 15;
        }

        Managers.Game.Despawn(gameObject);
    }


    private void SetStat(int tableNum)
    {
        var dict = Managers.Data.StatDict;
        Data.Stat stat = dict[tableNum];
        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
        _defense = 5;
        _moveSpeed = 5.0f;
    }


    
}
