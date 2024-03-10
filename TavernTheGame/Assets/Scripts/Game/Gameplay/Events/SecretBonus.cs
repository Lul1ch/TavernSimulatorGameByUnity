using System.Collections;
using UnityEngine;

public class SecretBonus : Event
{
    [Header("SecretBonusVariables")]
    [SerializeField] private string positiveEnd;
    [SerializeField] private string negativeEnd;
    [SerializeField] private string guestRefused;
    [SerializeField] private Sprite guardianSprite;
    [SerializeField] private Tavern tavern; 

    private void Start() {
        InitializeVariables();
        InvokeAnEvent();
    }

    protected override IEnumerator TriggerEventConsequences() {
        while (userAnswer == Answer.Empty) {
            yield return new WaitForSeconds(1f);
        }

        if (userAnswer == Answer.Yes) {
            int randForPunishment = Random.Range(0, 100);
            if (randForPunishment < 15) {
                ChangeMessageText(positiveEnd);
                tavern.tavernBonus += 10*tavern.bonusesValueModifier;
            } else {
                gameObject.GetComponent<SpriteRenderer>().sprite = guardianSprite;
                tavern.tavernBonus = 0;

                tavern.tavernMoney -= (int)(tavern.tavernMoney / 2);
                ChangeMessageText(negativeEnd);
            }
        } else if (userAnswer == Answer.No) {
            ChangeMessageText(guestRefused);
            tavern.tavernBonus += 1*tavern.bonusesValueModifier;
        }
        InvokeOnUserResponse();
    }

    private void InitializeVariables() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
    }  
}