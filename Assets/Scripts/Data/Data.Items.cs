using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase
{
    Data.TableBase tableBase;

    public int GetTableNum => tableBase.tableNum;
    public virtual string Name { get; }
    public virtual string Desc { get; }
    public virtual string ImgStr { get; }
    public int Amount { get; set; }
    protected virtual void Init(Data.TableBase tableBase)
    {

    }

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
        Init(tableBase);
    }
}

public class Rune : ItemBase
{
    private Data.RuneData _runeData;

    public override string Name => _runeData.name;
    public override string Desc => _runeData.desc; 
    public override string ImgStr => _runeData.imageStr;

    protected override void Init(Data.TableBase tableBase)
    {
        base.Init(tableBase);
        _runeData = tableBase as Data.RuneData;
    }
    public Rune(int tableNum) : base(tableNum) { }

}


public class Goods : ItemBase
{

    private Data.GoodsData _goodsData;
    public override string Name => _goodsData.name;
    public override string Desc => _goodsData.desc;
    public override string ImgStr => _goodsData.imageStr;

    protected override void Init(TableBase tableBase)
    {
        base.Init(tableBase);
        _goodsData = tableBase as GoodsData;
    }
    public Goods(int tableNum) : base(tableNum) { }

}