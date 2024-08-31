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
    }

    public void Test()
    {
        int[] arr = { 303001001,
                        303001002,
                        303001003,
                        303001004,
                        303002001,
                        303002002
        };

        Managers.Game.Inven.AddCard(Managers.Data.ShopDict[arr[Random.Range(0, arr.Length - 1)]]);

    }



}
