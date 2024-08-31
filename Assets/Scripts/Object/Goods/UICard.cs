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
    private float originalScale;
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
    }

    // 마우스가 카드에서 벗어날 때 호출되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        _rect.DOAnchorPosY(originalPosition.y, 0.2f).SetEase(Ease.OutQuad);
        _rect.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuad);
    }

    // 드래그 시작 시 호출되는 함수
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 중에는 카드가 투명해지고 레이캐스트가 무시되도록 설정
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

    // 드래그 중 호출되는 함수
    public void OnDrag(PointerEventData eventData)
    {
        
    }

    // 드래그 종료 시 호출되는 함수
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 종료 후 카드의 투명도를 원래대로 돌리고 레이캐스트 활성화
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        switch (_item)
        {
            case CharacterItem:
                break;
            case BuildingItem:
                if (BoardManager.Instance.CompleteCardBuilding())
                {
                    _OnUseAction?.Invoke(this);
                }
                break;
            case RuneItem:
                break;
        }
    }

    // 카드의 현재 World Position을 가져오는 함수
    private Vector3 GetWorldPosition()
    {
        // RectTransform의 위치를 World Position으로 변환
        return Camera.main.ScreenToWorldPoint(_rect.position);
    }

    private void ExcuteBuildingItems()
    {
        
    }
}
