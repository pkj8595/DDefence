using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SpriteEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites; // 애니메이션에 사용할 스프라이트 배열
    [SerializeField] private int _frameRate = 100; // 프레임 간 전환 속도

    private System.Threading.CancellationTokenSource _taskSource;

    public void PlayEffect(Vector3 position, string effectStr, int frameRate = 100)
    {
        transform.position = position;
        gameObject.SetActive(true);
        _frameRate = frameRate;
        StartTask();

    }

    private void StartTask()
    {
        CancelTask();
        _taskSource = new();
        CheckPawnPosition().Forget();
    }

    private void CancelTask()
    {
        if (_taskSource != null)
        {
            _taskSource.Cancel();
            _taskSource.Dispose();
            _taskSource = null;
        }
    }

    async UniTaskVoid CheckPawnPosition()
    {
        await UniTask.NextFrame();

        for (int i = 0; i < sprites.Length; i++)
        {
            spriteRenderer.sprite = sprites[i];
            await UniTask.Delay(_frameRate, cancellationToken: _taskSource.Token);
        }

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelTask();
    }

}
