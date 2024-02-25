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
                tavern.ChangeTavernBonus(10);
            } else {
                gameObject.GetComponent<SpriteRenderer>().sprite = guardianSprite;
                if (tavern.GetTavernBonus() > 0) {
                    tavern.ChangeTavernBonus(-1*tavern.GetTavernBonus());
                }
                tavern.DecreaseTavernMoney((int)(tavern.GetTavernMoney() / 2));
                ChangeMessageText(negativeEnd);
            }
        } else if (userAnswer == Answer.No) {
            ChangeMessageText(guestRefused);
            tavern.ChangeTavernBonus(1);
        }
        InvokeOnUserResponse();
    }

    private void InitializeVariables() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
    }  
}