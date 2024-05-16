using UnityEngine;
using System.Collections;
using YG;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] private Tavern tavern;
    [SerializeField] private Transform bonusesComponentsParent;
    
    private bool _isBonusesInitialized = false;
    private bool isProgressLoaded = false;
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
        if (isProgressLoaded) {
            return;
        }
        tavern.LoadSavedData();
        StartCoroutine("LoadBoughtBonuses");
        FillFoodIndexes();
        isProgressLoaded = true;
    }
    public void SaveData() {
        YandexGame.savesData.tavernMoney = tavern.tavernMoney;
        YandexGame.savesData.tavernBonus = tavern.tavernBonus;

        YandexGame.SaveProgress();
    }

    public static void ResetData() {
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
            if (indexOfBonus < YandexGame.savesData.bonus_number && YandexGame.savesData.isBonusesBoughtMas[indexOfBonus]) { //Костыль
                button.BuyBonus();
            }
            button.InvokeOnProgressLoaded();
        }
    }

    public void SaveFood(string foodName, int foodNumber, bool isNeedToSave = true) {
        if (!isNeedToSave) {
            return;
        }
        int foodIndex = YandexGame.savesData.foodNames.Count;
        if (YandexGame.savesData.foodIndexes.ContainsKey(foodName)) {
            foodIndex = YandexGame.savesData.foodIndexes[foodName];
            YandexGame.savesData.foodNumbers[foodIndex] = foodNumber;
        } else {
            YandexGame.savesData.foodNames.Add(foodName);
            YandexGame.savesData.foodNumbers.Add(foodNumber);
            YandexGame.savesData.foodIndexes.Add(foodName, foodIndex);
        }
        YandexGame.SaveProgress();
    }

    public void FillFoodIndexes() {
        for(int i = 0; i < YandexGame.savesData.foodNames.Count; i++) {
            string foodName = YandexGame.savesData.foodNames[i];
            YandexGame.savesData.foodIndexes.Add(foodName, i);
        }
    }

    public void RemoveApsentFood(string foodName) {
        if (!YandexGame.savesData.foodIndexes.ContainsKey(foodName)) {
            return;
        }
        int foodIndex = YandexGame.savesData.foodIndexes[foodName];
        YandexGame.savesData.foodNames.RemoveAt(foodIndex);
        YandexGame.savesData.foodNumbers.RemoveAt(foodIndex);
        YandexGame.savesData.foodIndexes.Remove(foodName);
    }
}
