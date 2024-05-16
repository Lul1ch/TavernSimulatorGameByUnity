using System.Collections.Generic;
using UnityEngine;

public class FoodHolder : MonoBehaviour
{
    [SerializeField] private List<GameObject> foodList = new List<GameObject>();
    private Dictionary<string, GameObject> foodDict = new Dictionary<string, GameObject>();

    private void Start() {
        foreach (var food in foodList) {
            foodDict[food.GetComponent<Food>().foodName] = food;
        }
        EventBus.onFoodHolderReady?.Invoke();
    }

    public GameObject GetFoodSample(string foodName) {
        return foodDict[foodName];
    }

    public void ClearStorages() {
        foodDict.Clear();
        foodList.Clear();
    }
}
