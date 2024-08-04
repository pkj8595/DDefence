using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;


public class BoardManager : MonoSingleton<BoardManager>
{
    private MeshFilter mergedMeshFilter;
    private MeshRenderer mergedMeshRenderer;
    private MeshCollider mergedMeshCollider;
    [SerializeField] private GameObject combineMeshObject;
    [SerializeField] private GameObject _nodeGroup;
    [SerializeField] private NavMeshSurface navSurface;

    [SerializeField] private List<GameObject> _nodeList;
    [SerializeField] private Dictionary<Vector3Int, TilePrefabBase> _dirTiles = new();
    [SerializeField] private Vector3 tileSize = new Vector3(1, 1, 1); // 각 셀의 크기
    [SerializeField] private Vector3 tileOffset = new Vector3(0, 0, 0); // 각 셀의 크기

    private bool _isEditMode = false;
    private bool _isSelectNode = false;
    private int _selectedNodeIndex = 0;

    [SerializeField] private MeshFilter _previewNode;
    [SerializeField] private Material _previewMat;


    private void Start()
    {
        /*if (combineMeshObject == null)
        {
            combineMeshObject = GameObject.Find("CombineMesh");
        }

        if (navSurface == null)
        {
            navSurface = GetComponentInChildren<NavMeshSurface>();
        }

        mergedMeshFilter = combineMeshObject.GetOrAddComponent<MeshFilter>();
        mergedMeshRenderer = combineMeshObject.GetOrAddComponent<MeshRenderer>();
        mergedMeshCollider = combineMeshObject.GetOrAddComponent<MeshCollider>();

        LoadBoard().Forget();

        Managers.Input.KeyAction -= OnKeyAction;
        Managers.Input.KeyAction += OnKeyAction;
        Managers.Input.MouseAction -= OnMouseAction;
        Managers.Input.MouseAction += OnMouseAction;*/
    }
    
    public async UniTask AsyncInit()
    {
        await UniTask.NextFrame();
        if (combineMeshObject == null)
        {
            combineMeshObject = GameObject.Find("CombineMesh");
        }

        if (navSurface == null)
        {
            navSurface = GetComponentInChildren<NavMeshSurface>();
        }

        mergedMeshFilter = combineMeshObject.GetOrAddComponent<MeshFilter>();
        mergedMeshRenderer = combineMeshObject.GetOrAddComponent<MeshRenderer>();
        mergedMeshCollider = combineMeshObject.GetOrAddComponent<MeshCollider>();

        LoadBoard().Forget();

        Managers.Input.KeyAction -= OnKeyAction;
        Managers.Input.KeyAction += OnKeyAction;
        Managers.Input.MouseAction -= OnMouseAction;
        Managers.Input.MouseAction += OnMouseAction;

    }


    void Update()
    {
        if (_isEditMode && _isSelectNode)
        {
            //프리뷰 노드 보여주기
            (Vector3Int position, Vector3 normal) nodeMouse = GetCellPositionToMouse();
            _previewNode.gameObject.transform.position = GridToWorld(nodeMouse.position);
            if (nodeMouse.normal != Vector3.zero)
            {
                _previewNode.gameObject.transform.rotation = Quaternion.LookRotation(nodeMouse.normal, Vector3.up);
            }
        }
    }

    private void OnKeyAction()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isEditMode )
            RemoveTile();

        if (Input.GetKeyDown(KeyCode.Space))
            ChangeEditMode();
    }

    private void OnMouseAction(Define.MouseEvent evt)
    {
        if (Define.MouseEvent.Press == evt && _isEditMode && _isSelectNode)
        {
            CreateNode_List(_selectedNodeIndex);
        }
    }

    private void ChangeEditMode()
    {
        _isEditMode = !_isEditMode;
        if (_isEditMode)
        {
            if (_previewNode == null)
            {
                _previewNode = new GameObject("previewNode").GetOrAddComponent<MeshFilter>();
                _previewNode.gameObject.GetOrAddComponent<MeshRenderer>().material = _previewMat;
            }
            AsyncUnmergeMeshes().Forget();
        }
        else
        {
            _isSelectNode = false;
            _previewNode.gameObject.SetActive(false);
            AsyncMergeAllMeshes().Forget();
        }
    }

    #region save & load
    //todo
    private void SaveBoard()
    {

    }

    async UniTask LoadBoard()
    {
        await UniTask.NextFrame();
        _dirTiles.Clear();
        var nodes = _nodeGroup.GetComponentsInChildren<TilePrefabBase>();
        for (int i = 0; i < nodes.Length; i++)
        {

            int nodeListIndex = _nodeList.FindIndex((x) =>
            {
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

            Vector3Int coor = WorldToGrid(nodes[i].transform.position);
            if (!_dirTiles.ContainsKey(coor))
            {
                var node = nodes[i].GetComponent<TilePrefabBase>();
                node.Init(coor);
                _dirTiles.Add(coor, node);
            }
        }
        AsyncMergeAllMeshes().Forget();

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

        (Vector3Int position, Vector3 normal) nodeCoordination = GetCellPositionToMouse();

        if (_dirTiles.ContainsKey(nodeCoordination.position))
        {
            Debug.Log($"좌표 {nodeCoordination}에 이미 저장된 값이 있음.");
            return;
        }

        //node setting
        TilePrefabBase node = Managers.Resource.Instantiate(_nodeList[index], _nodeGroup.transform).GetComponent<TilePrefabBase>();
        node.transform.position = GridToWorld(nodeCoordination.position);
        node.Init(nodeCoordination.position);
        if (nodeCoordination.normal != Vector3.zero)
            node.transform.rotation = Quaternion.LookRotation(nodeCoordination.normal, Vector3.up);
        node.SetActive(true);
        //save
        _dirTiles.Add(nodeCoordination.position, node);
    }

    /*private void CreateNode_Resource(string nodePrefabName)
    {
        Vector3Int nodeCoordination = GetCellPositionToMouse();

        if (_dirTiles.ContainsKey(nodeCoordination))
        {
            Debug.Log($"좌표 {nodeCoordination}에 이미 저장된 값이 있음.");
            return;
        }

        GameObject node = Managers.Resource.Instantiate($"Tiles/{nodePrefabName}", _grid.transform);
        node.transform.position = _grid.GetCellCenterWorld(nodeCoordination);

        _dirTiles.Add(nodeCoordination, node.GetComponent<TilePrefabBase>());
    }*/

    private void RemoveTile()
    {
        TilePrefabBase node = GetNodeToMouse();
        if (node == null)
            return;

        if (_dirTiles.ContainsKey(node.TilePosition))
        {
            _dirTiles.Remove(node.TilePosition);
            node.SetActive(false);
            return;
        }
    }
    #endregion

    private TilePrefabBase GetNodeToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = (int)Define.Layer.Ground;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, layerMask))
        {
            return raycastHit.collider.GetComponent<TilePrefabBase>();
        }
        return null;
    }

    //현재 마우스 위치를 참조해 Grid의 좌표를 가져온다.
    private (Vector3Int, Vector3) GetCellPositionToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = (int)Define.Layer.Water | (int)Define.Layer.Ground;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
        {
            if (1 << hit.transform.gameObject.layer == (int)Define.Layer.Water)
            {
                return (WorldToGrid(hit.point), Vector3.zero);
            }
            else
            {
                return (WorldToGrid(hit.transform.position + hit.normal), hit.normal);
            }
        }
        return (Vector3Int.zero, Vector3.zero);
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변환
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public Vector3 GridToWorld(Vector3Int gridPosition)
    {
        float x = gridPosition.x * tileSize.x + tileOffset.x;
        float y = gridPosition.y * tileSize.y + tileOffset.z;
        float z = gridPosition.z * tileSize.z + tileOffset.y;
        return new Vector3(x, y, z);
    }



    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변환
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Vector3 SnapPosition(Vector3 point)
    {
        float snappedX = Mathf.Round(point.x / tileSize.x) * tileSize.x + tileOffset.x;
        float snappedZ = Mathf.Round(point.z / tileSize.z) * tileSize.z + tileOffset.z;
        float snappedY = Mathf.Round(point.y / tileSize.y) * tileSize.y + tileOffset.y;

        return new Vector3(snappedX, snappedY, snappedZ);
    }

    public Vector3Int WorldToGrid(Vector3 poiint)
    {
        return Vector3Int.FloorToInt(poiint + (Vector3.one * 0.5f));
    }

    public int Round(float value)
    {
        return (int)(value + 0.5);
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


    /// <summary>
    /// Node 메쉬 병합
    /// </summary>
    async UniTaskVoid AsyncMergeAllMeshes()
    {
        await UniTask.NextFrame();

        List<MeshFilter> meshFilters = new List<MeshFilter>();

        foreach (var item in _dirTiles)
        {
            meshFilters.Add(item.Value.GetComponent<MeshFilter>());
        }

        if (meshFilters.Count == 0) return;

        CombineInstance[] combine = new CombineInstance[meshFilters.Count];
        for (int i = 0; i < meshFilters.Count; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine, true);

        mergedMeshFilter.mesh = combinedMesh;
        mergedMeshCollider.sharedMesh = combinedMesh;


        // 병합 후 개별 메쉬 오브젝트 비활성화
        _nodeGroup.gameObject.SetActive(false);

        // 병합된 메쉬의 재질 설정 (첫 번째 메쉬의 재질 사용)
        if (meshFilters.Count > 0)
        {
            mergedMeshRenderer.sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;
        }
        navSurface.BuildNavMesh();
    }

    /// <summary>
    /// 병합 해제
    /// </summary>
    async UniTaskVoid AsyncUnmergeMeshes()
    {
        await UniTask.NextFrame();
        if (mergedMeshFilter.mesh != null)
        {
            Destroy(mergedMeshFilter.mesh);
            mergedMeshFilter.mesh = null;
            mergedMeshCollider.sharedMesh = null;
        }

        // nodeGroup 활성화
        _nodeGroup.gameObject.SetActive(true);
    }

    public void SetNodeIndex(int index)
    {
        if (!_isEditMode)
            return;

        _isSelectNode = true;
        _selectedNodeIndex = index;
        _previewNode.mesh = _nodeList[_selectedNodeIndex].GetComponent<MeshFilter>().sharedMesh;
        _previewNode.gameObject.SetActive(true);
    }

}
