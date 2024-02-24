using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GridCellHighlighter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color highlightColor = Color.yellow;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    // When the mouse enters the collider area
    private void OnMouseEnter()
    {
        spriteRenderer.color = highlightColor;
    }

    // When the mouse exits the collider area
    private void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
    }
}
