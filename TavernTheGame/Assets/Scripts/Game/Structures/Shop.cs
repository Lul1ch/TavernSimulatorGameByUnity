using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

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
            GameObject curElement = GameObject.Instantiate(shopContentElement, shopContentElement.transform.localPosition, shopContentElement.transform.rotation);
            _foodStore[i].GetComponent<Product>().InitProductInfo();
            _foodStore[i].GetComponent<Product>().foodName = _foodStore[i].name;
            GameObject curProductObject = InstantiateProductIcon(curElement, _foodStore[i]);

            Product curProduct = curProductObject.GetComponent<Product>();
            curProduct.productIndex = i;
            //curProductObject.GetComponent<Product>().foodName = _foodStore[i].name;
            
            PrefabUtility.SaveAsPrefabAsset(curProductObject, "Assets/Prefabs/CurrentlyUsingFood/" + _foodStore[i].name + ".prefab");
            
            int curPrice = curProduct.price;
            curElement.transform.Find("Price").GetComponent<Text>().text = curPrice.ToString();
            curElement.transform.Find("Name").GetComponent<TMP_Text>().text = _foodStore[i].name;
            curElement.transform.Find("Buy").GetComponent<BuyButton>().InitProductVariable(curProductObject);
            //curProduct.productSprite = curProductSprite;
            curElement.transform.SetParent(parent, false);
            
            CheckForMinPrice(curPrice);

        }
        EventBus.onShopFilled?.Invoke();
    }

    private GameObject InstantiateProductIcon(GameObject curContentElement, GameObject objToInstantiate) {
            Transform curIconTransform = curContentElement.transform.Find("PositionForIcon");
            GameObject iconObject = GameObject.Instantiate(objToInstantiate, curIconTransform.position, Quaternion.identity);

            iconObject.transform.SetParent(curContentElement.transform, false);
            Destroy(curIconTransform.gameObject);

            iconObject.name = "Icon";
            
            return iconObject;
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
