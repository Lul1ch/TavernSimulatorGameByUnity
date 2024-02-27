using UnityEngine;
using System.Collections;

public class Whisper : Event
{
    [SerializeField] private int moneyBonus = 100;
    [Header("WhisperVariables")]
    [SerializeField] private string guestGaveMoney;
    [SerializeField] private string guestLeft;
    [SerializeField] private Tavern tavern;
    private int whisperCounter = 0;
    private int whisperMultiplier = 10;
    private int whisperMultiplierDecreaser = 5;
    private void Start() {
        InitializeVariables();
        InvokeAnEvent();
    }

    protected override IEnumerator TriggerEventConsequences() {
        while (userAnswer == Answer.Empty) {
            yield return new WaitForSeconds(1f);
        }

        if (userAnswer == Answer.Yes) {
            whisperCounter++;
        } else if (userAnswer == Answer.No) {
            whisperCounter--;
        }
        int randRewardChance = Random.Range(0, 100);
        if (whisperCounter*whisperMultiplier > randRewardChance && whisperMultiplier > 0) {
            whisperMultiplier -= whisperMultiplierDecreaser;
            tavern.tavernMoney += moneyBonus;
            ChangeMessageText(guestGaveMoney);
        } else {
            ChangeMessageText(guestLeft);
        }
        InvokeOnUserResponse();
    }

    private void InitializeVariables() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
    }
}
