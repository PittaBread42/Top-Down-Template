using UnityEngine;
using System.Collections.Generic;

public class ShopNPC : MonoBehaviour, IInteractable
{

    public string shopID = "shop_merchant_01";
    public string shopkeeperName = "Merchant";

    public List<ShopStockItem> defaultShopStock = new();
    private List<ShopStockItem> currentShopStock = new();

    private bool isInitialized = false;


    [System.Serializable]
    public class ShopStockItem
    {
        public int itemID;
        public int quantity;
    }

    void Start()
    {

    }

    private void InitializeShop()
    {
        if (isInitialized) return;

        currentShopStock = new List<ShopStockItem>();
        foreach (var item in defaultShopStock)
        {
            currentShopStock.Add(new ShopStockItem
            {
                itemID = item.itemID,
                quantity = item.quantity
            });
        }
        isInitialized = true; 
    }

    public bool CanInteract()
    {
        return true;
    }

    public bool Interact()
    {
        if (ShopController.Instance == null)
        {
            
        }
    }
}
