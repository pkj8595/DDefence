using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectedable
{
    public void OnSelect();
    public void OnDeSelect();
    public bool IsSelected();
}

public class SelectedManager : MonoBehaviour
{
    public RectTransform selectionBox;
    private Vector2 _startPosition;
    private Vector2 _endPosition;


    private List<ISelectedable> _selectedObject = new List<ISelectedable>();

    void Start()
    {
        selectionBox.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                return;

            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            //{
            //    int layer = 1 << hit.collider.gameObject.layer;
            //    if (SelectdPawnGroup == null || !SelectdPawnGroup.IsSeleced)
            //    {
            //        if (layer == (int)Define.Layer.Pawn)
            //        {
            //            //pawn BoardManager.Instance.WorldToGrid(hit.point);
            //            SelectdPawnGroup = hit.transform.parent.GetComponent<PawnGroup>();
            //            SelectdPawnGroup?.Selected();
            //        }
            //    }
            //    else
            //    {
            //        if (layer != (int)Define.Layer.Ground)
            //            return;
            //        SelectdPawnGroup.MoveGroupTo(BoardManager.Instance.WorldToGrid(hit.point));
            //    }
            //}
        }
    }

    public void SelectObject(ISelectedable monster)
    {
        if (!_selectedObject.Contains(monster))
        {
            _selectedObject.Add(monster);
            monster.OnSelect();
        }
    }

    public void DeselectAllObject()
    {
        foreach (var monster in _selectedObject)
        {
            monster.OnDeSelect();
        }
        _selectedObject.Clear();
    }

    public List<ISelectedable> GetSelectedMonsters()
    {
        return _selectedObject;
    }

    
}
