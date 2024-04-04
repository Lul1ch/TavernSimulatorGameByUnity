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
                ChangeMessageText(positiveEnd, false);
                tavern.tavernBonus += 10;
            } else {
                gameObject.GetComponent<SpriteRenderer>().sprite = guardianSprite;
                if (tavern.tavernBonus > 0) {
                    tavern.tavernBonus = 0;
                }
                if (tavern.tavernMoney > 0) {
                    tavern.tavernMoney -= (int)(tavern.tavernMoney / 2);
                }
                ChangeMessageText(negativeEnd, false);
            }
        } else if (userAnswer == Answer.No) {
            ChangeMessageText(guestRefused);
            tavern.tavernBonus += 1;
        }
        InvokeOnUserResponse();
    }

    private void InitializeVariables() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
    }  
}