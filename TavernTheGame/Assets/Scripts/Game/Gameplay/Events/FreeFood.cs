using UnityEngine;
using System.Collections;

public class FreeFood : Event
{
    [Header("FreeFoodVariables")]
    [SerializeField] private string userGaveFoodMessage;
    [SerializeField] private string userRejectedMessage;
    [SerializeField] private Tavern tavern;
    private void Start() {
        InitializeVariables();
        InvokeAnEvent();
    }
    protected override IEnumerator TriggerEventConsequences() {
        while (userAnswer == Answer.Empty) {
            yield return new WaitForSeconds(1f);
        }

        int rand = 0;

        if (userAnswer == Event.Answer.No) {
            ChangeMessageText(userRejectedMessage);
            rand = Random.Range(-3, -1);
        } else if (userAnswer == Event.Answer.FreeDish) {
            ChangeMessageText(userGaveFoodMessage);
            rand = Random.Range(1, 2);
        }
        tavern.tavernBonus += rand;
        InvokeOnUserResponse();
    }

    private void InitializeVariables() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
    }
}
