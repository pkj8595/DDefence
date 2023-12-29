using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : ManagerBase
{
    public Dictionary<int, Data.Stat> StatDict { get; private set; } = new Dictionary<int, Data.Stat>();
    public Dictionary<int, Data.CharacterTable> CharacterDict { get; private set; } = new Dictionary<int, Data.CharacterTable>();
    public Dictionary<int, Data.TileBaseTable> TileBaseDict { get; private set; } = new Dictionary<int, Data.TileBaseTable>();
    public Dictionary<int, Data.BuildingTable> BuildingDict { get; private set; } = new Dictionary<int, Data.BuildingTable>();
    public Dictionary<int, Data.GoodsTable> GoodsDict { get; private set; } = new Dictionary<int, Data.GoodsTable>();
    public Dictionary<int, Data.ItemTable> ItemDict { get; private set; } = new Dictionary<int, Data.ItemTable>();


    public override void Init()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/DefenceTable");
        Data.TableGroupData tableGroupData = JsonUtility.FromJson<Data.TableGroupData>(textAsset.text);

        tableGroupData.MakeCharacterTableData(CharacterDict);
        tableGroupData.MakeTileBaseTableData(TileBaseDict);
        tableGroupData.MakeBuildingTableData(BuildingDict);
        tableGroupData.MakeGoodsTableData(GoodsDict);
        tableGroupData.MakeItemTableData(ItemDict);

    }

    public Data.TableBase GetTableData(int tableNum)
    {
        Data.TableBase ret = null;
        switch (Utils.CalculateTable(tableNum))
        {
            case Data.CharacterTable.Table:
                Data.CharacterTable characterTable;
                CharacterDict.TryGetValue(tableNum,out characterTable);
                ret = characterTable;
                break;
            case Data.TileBaseTable.Table:
                Data.TileBaseTable tileBaseTable;
                TileBaseDict.TryGetValue(tableNum, out tileBaseTable);
                ret = tileBaseTable;
                break;
            case Data.BuildingTable.Table:
                Data.BuildingTable buildingTable;
                BuildingDict.TryGetValue(tableNum, out buildingTable);
                ret = buildingTable;
                break;
            case Data.GoodsTable.Table:
                Data.GoodsTable goodsTable;
                GoodsDict.TryGetValue(tableNum, out goodsTable);
                ret = goodsTable;
                break;
            case Data.ItemTable.Table:
                Data.ItemTable itemTable;
                ItemDict.TryGetValue(tableNum, out itemTable);
                ret = itemTable;
                break;
        }

        if(ret == null)
            Debug.LogError($"{tableNum} 데이터를 가져오는데 실패했습니다.");

        return ret;
    }

}
