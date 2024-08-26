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
    private List<ISelectedable> _selectedObject = new List<ISelectedable>();


    public void Init()
    {
        Managers.Input.MouseAction -= SelectMouseAction;
        Managers.Input.MouseAction += SelectMouseAction;
    }

    private void SelectMouseAction(Define.MouseEvent mouse)
    {
        if(mouse == Define.MouseEvent.Click)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider.TryGetComponent(out ISelectedable selectedable))
                {
                    selectedable.OnSelect();
                }
            }
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
