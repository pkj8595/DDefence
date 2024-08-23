using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkill
{
    private List<Skill> _skillList = new(Define.Pawn_Rune_Limit_Count);
    private Skill _currentSkill;
    float? _manaAmount;
    public  bool IsRunning { get; private set; } = false;
    public void Init(float? manaAmount)
    {
        _manaAmount = manaAmount;
    }
    public void Release()
    {
        _manaAmount = null;
    }

    public void SetBaseSkill(Skill baseSkill)
    {
        if (0 < _skillList.Count)
            _skillList[0] = baseSkill;
        else
            _skillList.Add(baseSkill);
    }

    public void SetSkill(Skill skill)
    {
        _skillList.Add(skill);
    }

    public Skill GetCurrentSkill()
    {
        if (_currentSkill != null && _currentSkill.IsReady(_manaAmount.Value))
            return _currentSkill;

        for (int i = 1; i < _skillList.Count; i++)
        {
            if(!_skillList[i].IsReady(_manaAmount.Value))
            {
                _currentSkill = _skillList[i];
                return _currentSkill;
            }
        }
        _currentSkill = _skillList[0];
        return _currentSkill;
    }

    public Skill GetRunnigSkill()
    {
        return _currentSkill;
    }

    public bool ReadyCurrentSkill(IStat _pawnStat)
    {
        bool isReady = GetCurrentSkill().ReadySkill(_pawnStat);
        if (isReady)
            IsRunning = true;
        return isReady;
    }

    public void ClearCurrentSkill()
    {
        _currentSkill = null;
        IsRunning = false;
    }

}
