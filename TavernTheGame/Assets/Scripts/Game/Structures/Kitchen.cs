using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Kitchen : MonoBehaviour
{
    [SerializeField] private List<GameObject> Dishes;
    [SerializeField] private GameObject kitchenContentElementSample;
    [SerializeField] private Transform parent;

    private void Start() {
        InitKitchenShowcase();
    }

    private void InitKitchenShowcase() {
        int index = 0;
        //Для каждого элемента, который лежит в массиве блюд создаём визуальное отображение в скролере с помощью ранее созданного объекта шаблона
        foreach(var elem in Dishes) {
            GameObject curElement = Instantiate(kitchenContentElementSample, kitchenContentElementSample.transform.position, kitchenContentElementSample.transform.rotation);

            Transform elemTransform = curElement.transform;
            Sprite curDishSprite = elem.GetComponent<SpriteRenderer>().sprite;
            int curDishCookingTime = elem.GetComponent<Food>().cookingTime;
            DishCond elemDishCond = elem.GetComponent<DishCond>();

            elemTransform.Find("Icon").GetComponent<Image>().sprite = curDishSprite;
            elemTransform.Find("Time").GetComponent<TMP_Text>().text = curDishCookingTime.ToString();
            elemTransform.Find("Name").GetComponent<TMP_Text>().text = elem.name;

            //Дополнительно сохраняем некоторые значения, для корректной работы функции готовки
            DishInfo curDishInfo = elemTransform.Find("Cook").GetComponent<DishInfo>();
            curDishInfo.productCookingTime = curDishCookingTime;
            curDishInfo.productName = elem.name;
            curDishInfo.productIndex = index;
            curDishInfo.productSprite = curDishSprite;

            curDishInfo.componentProductName = elemDishCond.GetComponentName();
                        
            List<GameObject> components = elemDishCond.GetDishComponents();
            foreach(var component in components) {
                if (!curDishInfo.componentsNames.ContainsKey(component.name)) {
                    curDishInfo.componentsNames.Add(component.name, 0);
                }
                curDishInfo.componentsNames[component.name]++;
            }

            elemTransform.SetParent(parent, false);
            index++;
        }
    }

    public GameObject GetDish(int index) {
        return Dishes[index]; 
    }

    public int GetKitchenDishesCount() {
        return Dishes.Count;
    }

    public GameObject GetDishByIndex(int index) {
        return Dishes[index];
    }

    public List<GameObject> GetDishesList() {
        return Dishes;
    }
}
