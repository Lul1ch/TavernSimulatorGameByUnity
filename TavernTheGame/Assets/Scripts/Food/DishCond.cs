using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DishCond : MonoBehaviour
{
    [SerializeField] private GameObject componentProduct;
    [SerializeField] private List<GameObject> dishComponents;

    private void Start() {
        ChangeDishPrice();
    }

    private void ChangeDishPrice() {
        Food foodScript = gameObject.GetComponent<Food>();
        int newPrice = foodScript.GetFoodQualityInt();
        foreach(var component in dishComponents) {
            newPrice += component.GetComponent<Food>().GetPrice();
        }
        foodScript.ChangePrice(newPrice);
    }

    public GameObject GetComponentObject() {
        return componentProduct;
    }

    public string GetComponentName() {
        return componentProduct.name;
    }

    public Sprite GetComponentSprite() {
        return componentProduct.GetComponent<SpriteRenderer>().sprite;
    }

    public List<GameObject> GetDishComponents() {
        return dishComponents;
    }
}
