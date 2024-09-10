using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEventTest : MonoBehaviour
{
    public Define.EGoodsType goods;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Test();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //Managers.UI.ShowUIPopup<UIPopupShop>();
            ShowShop();
        }
       

    }

    public void Test()
    {
        int[] arr = { 303001004,
                    303101001,
                    303002001,
                    303002002,
                    };

        Managers.Game.Inven.AddCard(Managers.Data.ShopDict[arr[Random.Range(0, arr.Length)]]);
    }

    public void Test2()
    {
        var uimain = Managers.UI.GetUI<UIMain>() as UIMain;
        uimain.UIMoveResource.QueueAddItem(transform.position, goods, 100);
    }

    public void Test3()
    {
        var uimain = Managers.UI.GetUI<UIMain>() as UIMain;
        uimain.UIMoveResource.QueueSpendItem(transform.position, goods, 100, (isSpend) =>
        {
            if (isSpend)
                Debug.Log("isSpend true");
            else
                Debug.Log("isSpend false");
        });
    }

    public void ShowShop()
    {
        Managers.UI.ShowUIPopup<UIPopupShop>();
    }

}
