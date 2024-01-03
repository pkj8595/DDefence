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
        public CharacterData[] CharacterTable;
        public StatData[] StatTable;
        public TileBaseData[] TileBaseTable;
        public BuildingData[] BuildingTable;
        public GoodsData[] GoodsTable;
        public ItemData[] ItemTable;


        public void MakeTableData(Dictionary<int, CharacterData> tableData)
        {
            for(int i = 0; i < CharacterTable.Length; i++)
            {
                tableData.Add(CharacterTable[i].tableNum, CharacterTable[i]);
            }
        }

        public void MakeTableData(Dictionary<int, StatData> tableData)
        {
            for (int i = 0; i < StatTable.Length; i++)
            {
                tableData.Add(StatTable[i].tableNum, StatTable[i]);
            }
        }

        public void MakeTableData(Dictionary<int, TileBaseData> tableData)
        {
            for (int i = 0; i < TileBaseTable.Length; i++)
            {
                tableData.Add(TileBaseTable[i].tableNum, TileBaseTable[i]);
            }
        }

        public void MakeTableData(Dictionary<int, BuildingData> tableData)
        {
            for (int i = 0; i < BuildingTable.Length; i++)
            {
                tableData.Add(BuildingTable[i].tableNum, BuildingTable[i]);
            }
        }

        public void MakeTableData(Dictionary<int, GoodsData> tableData)
        {
            for (int i = 0; i < GoodsTable.Length; i++)
            {
                tableData.Add(GoodsTable[i].tableNum, GoodsTable[i]);
            }
        }

        public void MakeTableData(Dictionary<int, ItemData> tableData)
        {
            for (int i = 0; i < ItemTable.Length; i++)
            {
                tableData.Add(ItemTable[i].tableNum, ItemTable[i]);
            }
        }

    }
   

    #region 101
    [Serializable]
    public class CharacterData : TableBase
    {
        public const int Table = 101;

        public string name;
        public string desc;
        public int statDataNum;
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

    #region 102 
    [Serializable]
    public class StatData : TableBase
    {
        public const int Table = 102;

        public int level;
        public int hp;
        public int mana;
        public int attack;
        public int defense;
        public float moveSpeed;
        public float attackSpeed;
        public float attackRange;
        public float bellruns;
        public int totalExp;
        public int dropExp;
    }
    #endregion

    #region 201
    [Serializable]
    public class TileBaseData : TableBase
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
    public class BuildingData : TableBase
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
    public class GoodsData : TableBase
    {
        public const int Table = 301;

        public string name;
        public string desc;
        public string imageStr;
    }
    #endregion

    #region 302
    [Serializable]
    public class ItemData : TableBase
    {
        public const int Table = 302;

        public string name;
        public string desc;
        public string imageStr;
    }
    #endregion

    
}