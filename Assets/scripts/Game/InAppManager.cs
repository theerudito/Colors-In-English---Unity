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
    private string purchaseMessage = "";

    [SerializeField] private NonConsumableItem[] nonConsumableItems;
    public static event Action<string> OnPurchaseSuccess;
    public static event Action<string> OnPurchaseError;
    private static IStoreController m_StoreController;
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

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        SetPurchaseMessage("Store Initialization Failed" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        SetPurchaseMessage("Store Initialization Failed" + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        SetPurchaseMessage("Purchase Failed" + failureReason);
        OnPurchaseError?.Invoke(product.definition.id);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        string productId = purchaseEvent.purchasedProduct != null
        ? purchaseEvent.purchasedProduct.definition.id
        : "unknown_product_id";

        if (purchaseEvent.purchasedProduct == null)
        {
            SetPurchaseMessage("Purchase Cancelled IAP");
            OnPurchaseError?.Invoke(productId);
        }
        else if (productId == nonConsumableItems[0].id)
        {
            SetPurchaseMessage("Purchase Successful IAP");
            OnPurchaseSuccess?.Invoke(productId);
        }
        else
        {
            SetPurchaseMessage("Purchase Failed IAP");
            OnPurchaseError?.Invoke(productId);
        }

        return PurchaseProcessingResult.Complete;
    }

    public void BuyNonConsumable(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log($"Attempting to purchase: {product.definition.id}");
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log($"Product not available for purchase: {productId}");
            }
        }
        else
        {
            Debug.Log("Purchase initialization not complete.");
        }
    }

    public bool HasPurchasedNonConsumable(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.hasReceipt)
            {
                Debug.Log($"Has purchased non consumable: {product.definition.id}");
                return true;
            }
            else
            {
                Debug.Log($"Has not purchased non consumable: {product.definition.id}");
                return false;
            }
        }
        return false;
    }

    public string GetPurchaseMessage() => purchaseMessage;

    public void SetPurchaseMessage(string message) => purchaseMessage = message;

}
