using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private Product product;
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
        if (tavern.GetTavernMoney() >= product.price) {
            //Проводиться оплата за покупку выбранного товара
            tavern.DecreaseTavernMoney(product.price);
            //Добавление продукта на склад
            tavern.UpdateDictionary(product.foodName, shop.foodStore[product.productIndex]);
            //Визуальное обновление окна склада в игре
            tavern.UpdateStorageInfo(product.foodName, product.productSprite);

            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
