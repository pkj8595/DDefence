using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase
{
    Data.TableBase tableBase;
    public virtual string Name { get; }
    public virtual string Desc { get; }
    public virtual string ImgStr { get; }
    public int Amount { get; set; }
    public ItemBase(int tableNum)
    {
        switch (Utils.CalculateTableNum(tableNum))
        {
            case Data.CharacterData.Table:
                tableBase = Managers.Data.CharacterDict[tableNum];
                break;
            case Data.TileBaseData.Table:
                tableBase = Managers.Data.TileBaseDict[tableNum];
                break;
            case Data.BuildingData.Table:
                tableBase = Managers.Data.BuildingDict[tableNum];
                break;
            case Data.RuneData.Table:
                tableBase = Managers.Data.RuneDict[tableNum];
                break;
            case Data.GoodsData.Table:
                tableBase = Managers.Data.GoodsDict[tableNum];
                break;
            default:
                Debug.LogError($"{tableNum} 식별되지 않은 케이스");
                break;
        }
    }
    
}

public class Rune : ItemBase
{
    private Data.RuneData _runeData;

    public Rune(int tableNum) : base(tableNum)
    {
    }

    public override string Name => _runeData.name;
    public override string Desc => _runeData.desc; 
    public override string ImgStr => _runeData.imageStr;

    public void Init(Data.RuneData runeData)
    {
        _runeData = runeData;
    }

}


public class Goods : ItemBase
{
    public Goods(int tableNum) : base(tableNum)
    {
    }
    public override string Name => throw new System.NotImplementedException();

    public override string Desc => throw new System.NotImplementedException();

    public override string ImgStr => throw new System.NotImplementedException();
}