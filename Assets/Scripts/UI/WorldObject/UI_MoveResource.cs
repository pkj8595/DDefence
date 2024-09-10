using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
public class UI_MoveResource : MonoBehaviour
{
    //goods
    public UIItem prefab;
    public Queue<UIItem> _itemPool = new(); // hp바 풀링
    [SerializeField] private RectTransform[] _goodsTransform;

    //goods queue
    private Queue<Action> addItemQueue = new Queue<Action>();
    private Queue<Action> spendItemQueue = new Queue<Action>();
    private bool isProcessing = false;

    public void QueueAddItem(Vector3 worldPosition, Define.EGoodsType goodsType, int amount)
    {
        addItemQueue.Enqueue(() => AddItem(worldPosition, goodsType, amount));
        ProcessQueue().Forget();
    }

    public void QueueSpendItem(Vector3 worldPosition, Define.EGoodsType goodsType, int amount, Action<bool> action)
    {
        spendItemQueue.Enqueue(() => SpendItem(worldPosition, goodsType, amount, action));
        ProcessQueue().Forget();
    }

    private async UniTaskVoid ProcessQueue()
    {
        if (isProcessing) return; // 이미 처리 중이면 리턴
        isProcessing = true;

        await UniTask.Delay(300);

        // AddItem 큐 먼저 처리
        while (addItemQueue.Count > 0)
        {
            var addItemAction = addItemQueue.Dequeue();
            addItemAction.Invoke();
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f)); 
        }
        await UniTask.Delay(1000);
        // AddItem이 다 끝난 후 SpendItem 큐 처리
        while (spendItemQueue.Count > 0)
        {
            var spendItemAction = spendItemQueue.Dequeue();
            spendItemAction.Invoke();
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        }

        isProcessing = false;
    }

    public void SpendItem(Vector3 worldPosition, Define.EGoodsType goodsType, int amount, Action<bool> action)
    {
        Vector3 fromPosition = _goodsTransform[GetGoodsIndex(goodsType)].position;
        bool able = Managers.Game.Inven.SpendItem(goodsType.ToInt(), amount);

        MoveResourceTask(worldPosition, fromPosition, goodsType, amount, able,() =>
        {
            action?.Invoke(able);
        },true).Forget();
    }

    public void AddItem(Vector3 worldPosition, Define.EGoodsType goodsType, int amount)
    {
        Vector3 fromPosition = Camera.main.WorldToScreenPoint(worldPosition);
        Vector3 toPosition = _goodsTransform[GetGoodsIndex(goodsType)].position;

        MoveResourceTask(toPosition, fromPosition, goodsType, amount, true, () =>
        {
            Managers.Game.Inven.AddItem(goodsType.ToInt(), amount);
        }).Forget();

    }

    async UniTaskVoid MoveResourceTask(Vector3 to, Vector3 from, Define.EGoodsType goodsType, int amount, bool isSpend, Action action, bool isWorld = false)
    {
        UIItem uiItem = GetOrCreateItem();
        uiItem.Init((int)goodsType, amount, isSpend);
        uiItem.transform.position = from;

        float elapsedTime = 0f;
        float duration = 0.8f; // 이동 시간 설정

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            if (isWorld)
            {
                Vector3 toPosition = Camera.main.WorldToScreenPoint(to);
                uiItem.transform.position = Vector3.Lerp(from, toPosition, t);
            }
            else
            {
                uiItem.transform.position = Vector3.Lerp(from, to, t);
            }
            await UniTask.Yield();
        }

        DequeuePool(uiItem);
        action?.Invoke();
    }

    private UIItem GetOrCreateItem()
    {
        UIItem ret;
        if (_itemPool.Count > 0)
            ret = _itemPool.Dequeue();
        else
            ret = Instantiate(prefab, transform);

        ret.gameObject.SetActive(true);
        return ret;
    }

    private void DequeuePool(UIItem item)
    {
        if (item != null)
        {
            item.gameObject.SetActive(false);
            _itemPool.Enqueue(item);
        }
    }

    public int GetGoodsIndex(Define.EGoodsType goods)
    {
        switch (goods)
        {
            case Define.EGoodsType.gold: return 0;
            case Define.EGoodsType.manaStone: return 1;
            case Define.EGoodsType.wood: return 2;
            case Define.EGoodsType.food: return 3;
            default: { Debug.LogError($"{goods} 식별되지않는 goodsType"); return -1; }
        }
    }

}
