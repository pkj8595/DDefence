using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Goods : MonoBehaviour
{
    public Image _imgGoods;
    
    public void Init(int goodsNum)
    {
        var goods = Managers.Data.GoodsDict[goodsNum];
        _imgGoods.sprite = Managers.Resource.Load<Sprite>($"Sprites/UI/Icon/{goods.imageStr}");
    }

    public void Explosion(Vector2 from, Vector2 to, float circleRange)
    {
        transform.position = from;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(from + Random.insideUnitCircle * circleRange, 0.25f)).SetEase(Ease.OutCubic);
        sequence.Append(transform.DOMove(to, 0.5f).SetEase(Ease.InCubic));
        sequence.AppendCallback(() => { gameObject.SetActive(false); });
    }


}
