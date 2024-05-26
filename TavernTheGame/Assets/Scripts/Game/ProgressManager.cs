using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using YG;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] private Tavern tavern;
    [SerializeField] private Transform bonusesComponentsParent;
    
    private static bool isTrainingScene = false;
    private bool _isBonusesInitialized = false;
    public bool isBonusesInitialized {
        get { return _isBonusesInitialized; }
        set { _isBonusesInitialized = value; }
    }
    private void OnEnable() => YandexGame.GetDataEvent += LoadData;
    private void OnDisable() => YandexGame.GetDataEvent -= LoadData;

    private void Start() {
        if ( SceneManager.GetActiveScene().name == "Training" ) {
            isTrainingScene = true;
        } else {
            isTrainingScene = false;
        }
        if (YandexGame.SDKEnabled == true) {
		    LoadData();
        }
    }

    public void LoadData() {
        if (isTrainingScene || SceneManager.GetActiveScene().name == "Training") {
            return;
        }
        Debug.Log("jsonDictionary = " +  YandexGame.savesData.jsonDictionary);
        Debug.Log("jsonBonuses = " +  YandexGame.savesData.jsonBonuses);
        YandexGame.savesData.foodMap = JsonConvert.DeserializeObject<Dictionary<string, int>>(YandexGame.savesData.jsonDictionary);
        tavern.LoadSavedData();
        StartCoroutine("LoadBoughtBonuses");
    }
    public void SaveData() {
        if ( isTrainingScene ) {
            return;
        }
        YandexGame.savesData.tavernMoney = tavern.tavernMoney;
        YandexGame.savesData.tavernBonus = tavern.tavernBonus;
        SaveProgressToYG();
    }

    public static void ResetData() {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }

    public static void SaveBoughtBonus(string bonusName) {
        if (YandexGame.SDKEnabled == false || isTrainingScene) {
            return;
        }
        if (!YandexGame.savesData.boughtBonusesMap.ContainsKey(bonusName)) {
            YandexGame.savesData.boughtBonusesMap.Add(bonusName, true);
        }
        YandexGame.savesData.jsonBonuses = JsonConvert.SerializeObject(YandexGame.savesData.boughtBonusesMap);
        SaveProgressToYG();
    }

    private IEnumerator LoadBoughtBonuses() {
        while (!isBonusesInitialized) {
            yield return new WaitForSeconds(1f);
        }
        YandexGame.savesData.boughtBonusesMap = JsonConvert.DeserializeObject<Dictionary<string, bool>>(YandexGame.savesData.jsonBonuses);
        foreach(Transform elem in bonusesComponentsParent) {
            string bonusName = elem.Find("Icon").gameObject.GetComponent<Bonus>().bonusName;
            BonusBuyButton button = elem.Find("BonusBuy").gameObject.GetComponent<BonusBuyButton>();
            if (YandexGame.savesData.boughtBonusesMap.ContainsKey(bonusName) && YandexGame.savesData.boughtBonusesMap[bonusName]) {
                button.BuyBonus();
            }
            button.InvokeOnProgressLoaded();
        }
    }

    public void SaveFoodData() {
        YandexGame.savesData.jsonDictionary = JsonConvert.SerializeObject(YandexGame.savesData.foodMap);
        SaveProgressToYG();
    }
    public void AddFood(string foodName, int foodNumber = 1) {
        if ( isTrainingScene ) {
            return;
        }

        if (YandexGame.savesData.foodMap.ContainsKey(foodName)) {
            YandexGame.savesData.foodMap[foodName] = foodNumber;
        } else {
            YandexGame.savesData.foodMap.Add(foodName, foodNumber);
        }
        SaveFoodData();
    }

    public void RemoveFood(string foodName, int foodNumber) {
        if ( isTrainingScene ) {
            return;
        }

        if (foodNumber > 0) {
            YandexGame.savesData.foodMap[foodName] = foodNumber;
        } else {
            YandexGame.savesData.foodMap.Remove(foodName);
        }
        SaveFoodData();
    }

    private static void SaveProgressToYG() {
        YandexGame.savesData.isSomeProgressSaved = true;
        YandexGame.SaveProgress();
    }
}
