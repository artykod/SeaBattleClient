using UnityEngine;

public class CellsParamsCopy : MonoBehaviour
{
    public Sprite[] Sprites;

    private void Awake()
    {
        var allSpriteArrayBindings = GetComponentsInChildren<ImageArrayBinding>(true);
        foreach (var i in allSpriteArrayBindings)
        {
            i.ReplaceArray(Sprites);
        }
    }
}
