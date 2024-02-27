using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(GridCell))]
public class GridCellDisplay : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color highlightColor = Color.cyan;
    public Color posColor = Color.green;
    public Color negColor = Color.red;
    private Color originalColor;
    public GameObject[] backgrounds;
    private bool setBackground = false;
    public GridCell gridCell;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridCell = GetComponent<GridCell>();    
        originalColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (!setBackground) SetBackground();
    }

    // When the mouse enters the collider area
    private void OnMouseEnter()
    {
        if (!GameManager.Instance.PlayingCard)
        {
            spriteRenderer.color = highlightColor;
        }
        else if (gridCell.cellFull || gridCell.gridIndex.x > 1)
        {
            spriteRenderer.color = negColor;
        }
        else spriteRenderer.color = posColor;
    }

    // When the mouse exits the collider area
    private void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
    }

    private void SetBackground()
    {
        if (gridCell.gridIndex.x % 2 != 0)
        {
            backgrounds[0].SetActive(true);
        }
        if (gridCell.gridIndex.y % 2 != 0)
        {
            backgrounds[1].SetActive(true);
        }
        setBackground = true;
    }
}
