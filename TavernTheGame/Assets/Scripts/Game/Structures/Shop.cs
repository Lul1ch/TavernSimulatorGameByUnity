using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public List<GameObject> FoodStore;

    private Tavern tavern;
    public Transform parent;

    public GameObject shopContentElement;

    private void Start() {
        tavern = GameObject.FindGameObjectWithTag("Tavern").GetComponent<Tavern>();
        //Создаём визуальное отображение каждого товара из массива продуктов в скроллере
        initShopShowcase();
    }

    private void initShopShowcase() {
        for (int i = 0; i < FoodStore.Count;i++) {
            GameObject curElement = GameObject.Instantiate(shopContentElement, shopContentElement.transform.position, shopContentElement.transform.rotation);
            
            FoodStore[i].GetComponent<Food>().InitProductInfo();
            Sprite curProductSprite = FoodStore[i].GetComponent<SpriteRenderer>().sprite;
            int curPrice = FoodStore[i].GetComponent<Food>().price;
            curElement.transform.Find("Price").GetComponent<Text>().text = curPrice.ToString();
            curElement.transform.Find("Icon").GetComponent<Image>().sprite = curProductSprite;
            curElement.transform.Find("Name").GetComponent<TMP_Text>().text = FoodStore[i].name;

            //Сохраняем необходимую информацию для работы функции покупки и доставки купленных товаров на склад
            ProductInfo curProduct = curElement.transform.Find("Buy").GetComponent<ProductInfo>();
            curProduct.productPrice = curPrice;
            curProduct.productIndex = i;
            curProduct.productName = FoodStore[i].name;
            curProduct.productSprite = curProductSprite;
            curElement.transform.SetParent(parent, false);
            
        }
    }
}
