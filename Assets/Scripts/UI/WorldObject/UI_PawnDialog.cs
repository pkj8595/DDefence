using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class UI_PawnDialog : MonoBehaviour
{
    public Text _txtDialog;
    public bool IsProcessing {get;set;}
    public void ShowDialog(string str)
    {
        ShowTask(str).Forget();
    }

    async UniTaskVoid ShowTask(string str)
    {
        IsProcessing = true;
        _txtDialog.gameObject.SetActive(true);

        int index = 0;
        while (index < str.Length)
        {
            index++;
            _txtDialog.text = str.Substring(0, index);
            await UniTask.NextFrame();
        }

        await UniTask.Delay(2000);

        _txtDialog.text = string.Empty;
        _txtDialog.gameObject.SetActive(false);
        IsProcessing = false;
    }
}
