using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Data
{
    [Serializable]
    public class TableBase
    {
        public int tableNum;
    }

    [Serializable]
    public class TableGroupData
    {
        public CharacterTable[] CharacterTable;
        public TileBaseTable[] TileBaseTable;
        public BuildingTable[] BuildingTable;
        public GoodsTable[] GoodsTable;
        public ItemTable[] ItemTable;


        public void MakeCharacterTableData(Dictionary<int, CharacterTable> tableData)
        {
            for(int i = 0; i < CharacterTable.Length; i++)
            {
                tableData.Add(CharacterTable[i].tableNum, CharacterTable[i]);
            }
        }

        public void MakeTileBaseTableData(Dictionary<int, TileBaseTable> tableData)
        {
            for (int i = 0; i < TileBaseTable.Length; i++)
            {
                tableData.Add(TileBaseTable[i].tableNum, TileBaseTable[i]);
            }
        }

        public void MakeBuildingTableData(Dictionary<int, BuildingTable> tableData)
        {
            for (int i = 0; i < BuildingTable.Length; i++)
            {
                tableData.Add(BuildingTable[i].tableNum, BuildingTable[i]);
            }
        }

        public void MakeGoodsTableData(Dictionary<int, GoodsTable> tableData)
        {
            for (int i = 0; i < GoodsTable.Length; i++)
            {
                tableData.Add(GoodsTable[i].tableNum, GoodsTable[i]);
            }
        }

        public void MakeItemTableData(Dictionary<int, ItemTable> tableData)
        {
            for (int i = 0; i < ItemTable.Length; i++)
            {
                tableData.Add(ItemTable[i].tableNum, ItemTable[i]);
            }
        }


    }


    #region Stat
    [Serializable]
    public class Stat 
    {
        public int level;
        public int maxHp;
        public int attack;
        public int totalExp;
    }

    
    #endregion

    #region 101
    [Serializable]
    public class CharacterTable : TableBase
    {
        public const int Table = 101;

        public string name;
        public string desc;
        public int attackType;
        public string Head;
        public string Ears;
        public string Eyes;
        public string Body;
        public string Hair;
        public string Armor;
        public string Helmet;
        public string Weapon;
        public string Shield;
        public string Cape;
        public string Back;
        public string Mask;
        public string Horns;
    }
    #endregion

    #region 201
    [Serializable]
    public class TileBaseTable : TableBase
    {
        public const int Table = 201;

        public int tileType;
        public string name;
        public string desc;
        public int layer;
        public int isTrigger;
    }
    #endregion

    #region 202
    [Serializable]
    public class BuildingTable : TableBase
    {
        public const int Table = 202;

        public int tileType;
        public string name;
        public string desc;
        public int layer;
        public int isTrigger;
    }
    #endregion

    #region 301
    [Serializable]
    public class GoodsTable : TableBase
    {
        public const int Table = 301;

        public string name;
        public string desc;
        public string imageStr;
    }
    #endregion

    #region 302
    [Serializable]
    public class ItemTable : TableBase
    {
        public const int Table = 302;

        public string name;
        public string desc;
        public string imageStr;
    }
    #endregion

    
}