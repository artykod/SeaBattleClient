using UnityEngine;

public class LayoutField : MonoBehaviour
{
    private const int CELL_SIZE = 64;

    public bool AlignShipPosition(LayoutShip ship, out int x, out int y)
    {
        var rt = transform as RectTransform;
        var mp = Vector2.zero;

        x = y = -1;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, ship.transform.position, null, out mp);
        if (mp.x < rt.rect.xMin) return false;
        if (mp.x > rt.rect.xMax) return false;
        if (mp.y < rt.rect.yMin) return false;
        if (mp.y > rt.rect.yMax) return false;

        x = Mathf.Clamp(Mathf.FloorToInt((mp.x + 48f) / 64f), -4, 5);
        y = Mathf.Clamp(Mathf.FloorToInt((mp.y + 32f) / 64f), -5, 4);
        ship.transform.localPosition = new Vector2(x * 64 - 6f, y * 64 + 10f);

        x += 4;
        y = 4 - y;

        return true;
    }
}
