using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnfairCharacter : Character
{
    [Header("UnfairGuests's variables")]
    [SerializeField] private int chanceNotToPay = 55;
    [SerializeField] private int chanceToPayHalfOfPrice = 90;
    public override void SayHello(List<string> list = null) { base.SayHello(foodOrdering.variants.HelloPhrasesUnfair); }

    public override void MakeOrder() {
        foodOrdering.curOrder = shop.GetRandomAlcohol();

        int rand = UnityEngine.Random.Range(0, foodOrdering.variants.OrderPhrasesUnfair.Count);
        string messageTextStr = foodOrdering.variants.OrderPhrasesUnfair[rand].Replace("^", "\"" + foodOrdering.curOrder.name + "\"");

        Action onComplete = PrepareOnCompleteAction();
        Say(messageTextStr, onComplete);
    }

    public override void React() {
        base.React(foodOrdering.variants.GoodReactPharasesUnfair, foodOrdering.variants.BadReactPharasesUnfair);
    }

    public override void AnswerIfClientWasntServiced(List<string> list = null) {
        base.AnswerIfClientWasntServiced(foodOrdering.variants.WasntServicedPhrasesUnfair);
    }

    public override void Pay() {
        int rand = UnityEngine.Random.Range(0, 100);
        int paymentModifier = 1f;
        if (rand < chanceNotToPay) {
            foodOrdering.tavernHint.ShowHint(Hint.EventType.isUnfairGuestGone);
            EventBus.onGuestLeft?.Invoke();
            return;
        } else if ( rand < chanceToPayHalfOfPrice) {
            paymentModifier = 0.5f;
        }
        base.Pay(paymentModifier);
    } 
}
