using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectedable
{
    public void OnSelect();
    public void OnDeSelect();
    public bool IsSelected { get; set; }
}

public class SelectedManager : MonoBehaviour
{
    private List<ISelectedable> _selectedObjectList = new List<ISelectedable>();


    public void Init()
    {
        Managers.Input.MouseAction -= SelectMouseAction;
        Managers.Input.MouseAction += SelectMouseAction;
    }

    private void SelectMouseAction(Define.MouseEvent mouse)
    {
        if (mouse == Define.MouseEvent.LClick)
        {
            //if(Input.GetMouseButton)
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = (int)Define.Layer.Pawn | (int)Define.Layer.Building;
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
            {
                if (hit.collider.TryGetComponent(out ISelectedable selectedable))
                {
                    SelectObject(selectedable);
                }
            }
        }

        // 마우스 우클릭으로 유닛 명령 (이동)
        if (mouse == Define.MouseEvent.RClick && _selectedObjectList.Count > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = (int)Define.Layer.Ground;
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
            {
                foreach (var selected in _selectedObjectList)
                {
                    PawnBase pawn = selected as PawnBase;
                    if (pawn != null && pawn.Team == Define.ETeam.Playable)
                    {
                        pawn.SetDestination(hit.point);
                    }
                }
            }
            DeselectAllObject();
        }

    }

    public void SelectObject(ISelectedable monster)
    {
        if (!_selectedObjectList.Contains(monster))
        {
            _selectedObjectList.Add(monster);
            monster.OnSelect();
        }
    }

    public void DeselectAllObject()
    {
        foreach (var monster in _selectedObjectList)
        {
            monster.OnDeSelect();
        }
        _selectedObjectList.Clear();
    }

    public List<ISelectedable> GetSelectedMonsters()
    {
        return _selectedObjectList;
    }

    
}
