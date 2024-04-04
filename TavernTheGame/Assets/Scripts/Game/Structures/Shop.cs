using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<GameObject> _foodStore;
    [SerializeField] private List<GameObject> alcohol;
    public List<GameObject> foodStore {
        get { return _foodStore; }
    }

    private Tavern tavern;
    [SerializeField] private Transform parent;

    [SerializeField] private GameObject shopContentElement;

    private int minPrice = -1;

    private void Start() {
        tavern = GameObject.FindGameObjectWithTag("Tavern").GetComponent<Tavern>();
        //Создаём визуальное отображение каждого товара из массива продуктов в скроллере
        if ( SceneManager.GetActiveScene().name != "Training" ) {
            InitShopShowcase();
        }
    }

    private void InitShopShowcase() {
        for (int i = 0; i < _foodStore.Count;i++) {
            CreateFoodContentObject(_foodStore[i], i);
        }
        int index = _foodStore.Count;
        for (int i = 0; i < alcohol.Count;i++) {
            CreateFoodContentObject(alcohol[i], i + index);
        }
        EventBus.onShopFilled?.Invoke();
    }

    private void CreateFoodContentObject(GameObject foodObject, int i) {
            GameObject curElement = GameObject.Instantiate(shopContentElement, shopContentElement.transform.localPosition, shopContentElement.transform.rotation);
            foodObject.GetComponent<Product>().InitProductInfo();
            foodObject.GetComponent<Product>().foodName = foodObject.name;
            GameObject curProductObject = InstantiateProductIcon(curElement, foodObject);

            Product curProduct = curProductObject.GetComponent<Product>();
            curProduct.productIndex = i;
            
            int curPrice = curProduct.price;
            curElement.transform.Find("Price").GetComponent<Text>().text = curPrice.ToString();
            curElement.transform.Find("Name").GetComponent<TMP_Text>().text = foodObject.name;
            curElement.transform.Find("Buy").GetComponent<BuyButton>().InitProductVariable(curProductObject);
            //curProduct.productSprite = curProductSprite;
            curElement.transform.SetParent(parent, false);
            
            CheckForMinPrice(curPrice);
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

    public void AddTrainProducts(Dish trainDish) {
        foreach(var component in trainDish.dishComponents) {
            _foodStore.Add(component);
        }
        InitShopShowcase();
    }

    public GameObject GetRandomAlcohol() {
        return alcohol[Random.Range(0, alcohol.Count)]; 
    }
}
