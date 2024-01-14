using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private ProductInfo productInfo;
    [SerializeField] private Tavern tavern;
    [SerializeField] private Shop shop;

    [Header("Scene's objects")]
    [SerializeField] private Button buyButton;
    private void Start() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        shop = FindObjectOfType<Shop>().GetComponent<Shop>();
        buyButton.onClick.AddListener(() => BuySelectedProduct());
    }

    private void BuySelectedProduct() {
        if (tavern.GetTavernMoney() >= productInfo.productPrice) {
            //Проводиться оплата за покупку выбранного товара
            tavern.DecreaseTavernMoney(productInfo.productPrice);
            //Добавление продукта на склад
            tavern.UpdateDictionary(productInfo.productName, shop.foodStore[productInfo.productIndex]);
            //Визуальное обновление окна склада в игре
            tavern.UpdateStorageInfo(productInfo.productName, productInfo.productSprite);

            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
