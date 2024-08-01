using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;


public class BoardManager : MonoSingleton<BoardManager>
{
    enum TILE_TYPE
    {
        GROUND,
        WALL,
        COUNT
    }

    public List<GameObject> _nodeList;
    public Tilemap _groundTilemap;
    //tile 좌표
    public Dictionary<Vector3Int, TilePrefabBase> _dirTiles = new Dictionary<Vector3Int, TilePrefabBase>();

    private void Start()
    {
        LoadBoard();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //CreateNode_Resource("BaseTile");
            CreateNode_List(1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RemoveTile();
        }

    }

    #region save & load
    //todo
    private void SaveBoard()
    {

    }

    private void LoadBoard()
    {
        var nodes = _groundTilemap.GetComponentsInChildren<TilePrefabBase>();
        for (int i = 0; i < nodes.Length; i++)
        {

            int nodeListIndex = _nodeList.FindIndex((x) => {
                return nodes[i].gameObject.name.Contains(x.gameObject.name); 
            });
            
            if (nodeListIndex != -1)
            {
                nodes[i].gameObject.name = _nodeList[nodeListIndex].name;
            }
            else
            {
                Debug.LogError($"bloadManager에 등록되지않은 NodeList입니다. :{nodes[i].name}");
                return;
            }

            Vector3Int coor = _groundTilemap.WorldToCell(nodes[i].transform.position);
            if (!_dirTiles.ContainsKey(coor))
            {
                var node = nodes[i].GetComponent<TilePrefabBase>();
                //todo name change
                _dirTiles.Add(coor, node);
            }

        }
    }
    #endregion


    #region create & remove
    private void CreateNode_List(int index)
    {
        if (!(0 <= index && index < _nodeList.Count))
        {
            Debug.Log($"index가 유효하지 않습니다.{index}");
            return;
        }

        Vector3Int nodeCoordination = GetCellPositionToMouse();

        if (_dirTiles.ContainsKey(nodeCoordination))
        {
            Debug.Log($"좌표 {nodeCoordination}에 이미 저장된 값이 있음.");
            return;
        }

        GameObject node = Managers.Resource.Instantiate(_nodeList[index], _groundTilemap.transform);
        node.transform.position = _groundTilemap.GetCellCenterWorld(nodeCoordination);

        _dirTiles.Add(nodeCoordination, node.GetComponent<TilePrefabBase>());
    }

    private void CreateNode_Resource(string nodePrefabName)
    {
        Vector3Int nodeCoordination = GetCellPositionToMouse();

        if (_dirTiles.ContainsKey(nodeCoordination))
        {
            Debug.Log($"좌표 {nodeCoordination}에 이미 저장된 값이 있음.");
            return;
        }

        GameObject node = Managers.Resource.Instantiate($"Tiles/{nodePrefabName}", _groundTilemap.transform);
        node.transform.position = _groundTilemap.GetCellCenterWorld(nodeCoordination);

        _dirTiles.Add(nodeCoordination, node.GetComponent<TilePrefabBase>());
    }
    private void RemoveTile()
    {
        Vector3Int tileCoordination = GetCellPositionToMouse();

        if (_dirTiles.ContainsKey(tileCoordination))
        {
            TilePrefabBase node = _dirTiles[tileCoordination];
            _dirTiles.Remove(tileCoordination);
            Managers.Resource.Destroy(node.gameObject);
            return;
        }
    }
    #endregion



    //현재 마우스 위치를 참조해 Grid의 좌표를 가져온다.
    private Vector3Int GetCellPositionToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return _groundTilemap.WorldToCell(raycastHit.point);
        }
        Debug.LogWarning($"raycast 실패");
        return Vector3Int.zero;
    }
    



    /// <summary>
    /// 인수로 받은 worldPosition에서 Navmesh로 이동 가능한 가장 가까운 Position 반환
    /// </summary>
    /// <param name="worldPosition">world position</param>
    /// <param name="movealbePosition">이동 가능한 world position 반환</param>
    /// <returns>NavMesh.SamplePosition 성공 여부</returns>
    public bool GetMoveablePosition(Vector3 worldPosition, out Vector3 movealbePosition)
    {
        movealbePosition = Vector3.zero;
        if (NavMesh.SamplePosition(worldPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            movealbePosition = hit.position;
            return true;
        }

        return false;
    }


    /* 맵 생성자
     * 
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
    */


}
