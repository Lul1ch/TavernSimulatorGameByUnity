using System;
using UnityEngine;
using System.Collections;

public class FreeFood : Event
{
    [Header("FreeFoodVariables")]
    [SerializeField] private string userGaveFoodMessage;
    [SerializeField] private string userRejectedMessage;
    [SerializeField] private Tavern tavern;
    [SerializeField] private FoodOrdering foodOrdering;
    private void Start() {
        InitializeVariables();
        InvokeAnEvent();
    }

    protected override void InvokeAnEvent() {
        void ReadyToGiveOrder() => foodOrdering.isOrderTold = true; 
        Action onComplete = null;
        onComplete += ReadyToGiveOrder;

        ChangeMessageText(welcomeMessage, onComplete);
        ChangeMessageButtonsVisibility(activateConfirmButton, activateDenyButton);
        eventCoroutine = TriggerEventConsequences();
        StartCoroutine(eventCoroutine);
    }
    protected override IEnumerator TriggerEventConsequences() {
        while (userAnswer == Answer.Empty) {
            yield return new WaitForSeconds(1f);
        }

        int rand = 0;

        if (userAnswer == Event.Answer.No) {
            ChangeMessageText(userRejectedMessage);
            rand = UnityEngine.Random.Range(-3, -1);
        } else if (userAnswer == Event.Answer.FreeDish) {
            ChangeMessageText(userGaveFoodMessage);
            rand = UnityEngine.Random.Range(1, 2);
        }
        tavern.tavernBonus += rand;
        InvokeOnUserResponse();
    }

    private void InitializeVariables() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        foodOrdering = FindObjectOfType<FoodOrdering>().GetComponent<FoodOrdering>();
    }
}
