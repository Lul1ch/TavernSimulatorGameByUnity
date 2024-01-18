using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DishCond : MonoBehaviour
{
    [SerializeField] private List<GameObject> dishComponents;

    private void Start() {
        ChangeDishPrice();
    }

    private void ChangeDishPrice() {
        Food foodScript = gameObject.GetComponent<Food>();
        int newPrice = (int)foodScript.foodQuality;
        foreach(var component in dishComponents) {
            newPrice += component.GetComponent<Food>().price;
        }
        foodScript.price = newPrice;
    }

    public List<GameObject> GetDishComponents() {
        return dishComponents;
    }
}
