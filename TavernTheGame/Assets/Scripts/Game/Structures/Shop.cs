using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<GameObject> _foodStore;

    public List<GameObject> foodStore {
        get { return _foodStore; }
    }

    private Tavern tavern;
    [SerializeField] private  Transform parent;

    [SerializeField] private  GameObject shopContentElement;

    private int minPrice = -1;

    private void Start() {
        tavern = GameObject.FindGameObjectWithTag("Tavern").GetComponent<Tavern>();
        //Создаём визуальное отображение каждого товара из массива продуктов в скроллере
        InitShopShowcase();
    }

    private void InitShopShowcase() {
        for (int i = 0; i < _foodStore.Count;i++) {
            GameObject curElement = GameObject.Instantiate(shopContentElement, shopContentElement.transform.position, shopContentElement.transform.rotation);
            
            _foodStore[i].GetComponent<Food>().InitProductInfo();
            Sprite curProductSprite = _foodStore[i].GetComponent<SpriteRenderer>().sprite;
            int curPrice = _foodStore[i].GetComponent<Food>().price;
            curElement.transform.Find("Price").GetComponent<Text>().text = curPrice.ToString();
            curElement.transform.Find("Icon").GetComponent<Image>().sprite = curProductSprite;
            curElement.transform.Find("Name").GetComponent<TMP_Text>().text = _foodStore[i].name;

            //Сохраняем необходимую информацию для работы функции покупки и доставки купленных товаров на склад
            ProductInfo curProduct = curElement.transform.Find("Buy").GetComponent<ProductInfo>();
            curProduct.productPrice = curPrice;
            curProduct.productIndex = i;
            curProduct.productName = _foodStore[i].name;
            curProduct.productSprite = curProductSprite;
            curElement.transform.SetParent(parent, false);
            
            CheckForMinPrice(curPrice);
        }
    }

    private void CheckForMinPrice(int price) {
        if (price < minPrice || minPrice == -1) {
            minPrice = price;
        }
    }

    public int GetMinPrice() {
        return minPrice;
    }
}
