using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;

    public float minDropDistance = 2f;
    public float maxDropDistance = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Slot dropslot = eventData.pointerEnter?.GetComponent<Slot>();

        if (dropslot == null)
        {
            GameObject item = eventData.pointerEnter;
            if (item != null)
            {
                dropslot = item.GetComponentInParents<Slot>();
            }
        }

        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropslot != null)
        {
            if (dropslot.currentItem != null)
            {
                dropslot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropslot.currentItem;
                dropslot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }

            transform.SetParent(dropslot.transform);
            dropslot.currentItem = gameObject;
        }
        else
        {
            if (!IsWithinInventory(eventData.position))
            {
                DropItem(originalSlot);
            }
            else
            {
                transform.SetParent(originalParent);
            }

            transform.SetParent(originalParent);
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

    void DropItem(Slot originalSlot)
    {
        originalSlot.currentItem = null;

        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform != null)
        {
            Debug.LogError("Missing 'Player' tag");
            return;
        }

        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;

        GameObject dropItem = Instantiate(gameObject, dropPosition, Quaternion.identity);
        dropItem.GetComponent<BounceEffect>().StartBounce();
    
        Destroy(gameObject);
    }

}
