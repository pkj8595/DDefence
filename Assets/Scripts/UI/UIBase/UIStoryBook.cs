using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using System.Text;

public class UIStoryBook : UIBase
{
    public Text _txtTitle;
    public Text _txtContent;
    public List<ChoiceItem> _btnChoiceItem;
    public GameObject _btnClose;
    private int _storyDataNum;

    public override void Init(UIData uiData)
    {
        base.Init(uiData);
        var data = uiData as UIStoryData;
        _storyDataNum = data.StoryDataNum;
        _btnClose.SetActive(false);
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        var data = Managers.Data.StoryDict[_storyDataNum];
        _txtTitle.text = data.name;
        _txtContent.text = data.desc;

        int choiceCount = 0;
        for (int i = 0; i < data.arr_choice.Length; i++)
        {
            if (data.arr_choice[i] != 0)
            {
                choiceCount++;
                var choiseData = Managers.Data.StoryChoiceDict[data.arr_choice[i]];
                _btnChoiceItem[i].SetData(choiseData);
                _btnChoiceItem[i].SetActive(true);

                //요구되는 아이템이 충분하다면 true
                if (CheckRequiredItem(choiseData))
                    _btnChoiceItem[i].SetBlockActive(false);
                else
                    _btnChoiceItem[i].SetBlockActive(true);

            }
            else
                _btnChoiceItem[i].SetActive(false);
        }
        
        if (choiceCount == 0)
        {
            _btnClose.SetActive(true);
        }
    }

    /// <summary>
    /// 선택지가 요구하는 자원 체크 
    /// </summary>
    /// <param name="choiseData"></param>
    private bool CheckRequiredItem(Data.StoryChoiceData choiseData)
    {
        for (int j = 0; j < choiseData.arr_requiredGoods.Length; j++)
        {
            //소모되는 수량이 충분하지 않다면 false 리턴
            if (!Managers.Game.Inven.CheckItem(choiseData.arr_requiredGoods[j],
                                              choiseData.arr_requiredAmount[j]))
            {
                return false;
            }
        }
        return true;
    }

    public void OnClickChoice(int index)
    {
        //선택지가 선택되면 선택지 hide
        for (int i = 0; i < _btnChoiceItem.Count; i++)
        {
            _btnChoiceItem[i].SetActive(false);
        }

        var data = _btnChoiceItem[index].ChoiceData;

        //얻을 수 있는 데이터가 있다면 추가
        for (int i = 0; i < data.arr_getItem[i]; i++)
        {
            if(data.arr_getItem[i] != 0)
            {
                Managers.Game.Inven.AddItem(data.arr_getItem[i], data.arr_getItemAmount[i]);
            }
        }

        //추가 되는 웨이브가 있다면 추가
        if (data.pushWave != 0)
            Managers.Game.WaveStack.Push(Managers.Data.WaveDict[data.pushWave]);

        var waveData = Managers.Game.WaveStack.Peek();
        StringBuilder builder = new StringBuilder();
        builder.Append(data.desc);
        builder.Append("\n\n");
        builder.Append(waveData.desc);
        _txtContent.text = builder.ToString();
        _btnClose.SetActive(true);
    }

    public override void OnClickClose()
    {
        base.OnClickClose();
        if (_storyDataNum == 111999999)
        {
            Managers.Scene.LoadScene(Define.Scene.Ending);
        }

        Managers.Game.NextPhase();
    }


}

[Serializable]
public class ChoiceItem
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Text _txtName;
    [SerializeField] private Text _txtDesc;
    [SerializeField] private GameObject _imgBlock;
    public Data.StoryChoiceData ChoiceData { get; set; }

    public void SetData(Data.StoryChoiceData data)
    {
        _txtName.text = data.name;
        //todo item Name and Amount
        StringBuilder requiredItemStr = new();
        for (int i = 0; i < data.arr_requiredGoods.Length; i++)
        {
            if (data.arr_requiredGoods[i] != 0)
            {
                requiredItemStr.Append(GetItemAmountStr(new ItemBase(data.arr_getItem[i]),
                                                        data.arr_getItemAmount[i]));
            }
        }

        _txtDesc.text = requiredItemStr.ToString();
        ChoiceData = data;
    }

    public void SetBlockActive(bool isBlock)
    {
        _imgBlock.SetActive(isBlock);
        if (isBlock)
            SetEmptyData();
    }

    public void SetEmptyData()
    {
        _txtName.text = "?????";
        _txtDesc.text = string.Empty;
    }

    public string GetItemAmountStr(ItemBase itemBase, int amount)
    {
        return $"{itemBase.Name} : {amount}\n";
    }

    public void SetActive(bool isActive)
    {
        _gameObject.SetActive(isActive);
    }
}