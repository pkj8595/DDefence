using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    enum TILE_TYPE
    {
        GROUND,
        WALL,
        COUNT
    }

    public Tilemap _groundTilemap;
    public Tilemap _colliderTilemap;
    public List<TileBase> _tileBaseList = new List<TileBase>();


    public void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateTile(TILE_TYPE.WALL);
            CreateTileObject("TilePrefabBase");
        }
        else if (Input.GetMouseButton(1))
        {
            RemoveTile();
        }
    }


    private void CreateTile(TILE_TYPE tileType)
    {
        _colliderTilemap.SetTile(GetCellPositionToMouse(), _tileBaseList[(int)tileType]);
    }

    private void RemoveTile()
    {
        _colliderTilemap.SetTile(GetCellPositionToMouse(), null);
    }

    private Vector3Int GetCellPositionToMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return _colliderTilemap.WorldToCell(mouseWorldPosition);
    }

    //
    private void CreateTileObject(string tilePrefabName)
    {
        Vector3 cellWorldPosition = _colliderTilemap.GetCellCenterWorld(GetCellPositionToMouse());
        GameObject testTile = Managers.Resource.Instantiate($"Tiles/{tilePrefabName}");
        testTile.transform.position = cellWorldPosition;
    }
}
