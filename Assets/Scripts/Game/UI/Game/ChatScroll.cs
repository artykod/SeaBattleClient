using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ChatScroll : MonoBehaviour
{
    private ScrollRect _scroll;

    private void Awake()
    {
        _scroll = GetComponent<ScrollRect>();
    }

    public void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        _scroll.verticalNormalizedPosition = 0f;
    }

    public void ScrollUp()
    {
        _scroll.verticalNormalizedPosition += 0.1f;
    }

    public void ScrollDown()
    {
        _scroll.verticalNormalizedPosition -= 0.1f;
    }
}
