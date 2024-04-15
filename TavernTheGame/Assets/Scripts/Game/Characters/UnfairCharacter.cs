using System;
using System.Collections.Generic;
using UnityEngine;

public class UnfairCharacter : Character
{
    [Header("UnfairGuests's variables")]
    [SerializeField] private int chanceNotToPay = 55;
    [SerializeField] private int chanceToPayHalfOfPrice = 90;
    public override void SayHello(List<string> list = null) { base.SayHello(foodOrdering.variants.HelloPhrasesUnfair); }

    public override void MakeOrder() {
        foodOrdering.curOrder = foodOrdering.shop.GetRandomAlcohol();

        int rand = UnityEngine.Random.Range(0, foodOrdering.variants.OrderPhrasesUnfair.Count);
        string messageTextStr = foodOrdering.variants.OrderPhrasesUnfair[rand].Replace("^", "\"" + foodOrdering.curOrder.name + "\"");

        Action onComplete = PrepareOnCompleteAction();
        Say(messageTextStr, onComplete);
    }

    public override bool React(List<string> goodReactList = null, List<string> badReactList = null) {
        UpdateTavernBonus();
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < chanceNotToPay) {
            foodOrdering.tavernHint.ShowHint(Hint.EventType.isUnfairGuestGone);
            EventBus.onGuestLeft?.Invoke();
            return false;
        }
        base.React(foodOrdering.variants.GoodReactPharasesUnfair, foodOrdering.variants.BadReactPharasesUnfair);
        return true;
    }

    public override void AnswerIfClientWasntServiced(List<string> list = null) {
        base.AnswerIfClientWasntServiced(foodOrdering.variants.WasntServicedPhrasesUnfair);
    }

    public override void Pay(float paymentModifier = 1f) {
        int rand = UnityEngine.Random.Range(0, 100);
        if ( rand < chanceToPayHalfOfPrice) {
            paymentModifier = 0.5f;
        }
        base.Pay(paymentModifier);
    } 
}
