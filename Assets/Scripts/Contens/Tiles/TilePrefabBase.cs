using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// MonoBehaviour가 들어가는 타일을 제어하는 BASE 클래스
/// </summary>
public class TilePrefabBase : MonoBehaviour, IWorldObject
{
    public Vector2Int _tileSize;
    public Vector2Int _tilePosition;

    void Start()
    {

    }
    private void OnMouseDown()
    {
        
    }

    //settting building table 
    public virtual void Init()
    {

    }

    public virtual void Clear() 
    {
    
    }

    public void DoNoting()
    {
        throw new System.NotImplementedException();
    }
}
