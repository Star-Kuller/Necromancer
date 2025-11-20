using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSortOrder : MonoBehaviour
{
    private const int Offset = -100000;

    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        // чем ниже по Y, тем больше sortingOrder
        _sr.sortingOrder = Offset + Mathf.RoundToInt(-transform.position.y);
    }
}