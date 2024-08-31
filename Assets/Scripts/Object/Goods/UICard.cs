using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

public class UICard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Text _txtTitle;
    public Text _txtDesc;
    public Image _imgMain;
    public Image _imgBack;

    [SerializeField] private RectTransform _rect;
    private Vector3 originalPosition;
    private Vector3 startDragPosition;
    private float originalScale;
    private bool isDragging = false;

    public RectTransform Rect => _rect;
    public CanvasGroup canvasGroup;

    private Action<UICard> _OnUseAction;
    private ItemBase _item;

    private void Start()
    {
        originalPosition = _rect.anchoredPosition;
        originalScale = _rect.localScale.x;
    }

    public void Init(Data.ShopData data, Action<UICard> onUseAction)
    {
        _rect.localScale = Vector2.one;
        _txtTitle.text = data.name;
        _txtDesc.text = data.desc;
        _OnUseAction = onUseAction;

        _item = ItemBase.GetItem(data.minTableRange);
        if (data.minTableRange == data.maxTableRange)
        {
            _imgMain.sprite = Managers.Resource.Load<Sprite>(Define.Path.UIIcon + _item.ImgStr);
        }
        else
        {
            switch (_item) 
            {
                case CharacterItem:
                    _imgMain.sprite = Managers.Resource.Load<Sprite>(Define.Path.UIIcon + "IconRandomChar");
                    break;
                case BuildingItem:
                    _imgMain.sprite = Managers.Resource.Load<Sprite>(Define.Path.UIIcon + "IconRandomBuilding");
                    break;
                case RuneItem:
                    _imgMain.sprite = Managers.Resource.Load<Sprite>(Define.Path.UIIcon + "IconRandomRune");
                    break;
            }

        }
    }

    public void Clear()
    {
        _txtTitle.text = string.Empty;
        _txtDesc.text = string.Empty;
    }

    public void OnClickCard()
    {
        //_OnUseAction?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _rect.DOAnchorPosY(originalPosition.y + 30f, 0.2f).SetEase(Ease.OutQuad);
        _rect.DOScale(originalScale + 0.1f, 0.2f).SetEase(Ease.OutQuad);

        if (isDragging && Input.GetMouseButton(0))
        {
            CancelDrag();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rect.DOAnchorPosY(originalPosition.y, 0.2f).SetEase(Ease.OutQuad);
        _rect.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuad);

        if (!isDragging && Input.GetMouseButton(0))
        {
            StartDrag();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            EndDrag();
        }
    }

   
    public void UseComplete(bool isSuccess)
    {
        if(isSuccess)
            _OnUseAction?.Invoke(this);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    private void StartDrag()
    {
        isDragging = true;
        startDragPosition = _rect.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        switch (_item)
        {
            case CharacterItem:
                break;
            case BuildingItem:
                BoardManager.Instance.ExcuteCardBuilding(_item);
                break;
            case RuneItem:
                break;
        }
    }

    private void EndDrag()
    {
        isDragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        switch (_item)
        {
            case CharacterItem:
                {
                    canvasGroup.alpha = 1f;
                    canvasGroup.blocksRaycasts = true;
                }
                break;
            case BuildingItem:
                if (!BoardManager.Instance.RotationStep(this))
                {
                    canvasGroup.alpha = 1f;
                    canvasGroup.blocksRaycasts = true;
                }
                break;
            case RuneItem:
                {
                    canvasGroup.alpha = 1f;
                    canvasGroup.blocksRaycasts = true;
                }
                break;
        }
    }

    private void CancelDrag()
    {
        isDragging = false;
        _rect.anchoredPosition = startDragPosition;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;


        switch (_item)
        {
            case CharacterItem:
                {
                   
                }
                break;
            case BuildingItem:
                {
                    BoardManager.Instance.OnCancelCard();
                }
                break;
            case RuneItem:
                {
                   
                }
                break;
        }
    }
}
