using UnityEngine;
using UnityEngine.EventSystems;

public class GameFieldInput : MonoBehaviour, IPointerClickHandler
{
    public RectTransform Cross;
    public RectTransform CrossLineV;
    public RectTransform CrossLineH;

    private int _x;
    private int _y;

    public System.Action<int, int> OnCellClick = delegate { };

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        OnCellClick(_x, _y);
    }

    private void Update()
    {
        var rt = transform as RectTransform;
        var mp = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, null, out mp);
        if (mp.x < rt.rect.xMin) mp.x = rt.rect.xMin;
        if (mp.x > rt.rect.xMax) mp.x = rt.rect.xMax;
        if (mp.y < rt.rect.yMin) mp.y = rt.rect.yMin;
        if (mp.y > rt.rect.yMax) mp.y = rt.rect.yMax;
        
        _x = Mathf.Clamp(Mathf.FloorToInt((mp.x + 32f) / 64f), -4, 5);
        _y = Mathf.Clamp(Mathf.FloorToInt((mp.y + 16f) / 64f), -5, 4);

        var crossPosition = new Vector2(_x * 64 - 16f, _y * 64);

        _x += 4;
        _y = 4 - _y;

        Cross.localPosition = crossPosition;
        var pos = CrossLineV.localPosition;
        pos.x = crossPosition.x;
        CrossLineV.localPosition = pos;
        pos = CrossLineH.localPosition;
        pos.y = crossPosition.y;
        CrossLineH.localPosition = pos;
    }
}
