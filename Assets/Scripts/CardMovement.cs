using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private RectTransform canvasRectTranform;
    private Vector3 originalScale;
    private int currentState = 0;
    private Quaternion originalRotation;
    private Vector3 originalPosition;
    private GridManager gridManager;
    private readonly int maxColumn = 2;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;
    [SerializeField] private float lerpFactor = 0.1f;
    [SerializeField] private int cardPlayDivider = 4;
    [SerializeField] private float cardPlayMultiplier = 1f;
    [SerializeField] private bool needUpdateCardPlayPosition = false;
    [SerializeField] private int playPositionYDivider = 2;
    [SerializeField] private float playPositionYMultiplier = 1f;
    [SerializeField] private int playPositionXDivider = 4;
    [SerializeField] private float playPositionXMultiplier = 1f;
    [SerializeField] private bool needUpdatePlayPosition = false;

    private LayerMask gridLayerMask;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvasRectTranform = canvas.GetComponent<RectTransform>();
        }

        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;

        updateCardPlayPostion();
        updatePlayPostion();
        gridManager = FindObjectOfType<GridManager>();

        gridLayerMask = LayerMask.GetMask("Grid");
    }

    void Update()
    {
        if (needUpdateCardPlayPosition)
        {
            updateCardPlayPostion();
        }

        if (needUpdatePlayPosition)
        {
            updatePlayPostion();
        }
        
        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;
            case 2:
                HandleDragState();
                if (!Input.GetMouseButton(0)) //Check if mouse button is released
                {
                    TransitionToState0();
                }
                break;
            case 3:
                HandlePlayState();
                break;
        }
    }

    private void TransitionToState0()
    {
        currentState = 0;
        GameManager.Instance.PlayingCard = false;
        rectTransform.localScale = originalScale; //Reset Scale
        rectTransform.localRotation = originalRotation; //Reset Rotation
        rectTransform.localPosition = originalPosition; //Reset Position
        glowEffect.SetActive(false); //Disable glow effect
        playArrow.SetActive(false); //Disable playArrow
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;
            originalScale = rectTransform.localScale;

            currentState = 1;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            TransitionToState0();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            currentState = 2;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == 2)
        {
            if (Input.mousePosition.y > cardPlay.y)
            {
                currentState = 3;
                playArrow.SetActive(true);
                rectTransform.localPosition = Vector3.Lerp(rectTransform.position, playPosition, lerpFactor);
            }
        }
    }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
    }

    private void HandleDragState()
    {
        //Set the card's rotation to zero
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.position = Vector3.Lerp(rectTransform.position, Input.mousePosition, lerpFactor);
    }

    private void HandlePlayState()
    {
        if (!GameManager.Instance.PlayingCard)
        {
            GameManager.Instance.PlayingCard = true;
        }

        rectTransform.localPosition = playPosition;
        rectTransform.localRotation = Quaternion.identity;

        if (!Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, gridLayerMask);

            if (hit.collider != null && hit.collider.GetComponent<GridCell>())
            {
                GridCell cell = hit.collider.GetComponent<GridCell>();
                Vector2 targetPos = cell.gridIndex;
                if (cell.gridIndex.x < maxColumn && gridManager.AddObjectToGrid(GetComponent<CardDisplay>().cardData.prefab, targetPos))
                {
                    HandManager handManager = FindAnyObjectByType<HandManager>();
                    DiscardManager discardManager = FindObjectOfType<DiscardManager>();
                    discardManager.AddToDiscard(GetComponent<CardDisplay>().cardData);
                    handManager.cardsInHand.Remove(gameObject);
                    handManager.UpdateHandVisuals();
                    Debug.Log("Placed character");
                    Destroy(gameObject);
                }
            }
            TransitionToState0();
        }

        if (Input.mousePosition.y < cardPlay.y)
        {
            currentState = 2;
            playArrow.SetActive(false);
        }
    }

    private void updateCardPlayPostion()
    {
        if (cardPlayDivider != 0 && canvasRectTranform != null)
        {
            float segment = cardPlayMultiplier / cardPlayDivider;

            cardPlay.y = canvasRectTranform.rect.height * segment;
        }
    }

    private void updatePlayPostion()
    {
        if (canvasRectTranform != null && playPositionYDivider != 0 && playPositionXDivider != 0)
        {
            float segmentX = playPositionXMultiplier / playPositionXDivider;
            float segmentY = playPositionYMultiplier / playPositionYDivider;

            playPosition.x = canvasRectTranform.rect.width * segmentX;
            playPosition.y = canvasRectTranform.rect.height * segmentY;
        }
    }
}
