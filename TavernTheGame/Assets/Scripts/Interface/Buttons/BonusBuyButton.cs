using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BonusBuyButton : MonoBehaviour
{
    [Header("Scene's objects")]
    [SerializeField] private Button bonusBuyButton;
    [SerializeField] private Hint hint;
    [SerializeField] private Bonus bonus;
    [SerializeField] private TrainingManager trainingManager;
    private Tavern tavern;
    private bool isReadyForNextHint = true;
    private bool isBonusAlreadyBought = false;
    private bool isCreditDecreaseAvailable = true;

    private void Start() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        bonus = transform.parent.Find("Icon").GetComponent<Bonus>();
        hint = GameObject.Find("/Tavern/TavernObjectsGroup/TavernInterface/FAQTavern").GetComponent<Hint>();
        if ( SceneManager.GetActiveScene().name == "Training") {
            trainingManager = FindObjectOfType<TrainingManager>().GetComponent<TrainingManager>();
        }
        bonusBuyButton.onClick.AddListener(() => BuySelectedBonus());
    }

    private void BuySelectedBonus() {
        if (tavern.tavernBonus >= bonus.price && !isBonusAlreadyBought) {
            bonus.Buy();
            tavern.tavernBonus -= bonus.price;
            isBonusAlreadyBought = true;
            if ( SceneManager.GetActiveScene().name == "Training" && isCreditDecreaseAvailable) {
                trainingManager.creditsForNextStep--;
                isCreditDecreaseAvailable = false;
                gameObject.SetActive(false);
            }
            Debug.Log("Bonus bought!");
        } else if (isReadyForNextHint) {
            if (!isBonusAlreadyBought) {
                int priceDiff = bonus.price - tavern.tavernBonus;
                hint.ShowHint(Hint.EventType.NotEnoughBonuses, (priceDiff).ToString() + ".");
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
