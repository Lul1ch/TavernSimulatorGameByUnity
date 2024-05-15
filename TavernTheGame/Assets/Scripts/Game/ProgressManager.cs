using UnityEngine;
using System.Collections;
using YG;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] private Tavern tavern;
    [SerializeField] private Transform bonusesComponentsParent;
    
    private bool _isBonusesInitialized = false;
    public bool isBonusesInitialized {
        get { return _isBonusesInitialized; }
        set { _isBonusesInitialized = value; }
    }
    private void OnEnable() => YandexGame.GetDataEvent += LoadData;
    private void OnDisable() => YandexGame.GetDataEvent -= LoadData;

    private void Start() {
        if (YandexGame.SDKEnabled == true) {
		    LoadData();
        }
    }

    public void LoadData() {
        tavern.LoadSavedData();
        StartCoroutine("LoadBoughtBonuses");
    }
    public void SaveData() {
        YandexGame.savesData.tavernMoney = tavern.tavernMoney;
        YandexGame.savesData.tavernBonus = tavern.tavernBonus;

        YandexGame.SaveProgress();
    }

    public void ResetData() {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }

    private IEnumerator LoadBoughtBonuses() {
        while (!isBonusesInitialized) {
            yield return new WaitForSeconds(1f);
        }
        foreach(Transform elem in bonusesComponentsParent) {
            int indexOfBonus = elem.Find("Icon").gameObject.GetComponent<Bonus>().bonusIndex;
            BonusBuyButton button = elem.Find("BonusBuy").gameObject.GetComponent<BonusBuyButton>();
            if (indexOfBonus < 4 && YandexGame.savesData.isBonusesBoughtMas[indexOfBonus]) { //Костыль
                button.BuyBonus();
            }
            button.InvokeOnProgressLoaded();
        }
    }
}
