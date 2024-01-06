using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using NavMeshPlus.Components;

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
    public NavMeshSurface _surface2D;


    public void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            CreateTile(TILE_TYPE.WALL);
            //CreateTileObject("TilePrefabBase");

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            RemoveTile();
        }
    }


    private void CreateTile(TILE_TYPE tileType)
    {
        _colliderTilemap.SetTile(GetCellPositionToMouse(), _tileBaseList[(int)tileType]);
        UpdateNavimeshSurface();

    }

    private void RemoveTile()
    {
        _colliderTilemap.SetTile(GetCellPositionToMouse(), null);
        UpdateNavimeshSurface();
    }

    private Vector3Int GetCellPositionToMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return _colliderTilemap.WorldToCell(mouseWorldPosition);
    }

    private void CreateTileObject(string tilePrefabName)
    {
        Vector3 cellWorldPosition = _colliderTilemap.GetCellCenterWorld(GetCellPositionToMouse());
        GameObject testTile = Managers.Resource.Instantiate($"Tiles/{tilePrefabName}", gameObject.transform);
        testTile.transform.position = cellWorldPosition;
    }

    public void UpdateNavimeshSurface()
    {
        _surface2D.BuildNavMeshAsync();
    }
}
