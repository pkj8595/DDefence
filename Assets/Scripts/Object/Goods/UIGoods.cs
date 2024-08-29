using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIGoods : MonoBehaviour
{
    public Image _imgGoods;
    public Define.EGoodsType goodsType;
    public Text _txtAmount;

    private void Start()
    {
        Init((int)goodsType);
    }

    public void Init(int goodsNum)
    {
        var goods = Managers.Data.GoodsDict[goodsNum];
        _imgGoods.sprite = Managers.Resource.Load<Sprite>($"Sprites/UI/Icon/{goods.imageStr}");
        _txtAmount.text = Managers.Game.Inven.GetItem(goodsNum).ToString();
    }

    public void UpdateAmount()
    {
        _txtAmount.text = Managers.Game.Inven.GetItem(goodsType.ToInt()).ToString();
    }

}
