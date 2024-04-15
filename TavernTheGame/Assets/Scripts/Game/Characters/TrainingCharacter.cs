using System;
using System.Collections.Generic;
using UnityEngine;

public class TrainingCharacter : Character
{
    [SerializeField] private GameObject trainingDish;
    private void Start() {
        foodOrdering = FindObjectOfType<FoodOrdering>().GetComponent<FoodOrdering>();
        foodOrdering.shop.AddTrainProducts(trainingDish.GetComponent<Dish>());
        SayHello();
    }
    public override void SayHello(List<string> list = null) {
        foodOrdering.trainingManager.ShowOrHideButtons(false);
        base.SayHello(foodOrdering.variants.HelloPhrases);
        MakeOrder();
    }

    public override void MakeOrder() {
        foodOrdering.curOrder = trainingDish;

        int rand = UnityEngine.Random.Range(0, foodOrdering.variants.OrderPhrases.Count);
        string messageTextStr = foodOrdering.variants.OrderPhrases[rand].Replace("^", "\"" + foodOrdering.curOrder.name + "\"");

        Action onComplete = PrepareOnCompleteAction();
        void ShowButtons() => EventBus.onTrainGuestToldHisOrder?.Invoke();
        onComplete += ShowButtons;
        Say(messageTextStr, onComplete);
        foodOrdering.trainingManager.SaveMessage(foodOrdering.trainingManager.indexToSaveClientOrder, messageTextStr);
    }

    public override bool React(List<string> goodReactList = null, List<string> badReactList = null) {
        UpdateTavernBonus();
        return true; //base.React(foodOrdering.variants.GoodReactPharases, foodOrdering.variants.BadReactPharases);
    }

    public override void Pay(float paymentModifier = 1f) {
        base.Pay();
    } 
}
