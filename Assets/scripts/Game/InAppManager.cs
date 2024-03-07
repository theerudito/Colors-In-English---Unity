using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class InAppManager : MonoBehaviour, IStoreListener
{
    public static InAppManager Instance { get; private set; }

    [Serializable]
    public class NonConsumableItem
    {
        public string id;
        public string title;
        public string desc;
        public float price;
    }

    [SerializeField] private NonConsumableItem[] nonConsumableItems;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenStore()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());


        foreach (var product in nonConsumableItems)
        {
            builder.AddProduct(product.id, ProductType.NonConsumable, new IDs
            {
                { product.id, GooglePlay.Name },
                { product.id, AppleAppStore.Name },
            });
        }

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreController.products != null;
    }

    private static IStoreController m_StoreController;

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("Store Initialized");
        m_StoreController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Store Initialization Failed" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("Store Initialization Failed" + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("Purchase Failed" + failureReason);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        string productId = purchaseEvent.purchasedProduct.definition.id;

        if (productId == nonConsumableItems[0].id)
        {
            Debug.Log("You have purchased the " + nonConsumableItems[0].title);
        }
        else
        {
            Debug.Log("Purchase Failed");
        }

        return PurchaseProcessingResult.Complete;
    }

    public void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                m_StoreController.InitiatePurchase(product);
                Debug.Log("Purchasing product asynchronously: " + product.definition.id);
                PlayerPrefs.SetString("removeAds", "true");
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public bool HasPurchasedNonConsumable(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.hasReceipt)
            {
                return true;
            }
        }
        return false;
    }
}
