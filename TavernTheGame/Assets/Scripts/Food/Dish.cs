using System.Collections.Generic;
using UnityEngine;

public class Dish : Food
{
    [Header("Dish Variables")]
    [SerializeField] private List<GameObject> _dishComponents;
    [SerializeField] private int _dishCookingTime;
    [SerializeField] private Sprite _dishSprite;
    private int _dishIndex;
    private Dictionary<GameObject, int> _componentsObjects = new Dictionary<GameObject,int>();

    public int dishCookingTime {
        get { return _dishCookingTime; }
        set { _dishCookingTime = value; }
    }
    public Sprite dishSprite {
        get { return _dishSprite; }
        set { _dishSprite = value; }
    }
    public int dishIndex {
        get { return _dishIndex; }
        set { _dishIndex = value; }
    }

    public List<GameObject> dishComponents {
        get { return _dishComponents; }
    }

    public Dictionary<GameObject, int> componentsObjects {
        get { return _componentsObjects; }
    }

    private void Start() {
        FillComponentsDictionary();
        ChangeDishPrice();
        //Debug.Log(foodName + " " + price);
    }

    private void FillComponentsDictionary() {
        foreach(var component in _dishComponents) {
            if (!_componentsObjects.ContainsKey(component)) {
                _componentsObjects.Add(component, 0);
            }
            _componentsObjects[component]++;
        }
    }

    private void ChangeDishPrice() {
        Food foodScript = gameObject.GetComponent<Food>();
        int newPrice = (int)foodScript.foodQuality;
        foreach(var component in _dishComponents) {
            newPrice += component.GetComponent<Food>().price;
        }
        foodScript.price = newPrice;
        foodScript.price = 10; //Костыль
    }
}
