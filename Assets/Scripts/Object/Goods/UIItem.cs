using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    public Image _imgItem;
    public Text _txtAmount;

    public void Init(int itemNum, int amount)
    {
        var item = ItemBase.GetItem(itemNum);

        _imgItem.sprite = Managers.Resource.Load<Sprite>($"Sprites/UI/Icon/{item.ImgStr}");
        _txtAmount.text = Managers.Game.Inven.GetItem(amount).ToString();
    }

}
