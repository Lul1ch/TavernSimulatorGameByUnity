using UnityEngine;
using UnityEngine.UI;
public class BonusBuyButton : MonoBehaviour
{
    [Header("Scene's objects")]
    [SerializeField] private Button bonusBuyButton;
    [SerializeField] private Hint hint;
    [SerializeField] private Bonus bonus;
    private Tavern tavern;
    private bool isReadyForNextHint = true;
    private bool isBonusAlreadyBought = false;

    private void Start() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        bonus = transform.parent.Find("Icon").GetComponent<Bonus>();
        hint = GameObject.Find("/Tavern/TavernObjectsGroup/TavernInterface/FAQTavern").GetComponent<Hint>();
        bonusBuyButton.onClick.AddListener(() => BuySelectedBonus());
    }

    private void BuySelectedBonus() {
        if (tavern.GetTavernBonus() >= bonus.price && !isBonusAlreadyBought) {
            bonus.Buy();
            tavern.ChangeTavernBonus(-1*bonus.price);
            isBonusAlreadyBought = true;
            Debug.Log("Bonus bought!");
        } else if (isReadyForNextHint) {
            if (!isBonusAlreadyBought) {
                hint.ShowHint(Hint.EventType.NotEnoughBonuses, string.Concat((bonus.price - tavern.GetTavernBonus()).ToString()," очков репутации."));
            } else {
                hint.ShowHint(Hint.EventType.BonusAlreadyBought);
            }
            isReadyForNextHint = false;
            Invoke("IsReadyForNextHint", hint.hintLifeTime);
        }
    }

    private void IsReadyForNextHint() {
        isReadyForNextHint = true;
    }
}
