using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler

{
    Transform originalParent;
    CanvasGroup canvasGroup;

    public float minDropDistance = 2f;
    public float maxDropDistance = 3f;

    private InventoryController inventoryController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryController = InventoryController.Instance;
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
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropslot = dropItem.GetComponentInParent<Slot>();
            }
        }

        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropslot != null)
        {
            if (dropslot.currentItem != null)
            {
                //Item droppedItem = GetComponent<Item>();
                //Item targetItem = dropslot.currentItem.GetComponent<Item>();

                //if (droppedItem.ID == targetItem.ID)
                //{
                //    targetItem.AddToStack(droppedItem.quantity);
                //    originalSlot.currentItem = null;
                //    Destroy(gameObject);
                //}
                //else
                //{
                dropslot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropslot.currentItem;
                dropslot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                //transform.SetParent(dropslot.transform);
                //dropslot.currentItem = gameObject;

                //GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                //}
            }
            else
            {
                originalSlot.currentItem = null;

                //transform.SetParent(dropslot.transform);
                //dropslot.currentItem = gameObject;

                //GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }

            transform.SetParent(dropslot.transform);
            dropslot.currentItem = gameObject;
        }
        else
        {
            if (!IsWithinInventory(eventData.position))
            {
                gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                DropItem(originalSlot);
            }
            else
            {
                transform.SetParent(originalParent);
                //GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
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
        //Item item = GetComponent<Item>();
        //int quantity = item.quantity;

        //if (quantity > 1)
        //{
        //    item.RemoveFromStack();

        //    transform.SetParent(originalParent);
        //    GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        //    quantity = 1;
        //}
        //else
        //{
        //    originalSlot.currentItem = null;
        //}

        originalSlot.currentItem = null;

        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Missing 'Player' tag");
            return;
        }
        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;

        //GameObject dropItem =
        Instantiate(gameObject, dropPosition, Quaternion.identity);
        //Item droppedItem = dropItem.GetComponent<Item>();
        //droppedItem.quantity = 1;

        //dropItem.GetComponent<BounceEffect>().StartBounce();

        //if (quantity <= 1 && originalSlot.currentItem == null)
        //{
        Destroy(gameObject);
        //}

    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if (eventData.button == PointerEventData.InputButton.Right)
    //    {
    //        SplitStack();
    //    }
    //}

    //private void SplitStack()
    //{
    //    Item item = GetComponent<Item>();
    //    if (item == null || item.quantity <= 1) return;

    //    int splitAmount = item.quantity / 2;
    //    if (splitAmount <= 0) return;

    //    item.RemoveFromStack(splitAmount);

    //    GameObject newItem = item.CloneItem(splitAmount);

    //    if (inventoryController == null || newItem == null) return;

    //    foreach (Transform slotTransform in inventoryController.InventoryPanel.transform)
    //    {
    //        Slot slot = slotTransform.GetComponent<Slot>();
    //        if (slot != null && slot.currentItem == null)
    //        {
    //            slot.currentItem = newItem;
    //            newItem.transform.SetParent(slot.transform);
    //            newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    //            return;
    //        }
    //    }
    //}
    //item.AddToStack(splitAmount);
    //Destroy(newItem);
}
