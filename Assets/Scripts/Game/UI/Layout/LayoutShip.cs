using UnityEngine;
using UnityEngine.EventSystems;

public class LayoutShip : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Data.ShipType ShipType;

    private LayoutField _field;
    private Vector2 _initialOffset;
    private Vector2 _startPosition;
    private ShipViewContext _ship;
    private bool _isDragged;
    private int _x, _y;

    public event System.Action OnSendServerRequest = delegate { };

    public void FetchShipView(ShipViewContext ship)
    {
        _ship = ship;
        if (_ship != null) _startPosition = transform.localPosition = _ship.Position.Value;
    }

    private void Awake()
    {
        _field = GetComponentInParent<LayoutField>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        _initialOffset = new Vector2(transform.position.x, transform.position.y) - eventData.position;
        _startPosition = transform.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + _initialOffset;
        _field.AlignShipPosition(this, out _x, out _y);
        _isDragged = true;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (_field.AlignShipPosition(this, out _x, out _y))
        {
            if (_ship == null || _ship.Model == null)
            {
                OnSendServerRequest();
                Core.Instance.Match.PlaceShip(new Data.FieldShipData(Data.ShipDirection.Horizontal, ShipType, _x, _y));
            }
            else
            {
                OnSendServerRequest();
                Core.Instance.Match.ChangeShip(_ship.Model.X, _ship.Model.Y, new Data.FieldShipData(_ship.Model.Direction, ShipType, _x, _y));
            }
        }
        else
        {
            transform.position = _startPosition;
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!_isDragged && _field.AlignShipPosition(this, out _x, out _y) && _ship != null && _ship.Model != null)
        {
            OnSendServerRequest();
            var direction = _ship.Model.Direction == Data.ShipDirection.Horizontal ? Data.ShipDirection.Vertical : Data.ShipDirection.Horizontal;
            Core.Instance.Match.ChangeShip(_ship.Model.X, _ship.Model.Y, new Data.FieldShipData(direction, ShipType, _x, _y));
        }

        _isDragged = false;
    }
}
