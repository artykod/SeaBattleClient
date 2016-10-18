using UnityEngine;

public class UIRoot : MonoBehaviour
{
    private void Awake()
    {
        foreach (Transform ch in transform)
        {
            Destroy(ch.gameObject);
        }
    }
}
