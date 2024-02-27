using UnityEngine;

public class OrderClient : MonoBehaviour
{
    [SerializeField] private FoodOrdering foodOrdering;
    [SerializeField] private Shop shop;
    [SerializeField] private GameObject trainingDish;

    private void Start() {
        InitializeVariables();
        shop.AddTrainProducts(trainingDish.GetComponent<Dish>());
        MakeOrder();
    }    

    private void MakeOrder() {
        foodOrdering.curOrder = trainingDish;
        foodOrdering.SayWhatYouWant();
    }

    private void InitializeVariables() {
        shop = FindObjectOfType<Shop>().GetComponent<Shop>();
        foodOrdering = FindObjectOfType<FoodOrdering>().GetComponent<FoodOrdering>();
    }
}
