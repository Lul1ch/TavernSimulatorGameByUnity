using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bard : Event
{
    [Header("BardVariables")]
    [SerializeField] private Character characterScript;
    [SerializeField] private List<string> supportingMessages;
    [SerializeField] private List<string> resultMessages;
    [SerializeField] private string onDenyResponseMessage;
    [SerializeField] private Tavern tavern;
    [SerializeField] private int maxBonusModifier = 5;
    static private int _curBonusModifier = -1;
    private static int _indexOfCurSupportingMessage = 0;
    private static int _indexOfCurResultMessage = 0;

    private int indexOfCurSupportingMessage {
        get { return _indexOfCurSupportingMessage; }
        set { if (value <= supportingMessages.Count - 1 && value >= 0) { _indexOfCurSupportingMessage = value; } }
    }
    private int indexOfCurResultMessage {
        get { return _indexOfCurResultMessage; }
        set { if (value <= resultMessages.Count - 1 && value >= 0) { _indexOfCurResultMessage = value; } }
    }
    private int curBonusModifier {
        get { return _curBonusModifier; }
        set { if (value <= maxBonusModifier) { _curBonusModifier = value; } }
    }

    private void Start() {
        FindObjectOfType<QueueCreating>().GetComponent<QueueCreating>().isItAFoodRequiredEventVar = doesEventRequireAFoodIssuing;
        InitializeVariables();
        characterScript.SayHello();
        characterScript.MakeOrder();
    }

    public override void InvokeOnUserGaveFood() {
        characterScript.React();
        if (IsGuestReactionPossitive()) {
            InvokeAnEvent(false);
        } else {
            characterScript.Pay();
            InvokeOnUserResponse();
        }
    }

    protected override IEnumerator TriggerEventConsequences() {
        void ChangeButtonsVisibility() => ChangeMessageButtonsVisibility(activateConfirmButton, activateDenyButton);
        Action onComplete = null;
        onComplete += ChangeButtonsVisibility;
        ChangeMessageText(supportingMessages[indexOfCurSupportingMessage++], onComplete, true);

        while (userAnswer == Answer.Empty) {
            yield return new WaitForSeconds(1f);
        }

        if (userAnswer == Answer.Yes) {
            tavern.tavernBonus += curBonusModifier;
            ChangeMessageText(resultMessages[indexOfCurResultMessage++], false);
            curBonusModifier++;
        } else {
            indexOfCurSupportingMessage--;
            ChangeMessageText(onDenyResponseMessage);
            characterScript.Pay();
        }
        InvokeOnUserResponse();
    }

    private bool IsGuestReactionPossitive() {
        return characterScript.characterReaction == Character.Mood.Happy;
    }

    private void InitializeVariables() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
    } 
}
