using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSlot : UI_Slot
{
    [SerializeField] private Image _imgSlot;
    [SerializeField] private Image _imgCoolTime;
    [SerializeField] private Text _txtName;
    [SerializeField] private Text _txtCoolTime;
    Skill _skill;

    public void Init(Skill skill)
    {
        HasData = skill != null;
        _skill = skill;
        _imgSlot.sprite = Managers.Resource.Load<Sprite>($"{Define.Path.UIIcon}{skill.Icon}");
        _txtName.text = skill.Name;
        _imgCoolTime.fillAmount = 1.0f;
    }

    public void Update()
    {
        if (this.gameObject.activeSelf && _skill != null)
        {
            float percent = _skill.GetCulcalatePercentCoolTime();
            _imgCoolTime.fillAmount = percent;
            _txtCoolTime.gameObject.SetActive(percent != 0f);
            _txtCoolTime.text = (_skill.CoolTime - (Time.time - _skill.LastRunTime)).ToString("0.0");
        }
    }

    public override string SetTitleStr()
    {
        return _skill.Name;
    }

    public override string SetDescStr()
    {
        Data.SkillData data = Managers.Data.SkillDict[_skill.TableNum];
        System.Text.StringBuilder str = new();
        str.Append(_skill.Desc);
        str.Append("\n\n");
        str.Append(Utils.GetSkillStr(data));
        str.Append("\n\n");
        for (int i = 0; i < data.arr_affect.Length; i++)
        {
            if (data.arr_affect[i] != 0)
            {
                str.Append($"\nEffect { i + 1 }\n");
                str.Append(Utils.GetSkillAffect(Managers.Data.SkillAffectDict[data.arr_affect[i]]));
            }
        }

        return str.ToString();
    }
    
}
