using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoardManager : MonoSingleton<BoardManager>
{
    private MeshFilter mergedMeshFilter;
    private MeshRenderer mergedMeshRenderer;
    private MeshCollider mergedMeshCollider;
    [SerializeField] private GameObject _combineMeshObject;
    [SerializeField] private GameObject _nodeGroup;
    [SerializeField] private GameObject _buildingGroup;
    [SerializeField] private NavMeshSurface navSurface;

    [SerializeField] private List<GameObject> _nodeList;
    [SerializeField] private List<GameObject> _buildingList;

    [SerializeField] private Dictionary<Vector3Int, NodeBase> _dirNodes = new();
    private List<BuildingNode> _constructedBuildingList { get => GameView.Instance.ConstructedBuildingList; }
    [SerializeField] private Vector3 tileSize = new Vector3(1, 1, 1); // 각 셀의 크기

    private bool _isEditMode = false;
    private bool _isSelectNode = false;
    private int _selectedNodeIndex = -1;
    private bool _isCard = false;

    [SerializeField] private NodeBase _previewNode;
    [SerializeField] private Material _previewMaterial_Green;
    [SerializeField] private Material _previewMaterial_Red;

    private (Vector3Int position, Vector3 normal) _beforeMousePosition;
    private string _cardItemPath;

    private void Start()
    {
        if (_combineMeshObject == null)
        {
            _combineMeshObject = GameObject.Find("CombineMesh");
        }

        if (navSurface == null)
        {
            navSurface = GetComponentInChildren<NavMeshSurface>();
        }

        mergedMeshFilter = _combineMeshObject.GetOrAddComponent<MeshFilter>();
        mergedMeshRenderer = _combineMeshObject.GetOrAddComponent<MeshRenderer>();
        mergedMeshCollider = _combineMeshObject.GetOrAddComponent<MeshCollider>();

        LoadBoard();

        Managers.Input.KeyAction -= OnKeyAction;
        Managers.Input.KeyAction += OnKeyAction;
        Managers.Input.MouseAction -= OnMouseAction;
        Managers.Input.MouseAction += OnMouseAction;
    }

    void Update()
    {
        if (_isEditMode && _isSelectNode || _isCard)
        {
            //프리뷰 노드 보여주기
            (Vector3Int position, Vector3 normal) nodeMouse = GetCellPositionToMouse();
            if (Input.GetKeyDown(KeyCode.F))
                Debug.Log(nodeMouse);
            if (_beforeMousePosition.position == nodeMouse.position &&
                _beforeMousePosition.normal == nodeMouse.normal)
                return;
            _beforeMousePosition = nodeMouse;



            _previewNode.transform.position = ComputeNodeScalePosition(_previewNode, nodeMouse.position);
            _previewNode.Position = nodeMouse.position;
            _previewNode.SetNodeRotation(nodeMouse.normal);

            ChangeMaterialPreviewNode(CanPlaceBuilding(nodeMouse.position, _previewNode.NodeSize, _previewNode));
        }
    }

    private void OnKeyAction()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isEditMode )
            RemoveTile();

        if (Input.GetKeyDown(KeyCode.Space))
            ChangeEditMode();
    }

    private void OnMouseAction(Define.MouseEvent evt)
    {
        if (Define.MouseEvent.PointerDown == evt && _isEditMode && _isSelectNode)
        {
            CreateNode_List(_selectedNodeIndex);
        }
    }

    private void ChangeEditMode()
    {
        _isEditMode = !_isEditMode;
        
        if (_isEditMode)
        {
            UnmergeMeshes();
            var uiBoard = Managers.UI.ShowUI<UIBoard>();
            uiBoard.SetActive(_isEditMode);
        }
        else
        {
            _isSelectNode = false;
            _selectedNodeIndex = -1;
            ClearPreviewNode();
            MergeAllMeshes();
            Managers.UI.ColseUI<UIBoard>();
        }
    }

    #region save & load
    //todo : saveload
    private void SaveBoard()
    {

    }

    private void LoadBoard()
    {
        _dirNodes.Clear();
        var nodes = _nodeGroup.GetComponentsInChildren<NodeBase>();
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

            Vector3Int coor = WorldToGrid(nodes[i].transform.position, Vector3.zero);
            if (!_dirNodes.ContainsKey(coor))
            {
                var node = nodes[i].GetComponent<NodeBase>();
                node.Init(coor);
                _dirNodes.Add(coor, node);
            }
        }
        MergeAllMeshes();

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

        AddBuildingNode(_previewNode, _nodeList[index]);
    }


    private void RemoveTile()
    {
        NodeBase node = GetNodeToMouse();
        if (node == null)
            return;

        RemoveNode(node);
    }

    #endregion

    private NodeBase GetNodeToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = (int)Define.Layer.Ground | (int)Define.Layer.Building;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, layerMask))
        {
            if(raycastHit.collider != null)
                return raycastHit.collider.GetComponent<NodeBase>();
            if (raycastHit.rigidbody != null)
                return raycastHit.rigidbody.GetComponent<NodeBase>();
            Debug.Log("collider,rigidbody 검출 실패");
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
                return (WorldToGrid(hit.point, hit.normal), Vector3.zero);
            }
            else /*if (1 << hit.transform.gameObject.layer == (int)Define.Layer.Ground)*/
            {
                return (WorldToGrid(hit.point, hit.normal), hit.normal);
            }
        }
        return (Vector3Int.zero, Vector3.zero);
    }

    private bool AddBuildingNode(NodeBase previewNode)
    {
        GameObject nodeprefab = Managers.Resource.Instantiate(_cardItemPath, this.transform);
        nodeprefab.SetActive(false);
        return AddBuildingNode(previewNode, nodeprefab, true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="previewNode"></param>
    /// <param name="nodePrefab"></param>
    private bool AddBuildingNode(NodeBase previewNode, GameObject nodePrefab, bool isCard = false)
    {
        NodeBase node = nodePrefab.GetComponent<NodeBase>();
        if (node == null)
        {
            if (isCard)
                Destroy(node.gameObject);
            Debug.Log($"{previewNode.name}이 없습니다.");
            return false;
        }

        Vector3Int gridPosition = previewNode.Position;
        if (!CanPlaceBuilding(gridPosition, node.NodeSize, previewNode))
        {
            if (isCard)
                Destroy(node.gameObject);
            Debug.Log("이미 설치된 위치입니다.");
            return false;
        }

        // 차지하는 공간을 nodes 딕셔너리에 추가
        for (int y = 0; y < node.NodeSize.y; y++)
        {
            for (int x = 0; x < node.NodeSize.x; x++)
            {
                for (int z = 0; z < node.NodeSize.z; z++)
                {
                    Vector3Int nodePosition = new Vector3Int(gridPosition.x + x,
                                                             gridPosition.y + y,
                                                             gridPosition.z + z);
                    _dirNodes.Add(nodePosition, node);
                }
            }
        }

        NodeBase nodeObject;
        // 생성
        if (isCard)
            nodeObject = node;
        else
            nodeObject = Managers.Resource.Instantiate(node.gameObject).GetComponent<NodeBase>();

        // node 생성 및 초기화
        nodeObject.Init(gridPosition);
        nodeObject.SetActive(true);
        nodeObject.transform.position = previewNode.transform.position;
        if (nodeObject is BuildingNode)
        {
            nodeObject.transform.SetParent(_buildingGroup.transform);
            _constructedBuildingList.Add(nodeObject as BuildingNode);
        }
        else
        {
            nodeObject.transform.SetParent(_nodeGroup.transform);
            nodeObject.transform.rotation = previewNode.transform.rotation;
        }
        //설치 성공시 실행
        nodeObject.InstallationSuccess();
        return true;
    }

    private static Vector3 ComputeNodeScalePosition(NodeBase node, Vector3Int gridPosition)
    {
        return gridPosition + new Vector3((node.NodeSize.x - 1) * 0.5f, 0, (node.NodeSize.z - 1) * 0.5f);
    }

    // 건물 노드 제거
    public void RemoveNode(NodeBase node)
    {
        // building 일 경우 리스트에서 삭제
        if( node is BuildingNode)
        {
            _constructedBuildingList.Remove(node as BuildingNode);
        }

        // 차지하는 공간을 nodes 딕셔너리에서 제거
        for (int y = 0; y < node.NodeSize.y; y++)
        {
            for (int x = 0; x < node.NodeSize.x; x++)
            {
                for (int z = 0; z < node.NodeSize.z; z++)
                {
                    Vector3Int nodePosition = new Vector3Int(node.Position.x + x,
                                                             node.Position.y + y,
                                                             node.Position.z + z);
                    _dirNodes.Remove(nodePosition);
                }
            }
        }
        Managers.Resource.Destroy(node.gameObject);
    }


    private bool CanPlaceBuilding(Vector3Int gridPosition, Vector3Int size, NodeBase node)
    {
        //블록노드의 경우 y가 0이 아니라면 아래 blocknode를 찾는다.
        if(node is BlockNode)
        {
            if (gridPosition.y != 0)
            {
                for (int x = 0; x < size.x; x++)
                {
                    for (int z = 0; z < size.z; z++)
                    {
                        Vector3Int nodePosition = new Vector3Int(gridPosition.x + x, gridPosition.y - 1, gridPosition.z + z);
                        if (_dirNodes.ContainsKey(nodePosition))
                        {
                            if (_dirNodes[nodePosition] is not BlockNode)
                                return false;
                        }
                        else
                            return false;
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    Vector3Int nodePosition = new Vector3Int(gridPosition.x + x, gridPosition.y - 1, gridPosition.z + z);
                    if (_dirNodes.ContainsKey(nodePosition))
                    {
                        if (_dirNodes[nodePosition] is not BlockNode)
                            return false;
                    }
                    else
                        return false;
                }
            }
        }

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    Vector3Int nodePosition = new Vector3Int(gridPosition.x + x, gridPosition.y + y, gridPosition.z + z);
                    if (_dirNodes.ContainsKey(nodePosition))
                    {
                        //Debug.Log($"field : x{x},y{y},z{z} nodePosition : {nodePosition} ");
                        return false; // 이미 노드가 있는 위치에는 건물을 지을 수 없음
                    }
                }
            }
        }
        return true; // 모든 위치가 비어있으면 건물을 지을 수 있음
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변환
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public Vector3 GridToWorld(Vector3Int gridPosition)
    {
        float x = gridPosition.x * tileSize.x;
        float y = gridPosition.y * tileSize.y;
        float z = gridPosition.z * tileSize.z;
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변환
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Vector3 SnapPosition(Vector3 point)
    {
        float snappedX = Mathf.Round(point.x / tileSize.x) * tileSize.x;
        float snappedZ = Mathf.Round(point.z / tileSize.z) * tileSize.z;
        float snappedY = Mathf.Round(point.y / tileSize.y) * tileSize.y;

        return new Vector3(snappedX, snappedY, snappedZ);
    }

    /// <summary>
    /// 반올림 
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Vector3Int WorldToGrid(Vector3 point, Vector3 normal)
    {
        if (normal == Vector3.zero || normal.x + normal.y + normal.z > 0)
            return Vector3Int.FloorToInt(point + (Vector3.one * 0.5f));

        return Vector3Int.FloorToInt(point + (Vector3.one * 0.5f)) + Vector3Int.FloorToInt(normal);
    }

    /// <summary>
    /// 인수로 받은 worldPosition에서 Navmesh로 이동 가능한 가장 가까운 Position 반환
    /// </summary>
    /// <param name="worldPosition">world position</param>
    /// <param name="movealbePosition">이동 가능한 world position 반환</param>
    /// <returns>NavMesh.SamplePosition 성공 여부</returns>
    public bool GetMoveablePosition(Vector3 worldPosition, out Vector3 movealbePosition, float maxDistance = 1f)
    {
        movealbePosition = Vector3.zero;
        if (NavMesh.SamplePosition(worldPosition, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            movealbePosition = hit.position;
            return true;
        }

        return false;
    }

    #region mesh
    /// <summary>
    /// Node 메쉬 병합
    /// </summary>
    private void MergeAllMeshes()
    {
        //_nodeGroup 하위 nodeBlock의 메쉬가져오기 
        MeshFilter[] meshFilters = _nodeGroup.GetComponentsInChildren<MeshFilter>();

        if (meshFilters.Length == 0) return;

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
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
        if (meshFilters.Length > 0)
        {
            mergedMeshRenderer.sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;
        }
        navSurface.BuildNavMesh();
    }

    /// <summary>
    /// 병합 해제
    /// </summary>
    private void UnmergeMeshes()
    {
        if (mergedMeshFilter.mesh != null)
        {
            Destroy(mergedMeshFilter.mesh);
            mergedMeshFilter.mesh = null;
            mergedMeshCollider.sharedMesh = null;
        }

        // nodeGroup 활성화
        _nodeGroup.gameObject.SetActive(true);
    }
    #endregion

    /// <summary>
    /// 보드 UI에서 생성 
    /// </summary>
    /// <param name="index"></param>
    public void SetNodeIndex(int index)
    {
        if (!_isEditMode)
            return;
        if (_selectedNodeIndex == index)
            return;

        ClearPreviewNode();
        _selectedNodeIndex = index;
        _previewNode = Managers.Resource.Instantiate(_nodeList[_selectedNodeIndex], this.transform)
                                        .GetComponent<NodeBase>();
        _previewNode.gameObject.layer = 0;
        _previewNode.gameObject.name = "PreviewNode";
        ChangeMaterialPreviewNode(false);
        _previewNode.SetActive(true);

        _isSelectNode = true;

    }

    
    /// <summary>
    /// 카드에서 생성
    /// </summary>
    /// <param name="itemBase"></param>
    public void ExcuteCardBuilding(ItemBase itemBase)
    {
        ClearPreviewNode();

        _cardItemPath = $"Building/{Managers.Data.BuildingDict[itemBase.GetTableNum].prefab}";
        _previewNode = Managers.Resource.Instantiate(_cardItemPath, this.transform)
                                        .GetComponent<NodeBase>();
        _previewNode.gameObject.layer = 0;
        _previewNode.gameObject.name = "PreviewNode";
        ChangeMaterialPreviewNode(false);
        _previewNode.SetActive(true);
        _isSelectNode = true;
        _isCard = true;
    }

    /// <summary>
    /// 카드를 놓았을때 실행
    /// </summary>
    /// <param name="itemBase"></param>
    public bool CompleteCardBuilding()
    {
        bool isBuilding = AddBuildingNode(_previewNode);
        ClearPreviewNode();
        _isSelectNode = false;
        _isCard = false;
        _cardItemPath = string.Empty;
        return isBuilding;
    }

    public void OnCancelSelectedNode()
    {
        ClearPreviewNode();
        _isSelectNode = false;
    }

    private void ChangeMaterialPreviewNode(bool isCan)
    {
        MeshRenderer[] meshRender = _previewNode.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshRender.Length; i++)
        {
            meshRender[i].material = isCan ? _previewMaterial_Green : _previewMaterial_Red;
        }
    }

    private void ClearPreviewNode()
    {
        if (_previewNode != null)
           Destroy(_previewNode.gameObject);
        _previewNode = null;
    }


}
