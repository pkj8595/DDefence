using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    public Text _txtTitle;
    public Text _txtDesc;
    public Image _imgMain;


    public void Init(Data.ShopData data)
    {
        _txtTitle.text = data.name;
        _txtDesc.text = data.desc;

        var item = ItemBase.GetItem(data.minTableRange);
        if (data.minTableRange == data.maxTableRange)
        {
            _imgMain.sprite = Managers.Resource.Load<Sprite>(Define.Path.UIIcon + item.ImgStr);
        }
        else
        {
            switch (item) 
            {
                case CharacterItem:
                    _imgMain.sprite = Managers.Resource.Load<Sprite>(Define.Path.UIIcon + "RandomCharIcon");
                    break;
                case BuildingItem:
                    _imgMain.sprite = Managers.Resource.Load<Sprite>(Define.Path.UIIcon + "RandomBuildingIcon");
                    break;
                case RuneItem:
                    _imgMain.sprite = Managers.Resource.Load<Sprite>(Define.Path.UIIcon + "RandomRuneIcon");
                    break;
            }

        }


    }


}
