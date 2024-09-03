using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UI_RuneSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image _imgSlot;
    [SerializeField] private Text _txtName;
    private Data.RuneData _runeData;
    private int _index;
    private PawnBase _pawnBase;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropItem = eventData.pointerDrag;
        if (dropItem == null)
            return;

        if (dropItem.TryGetComponent(out UICard card))
        {
            if (card.Item is RuneItem)
            {
                _pawnBase.SetRuneData(card.Item.TableBase as Data.RuneData, _index);
                Managers.UI.GetUI<UIUnitPopup>().UpdateUI();
                card.UseComplete(true);
            }
            else
            {
                card.UseComplete(false);
            }
        }
    }

    public void Init(PawnBase pawn, int index)
    {
        _pawnBase = pawn;
        _index = index;
        _runeData = pawn.RuneList[index];

        if (_runeData != null)
        {
            _imgSlot.sprite = Managers.Resource.Load<Sprite>($"{Define.Path.UIIcon}{_runeData.imageStr}");
            _txtName.text = _runeData.name;
            _imgSlot.gameObject.SetActive(true);
        }
        else
        {
            _txtName.text = string.Empty;
            _imgSlot.gameObject.SetActive(false);
        }

    }

    


}
