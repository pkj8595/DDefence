using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEventTest : MonoBehaviour
{
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Test();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Managers.UI.ShowUIPopup<UIPopupShop>();
        }
    }

    public void Test()
    {
        int[] arr = { 303001004,
303003001,
303003002,
303100001,
303100002,
303100003,
303100004,
303100005,
303101001,
303002001,
303002002,
        };



        Managers.Game.Inven.AddCard(Managers.Data.ShopDict[arr[Random.Range(0, arr.Length)]]);

    }



}
