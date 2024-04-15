using System.Collections.Generic;
using System;
using UnityEngine;

public class NormalCharacter : Character
{
    public override void SayHello(List<string> list = null) { 
        base.SayHello(foodOrdering.variants.HelloPhrases); 
    }

    public override void MakeOrder() {
        foodOrdering.curOrder = foodOrdering.kitchen.GetDishByIndex(UnityEngine.Random.Range(0, foodOrdering.kitchen.GetKitchenDishesCount()));

        int rand = UnityEngine.Random.Range(0, foodOrdering.variants.OrderPhrases.Count);
        string messageTextStr = foodOrdering.variants.OrderPhrases[rand].Replace("^", "\"" + foodOrdering.curOrder.name + "\"");

        Action onComplete = PrepareOnCompleteAction();
        Say(messageTextStr, onComplete);
    }

    public override bool React(List<string> goodReactList = null, List<string> badReactList = null) {
        UpdateTavernBonus();
        return base.React(foodOrdering.variants.GoodReactPharases, foodOrdering.variants.BadReactPharases);
    }

    public override void AnswerIfClientWasntServiced(List<string> list = null) {
        base.AnswerIfClientWasntServiced(foodOrdering.variants.WasntServicedPhrases);
    }

    public override void Pay(float paymentModifier = 1f) {
        base.Pay();
    } 
}
