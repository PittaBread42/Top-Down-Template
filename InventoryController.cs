using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    public GameObject InventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    { 
        itemDictionary = FindObjectOfType<ItemDictionary>();

        //for (int i = 0; i < slotCount; i++)
        //{
        //    Slot slot = Instantiate(slotPrefab, InventoryPanel.transform).GetComponent<Slot>();
        //    if (i<itemPrefabs.Length)
        //    {
        //        GameObject item = Instantiate(itemPrefabs[i], slot.transform);
        //        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //        slot.currentItem = item;
        //    }
        //}

        public List<InventorySaveData> GetInventoryItem()
        {
            List<InventorySaveData> invData = new List<InventorySaveData>();
            foreach (Transform slotTransform in inventoryPanel.transform)
            {
                Slot slot = slotTransform.GetComponent<Item>();

                if (slot.currentItem != null)
            {
                itemDictionary item = slot.currentItem.GetComponenet<item>();
                invData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTransform.getSiblingIndex() });
            }
        }

        return invData;
    }

    public void SetInventoryItems(List<InventorySaveData>)
    {
        foreach(transform child in InventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < slotCount; i++)
        {
            instantiate(slotPrefab, InventoryPanel.transform);
        }

        foreach(InventorySaveData data in inventorySaveData)
        {
            if (data.slot < slotCount)
            {
                Slot slot = InventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }
}
