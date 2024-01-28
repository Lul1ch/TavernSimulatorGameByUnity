using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Kitchen : MonoBehaviour
{
    [SerializeField] private List<GameObject> dishes;
    [SerializeField] private GameObject kitchenContentElementSample;
    [SerializeField] private Transform parent;

    public void InitKitchenShowcase() {
        int index = 0;
        //Для каждого элемента, который лежит в массиве блюд создаём визуальное отображение в скролере с помощью ранее созданного объекта шаблона
        foreach(var elem in dishes) {
            GameObject curElement = Instantiate(kitchenContentElementSample, kitchenContentElementSample.transform.position, kitchenContentElementSample.transform.rotation);
            elem.GetComponent<Dish>().ChangeDishPrice();
            GameObject curDish = InstantiateDishIcon(curElement, elem);

            Transform elemTransform = curElement.transform;
            Sprite curDishSprite = elem.GetComponent<Image>().sprite;
            Dish elemDishScript = elem.GetComponent<Dish>();
            int curDishCookingTime = elemDishScript.dishCookingTime;

            elemTransform.Find("Time").GetComponent<TMP_Text>().text = curDishCookingTime.ToString();
            elemTransform.Find("Name").GetComponent<TMP_Text>().text = elem.name;
            elemTransform.Find("Cook").GetComponent<CookButton>().InitDishVariable(curDish);

            elemDishScript.foodName = elem.name;
            elemDishScript.dishIndex = index;
            elemDishScript.dishSprite = curDishSprite;

            elemTransform.SetParent(parent, false);
            index++;
        }
    }

    private GameObject InstantiateDishIcon(GameObject curContentElement, GameObject objToInstantiate) {
            Transform curIconTransform = curContentElement.transform.Find("PositionForIcon");
            GameObject iconObject = GameObject.Instantiate(objToInstantiate, curIconTransform.position, Quaternion.identity);

            iconObject.transform.SetParent(curContentElement.transform, false);
            Destroy(curIconTransform.gameObject);

            iconObject.name = "Icon";
            
            return iconObject;
    }

    public GameObject GetDish(int index) {
        return dishes[index]; 
    }

    public int GetKitchenDishesCount() {
        return dishes.Count;
    }

    public GameObject GetDishByIndex(int index) {
        return dishes[index];
    }

    public List<GameObject> GetDishesList() {
        return dishes;
    }
}
