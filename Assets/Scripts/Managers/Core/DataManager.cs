using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : ManagerBase
{
    public Dictionary<int, Data.CharacterData> CharacterDict { get; private set; } = new Dictionary<int, Data.CharacterData>();
    public Dictionary<int, Data.StatData> StatDict { get; private set; } = new Dictionary<int, Data.StatData>();
    public Dictionary<int, Data.TileBaseData> TileBaseDict { get; private set; } = new Dictionary<int, Data.TileBaseData>();
    public Dictionary<int, Data.BuildingData> BuildingDict { get; private set; } = new Dictionary<int, Data.BuildingData>();
    public Dictionary<int, Data.GoodsData> GoodsDict { get; private set; } = new Dictionary<int, Data.GoodsData>();
    public Dictionary<int, Data.ItemData> ItemDict { get; private set; } = new Dictionary<int, Data.ItemData>();


    public override void Init()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/DefenceTable");
        Data.TableGroupData tableGroupData = JsonUtility.FromJson<Data.TableGroupData>(textAsset.text);

        tableGroupData.MakeTableData(CharacterDict);
        tableGroupData.MakeTableData(StatDict);
        tableGroupData.MakeTableData(TileBaseDict);
        tableGroupData.MakeTableData(BuildingDict);
        tableGroupData.MakeTableData(GoodsDict);
        tableGroupData.MakeTableData(ItemDict);

    }

    public Data.TableBase GetTableData(int tableNum)
    {
        Data.TableBase ret = null;
        switch (Utils.CalculateTable(tableNum))
        {
            case Data.CharacterData.Table:
                Data.CharacterData characterTable;
                CharacterDict.TryGetValue(tableNum,out characterTable);
                ret = characterTable;
                break;
            case Data.StatData.Table:
                Data.StatData statTable;
                StatDict.TryGetValue(tableNum, out statTable);
                ret = statTable;
                break;
            case Data.TileBaseData.Table:
                Data.TileBaseData tileBaseTable;
                TileBaseDict.TryGetValue(tableNum, out tileBaseTable);
                ret = tileBaseTable;
                break;
            case Data.BuildingData.Table:
                Data.BuildingData buildingTable;
                BuildingDict.TryGetValue(tableNum, out buildingTable);
                ret = buildingTable;
                break;
            case Data.GoodsData.Table:
                Data.GoodsData goodsTable;
                GoodsDict.TryGetValue(tableNum, out goodsTable);
                ret = goodsTable;
                break;
            case Data.ItemData.Table:
                Data.ItemData itemTable;
                ItemDict.TryGetValue(tableNum, out itemTable);
                ret = itemTable;
                break;
        }

        if(ret == null)
            Debug.LogError($"{tableNum} 데이터를 가져오는데 실패했습니다.");

        return ret;
    }

}
