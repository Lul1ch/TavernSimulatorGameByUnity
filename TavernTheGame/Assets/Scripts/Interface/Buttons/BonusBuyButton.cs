using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using YG;
public class BonusBuyButton : MonoBehaviour
{
    [SerializeField] private Sprite boughtSprite;
    [Header("Scene's objects")]
    [SerializeField] private Button bonusBuyButton;
    [SerializeField] private Hint hint;
    [SerializeField] private Bonus bonus;
    [SerializeField] private TrainingManager trainingManager;
    private Tavern tavern;
    private bool isReadyForNextHint = true;
    private bool _isBonusAlreadyBought = false;
    private bool isCreditDecreaseAvailable = true;

    public bool isBonusAlreadyBought {
        get { return _isBonusAlreadyBought; }
        set { _isBonusAlreadyBought = value; }
    }

    private void Start() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        bonus = transform.parent.Find("Icon").GetComponent<Bonus>();
        hint = GameObject.Find("/Tavern/TavernObjectsGroup/TavernInterface/FAQTavern").GetComponent<Hint>();
        if ( SceneManager.GetActiveScene().name == "Training") {
            trainingManager = FindObjectOfType<TrainingManager>().GetComponent<TrainingManager>();
        }
        bonusBuyButton.onClick.AddListener(() => BuySelectedBonus());
        Debug.Log("Bonus (" + bonus + ")");
        //if (YandexGame.savesData.isBonusesBought.ContainsKey(bonus.gameObject.name) && YandexGame.savesData.isBonusesBought[bonus.gameObject.name]) {
        //    BuySelectedBonus();
        //}
    }

    private void BuySelectedBonus() {
        if (tavern.tavernBonus >= bonus.price && !isBonusAlreadyBought) {
            bonus.Buy();
            tavern.ChangeTavernBonusWithOutModifier(-1*bonus.price);
            isBonusAlreadyBought = true;
            YandexGame.savesData.isBonusesBought[bonus.gameObject.name] = true;
            if ( SceneManager.GetActiveScene().name == "Training" && isCreditDecreaseAvailable) {
                trainingManager.creditsForNextStep--;
                isCreditDecreaseAvailable = false;
                gameObject.SetActive(false);
            }
            InvokeOnBonusBought();
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

    private void BuyBonus() {

    }

    private void IsReadyForNextHint() {
        isReadyForNextHint = true;
    }

    private void InvokeOnBonusBought() {
        gameObject.GetComponent<Image>().sprite = boughtSprite;
        gameObject.transform.parent.Find("Name").GetComponent<TMP_Text>().text = "<color=grey>" + gameObject.transform.parent.Find("Name").GetComponent<TMP_Text>().text + "</color>"; 
    }

}
