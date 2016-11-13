using UnityEngine;
using System.Collections;

public abstract class DialogBase : BindModel
{
    public event System.Action<DialogBase> OnClose = delegate { };

    private float _animationSpeed = 10f;
    private CanvasGroup _canvasGroup;

    public DialogBase(string prefab) : base("UI/Dialogs/" + prefab)
    {
        _canvasGroup = Instance.AddComponent<CanvasGroup>();
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        _canvasGroup.alpha = 0f;
        var t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * _animationSpeed;
            _canvasGroup.alpha = 1f - t;
            yield return null;
        }
        _canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        _canvasGroup.alpha = 1f;
        var t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * _animationSpeed;
            _canvasGroup.alpha = t;
            yield return null;
        }
        _canvasGroup.alpha = 0f;

        Root.Destroy();
        OnClose(this);
    }

    [BindCommand]
    protected void Exit()
    {
        SoundController.Sound(SoundController.SOUND_CLOSE);
        StartCoroutine(FadeOut());
    }
}
