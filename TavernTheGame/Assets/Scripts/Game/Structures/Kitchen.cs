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
    [SerializeField] private Hint tavernHint;
    [SerializeField] private Tavern tavern;

    public void InitKitchenShowcase() {
        int index = 0;
        CreateFoodContentObjects(dishes, ref index);
    }

    private void CreateFoodContentObjects(List<GameObject> list, ref int index) {
        foreach(var elem in list) {
            GameObject curElement = Instantiate(kitchenContentElementSample, kitchenContentElementSample.transform.position, kitchenContentElementSample.transform.rotation);
            curElement.name = elem.name;
            elem.GetComponent<Dish>().ChangeDishPrice();
            GameObject curDish = InstantiateDishIcon(curElement, elem);

            Transform elemTransform = curElement.transform;
            Sprite curDishSprite = elem.GetComponent<Image>().sprite;
            Dish elemDishScript = elem.GetComponent<Dish>();
            int curDishCookingTime = elemDishScript.dishCookingTime;

            elemTransform.Find("Time").GetComponent<Text>().text = curDishCookingTime.ToString();
            elemTransform.Find("Name").GetComponent<TMP_Text>().text = elem.name;
            elemTransform.Find("Cook").GetComponent<CookButton>().InitDishVariable(curDish);
            if (elemDishScript.foodName == "") {
                elemDishScript.foodName = elem.name;
            }
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

    public void HalfDishesCookTime() {
        foreach (Transform child in parent) {
            Dish dish = child.Find("Icon").GetComponent<Dish>();
            dish.dishCookingTime = (int)Mathf.Round(dish.dishCookingTime / 2);
            child.Find("Time").GetComponent<Text>().text = dish.dishCookingTime.ToString();
        }
    }

    public void AutomaticCookStart(string dishName) {
        if (tavern.IsNumberGreaterThanZero(dishName)) {
            return;
        }
        string addHintStr = parent.Find(dishName).Find("Cook").GetComponent<CookButton>().CookSelectedDish(true);
        if (addHintStr != null) {
            tavernHint.ShowHint(Hint.EventType.NotEnoughProducts, addHintStr);
        } else {
            tavernHint.ShowHint(Hint.EventType.AutomaticCookingStarted);
        }
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
