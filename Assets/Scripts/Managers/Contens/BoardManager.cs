using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using NavMeshPlus.Components;
using UnityEngine.AI;

public class BoardManager : MonoSingleton<BoardManager>
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeMap();
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

    /// <summary>
    /// update navimesh
    /// </summary>
    public void UpdateNavimeshSurface()
    {
        _surface2D.BuildNavMeshAsync();
    }

    /// <summary>
    /// 인수로 받은 worldPosition에서 Navmesh로 이동 가능한 가장 가까운 Position 반환
    /// </summary>
    /// <param name="worldPosition">world position</param>
    /// <param name="movealbePosition">이동 가능한 world position 반환</param>
    /// <returns>NavMesh.SamplePosition 성공 여부</returns>
    public bool GetMoveablePosition(Vector2 worldPosition, out Vector2 movealbePosition)
    {
        movealbePosition = Vector2.zero;
        if (NavMesh.SamplePosition(worldPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            movealbePosition = hit.position;
            return true;
        }

        return false;
    }

    public void MakeMap()
    {
        //todo :: 이벤트 방 만들어서 랜덤 배치하기 
        int x = 200;
        int y = 140;
        MapGenerator generator = new MapGenerator();
        List<List<int>> wallMap = generator.MakeMap(x, y, null, true, 56);

        _groundTilemap.ClearAllTiles();
        _colliderTilemap.ClearAllTiles();

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                _groundTilemap.SetTile(new Vector3Int(i, j, 0), _tileBaseList[TILE_TYPE.GROUND.ToInt()]);
                if (wallMap[j][i] == 1)
                    _colliderTilemap.SetTile(new Vector3Int(i, j, 0), _tileBaseList[TILE_TYPE.WALL.ToInt()]);
            }
        }

        //UpdateNavimeshSurface();
    }

}
