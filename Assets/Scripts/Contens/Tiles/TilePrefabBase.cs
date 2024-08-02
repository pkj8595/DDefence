using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// MonoBehaviour가 들어가는 타일을 제어하는 BASE 클래스
/// </summary>
public class TilePrefabBase : MonoBehaviour, IWorldObject
{
    [SerializeField] private Vector3Int _tilePosition;
    [SerializeField] private Vector3Int _tileSize;
    [SerializeField] private float aniDuration = 0.5f;

    public Vector3Int TilePosition { get => _tilePosition; set => _tilePosition = value; }
    public Vector3Int TileSize { get => _tileSize; set => _tileSize = value; }
    private static Vector3 StartSize = new(0.1f, 0.1f, 0.1f);

    public void Init(Vector3Int tilePosition)
    {
        this._tilePosition = tilePosition;
    }

    public void DoNoting()
    {
        throw new System.NotImplementedException();
    }

    private void OnEnable()
    {
    }

    public void SetActive(bool isActive)
    {
        if (isActive)
            OnActive();
        else
            OnDeActive();
    }

    private void OnActive()
    {
        gameObject.SetActive(true);
        transform.DOScale(_tileSize, aniDuration).From(StartSize).SetEase(Ease.InOutBack);
    }

    private void OnDeActive()
    {
        transform.DOScale(StartSize, aniDuration)
                .From(_tileSize)
                .SetEase(Ease.InOutBack)
                .OnComplete(() => {
                    gameObject.SetActive(false);
                    Managers.Resource.Destroy(gameObject); 
                });
    }

}
