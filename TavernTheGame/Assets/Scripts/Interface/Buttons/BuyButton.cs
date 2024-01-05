using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    [SerializeField] private Button buyButton;

    [SerializeField] private ProductInfo productInfo;
    [SerializeField] private Tavern tavern;
    [SerializeField] private Shop shop;
    private void Start() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        shop = FindObjectOfType<Shop>().GetComponent<Shop>();
        buyButton.onClick.AddListener(() => BuySelectedProduct());
    }

    private void BuySelectedProduct() {
        if (tavern.GetTavernMoney() >= productInfo.productPrice) {
            Debug.Log(tavern);
            //Проводиться оплата за покупку выбранного товара
            tavern.DecreaseTavernMoney(productInfo.productPrice);
            //Добавление продукта на склад
            tavern.UpdateDict(productInfo.productName, shop.FoodStore[productInfo.productIndex]);
            //Визуальное обновление окна склада в игре
            tavern.UpdateStorageInfo(productInfo.productName, productInfo.productSprite);

            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
