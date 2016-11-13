using UnityEngine.UI;

public class VisibleBinding : CompareBinding
{
    private Graphic[] _allGraphics;

    private void Awake()
    {
        _allGraphics = GetComponentsInChildren<Graphic>();
    }

    protected override void ComparingHandler(bool isVisible)
    {
        for (int i = 0, l = _allGraphics.Length; i < l; i++)
        {
            var graphic = _allGraphics[i];
            if (graphic) graphic.enabled = isVisible;
        }
    }
}
