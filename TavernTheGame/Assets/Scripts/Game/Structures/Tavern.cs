using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class Tavern : MonoBehaviour
{
    private Dictionary<string,int> foodStorage = new Dictionary<string,int>();
    private Dictionary<string, GameObject> foodSamples = new Dictionary<string, GameObject>();
    [SerializeField] private Text moneyAmount, bonusNumber;
    [SerializeField] private GameObject contentSample;
    [SerializeField] private ProgressManager progressManager;
    [SerializeField] private FoodHolder foodHolder;
    [Header("StorageWindow")]
    [SerializeField] private Transform _productContentParent;
    [SerializeField] private Transform _dishContentParent;
    [SerializeField] private GameObject foodHeader;
    [SerializeField] private GameObject dishHeader;

    private int _tavernMoney = 100;
    private int _moneyBonus = 0;
    private int _bonusesValueModifier = 1;
    private bool _isFoodHolderReady = false;

    public Transform productContentParent {
        get { return _productContentParent; }
    }
    public Transform dishContentParent {
        get { return _dishContentParent; }
    }
    public int tavernMoney {
        set { _tavernMoney = value; 
              UpdateCounterInterface(); 
              if(progressManager != null) { progressManager.SaveData(); }
            }
        get { return _tavernMoney; }
    }
    public int tavernBonus {
        set { int oldBonusValue = _moneyBonus; 
              _moneyBonus += (value - oldBonusValue)*_bonusesValueModifier; 
              UpdateCounterInterface(); 
              if(progressManager != null) { progressManager.SaveData(); } 
            }
        get { return _moneyBonus; }
    }
    public int bonusesValueModifier {
        set { _bonusesValueModifier = value; }
        get { return _bonusesValueModifier; }
    }
    public bool isFoodHolderReady {
        get { return _isFoodHolderReady; }
        set { _isFoodHolderReady = value; }
    }
    //private void Start() {
    //    ProgressManager.ResetData();
    //}

    public void UpdateDictionary(string foodName, Transform parent, GameObject foodObject, int foodNumber = 1, bool isNeedToSaveProgress = true) {
        if (foodStorage.ContainsKey(foodName) == true) {
            foodStorage[foodName]++;
        } else {
            foodStorage.Add(foodName, foodNumber);
            foodSamples.Add(foodName, foodObject);
        }
        UpdateStorageInfo(foodName, parent, foodObject, foodNumber);
        MoveFoodToTheTop(foodName, parent);
        foodNumber = foodStorage[foodName];
        if (isNeedToSaveProgress && foodNumber > 0 ) {
            progressManager.AddFood(foodName, foodNumber);
        }
    }

    public void UpdateStorageInfo(string foodName, Transform parent, GameObject foodObject = null, int foodNumber = 1, bool force = false) {
        if (foodName == "") {
            Debug.Log("Tavern r63 (" + foodName + ")");
            foodName = foodObject.GetComponent<Food>().foodName;
        }
        if (IsFoodStorageEmpty(parent)) {
            if (parent == _dishContentParent) {
                ChangeHeaderVisuability(dishHeader, true);
            } else {
                ChangeHeaderVisuability(foodHeader, true);
            }
        }
        Transform curFood = parent.Find(foodName);
        if (curFood == null || curFood == parent) {
            GameObject newContentElem = Instantiate(contentSample, contentSample.transform.position, contentSample.transform.rotation);
            GameObject curFoodObject = InstantiateFoodIcon(newContentElem, foodObject);
            newContentElem.transform.Find("Number").GetComponent<Text>().text = foodNumber.ToString();
            newContentElem.transform.Find("Name").GetComponent<Text>().text = foodName;
            newContentElem.transform.Find("Give").GetComponent<GiveButton>().InitFoodVariable(curFoodObject.GetComponent<Food>());

            curFood = newContentElem.transform;

            newContentElem.name = foodName;
            newContentElem.transform.SetParent(parent, false);
            if (force) {
                Destroy(newContentElem);
            } else {
                UpdateStorageInfo(foodName + "_temp", parent, foodObject, foodNumber, true);
            }
        } else {
        //Если нашли, то просто обновляем счётчик
            try
            {
                curFood.Find("Number").GetComponent<Text>().text = foodStorage[foodName].ToString();
            }
            catch
            {
                Debug.Log(curFood + " " + foodStorage.ContainsKey(foodName).ToString());
            }
        }
    }

    public void MoveFoodToTheTop(string foodName, Transform parent) {
        Transform curFood = parent.Find(foodName);
        if (curFood != null) {
            curFood.SetSiblingIndex(0);
        }
    }

    private GameObject InstantiateFoodIcon(GameObject curContentElement, GameObject objToInstantiate) {
            Transform curIconTransform = curContentElement.transform.Find("PositionForIcon");
            GameObject iconObject = GameObject.Instantiate(objToInstantiate, curIconTransform.position, Quaternion.identity);

            iconObject.transform.SetParent(curContentElement.transform, false);
            Destroy(curIconTransform.gameObject);

            iconObject.name = "Icon";
            
            return iconObject;
    }

    private void UpdateCounterInterface() {
        moneyAmount.text = _tavernMoney.ToString();
        bonusNumber.text = _moneyBonus.ToString();
    }

    public GameObject GetFoodObject(string foodName) {
        if (foodSamples.ContainsKey(foodName)){
            return foodSamples[foodName];
        }
        return null;
    }

    public int GetNumberOfFoodInStorage(string foodName) {
        if (foodStorage.ContainsKey(foodName)){
            return foodStorage[foodName];
        }
        return 0;
    }

    public bool IsEnoughFoodInStorage(string foodName, int foodAmount) {
        if (foodStorage.ContainsKey(foodName)){
            return (foodStorage[foodName] >= foodAmount) ? true : false;
        }
        return false;
    }

    public void ReduceFoodNumber(string foodName, int foodNumber = 1) {
        foodStorage[foodName] -= foodNumber;
        Transform parent = (foodSamples[foodName].TryGetComponent<Product>(out Product temp)) ? productContentParent : dishContentParent;
        progressManager.RemoveFood(foodName, foodStorage[foodName]);
        if (foodStorage[foodName] <= 0) {
            DestroyTavernContentElement(foodName, parent);
        } else {
            UpdateStorageInfo(foodName, parent);
        }
    }

    private void DestroyTavernContentElement(string foodName, Transform parent) {
        Transform curFood = parent.Find(foodName);
        if (curFood.gameObject != null) {
            curFood.SetParent(null);
            Destroy(curFood.gameObject);
        }
        foodStorage.Remove(foodName);
        foodSamples.Remove(foodName);

        if (IsFoodStorageEmpty(parent)) {
            if (parent == _dishContentParent) {
                ChangeHeaderVisuability(dishHeader, false);
            } else {
                ChangeHeaderVisuability(foodHeader, false);
            }
        }
    }

    public bool IsNumberGreaterThanZero(string foodName) {
        return (foodStorage.ContainsKey(foodName) && foodStorage[foodName] > 0);
    }

    public bool IsFoodStoragesEmpty() {
        return IsFoodStorageEmpty(productContentParent) && IsFoodStorageEmpty(dishContentParent);
    }

    public bool IsFoodStorageEmpty(Transform parent) {
        return parent.childCount == 0;
    }

    public void ChangeTavernBonusWithOutModifier(int value) {
        _moneyBonus += value;
        UpdateCounterInterface();
        progressManager.SaveData();
    }

    private void ChangeHeaderVisuability(GameObject header, bool activity) {
        if (header != null) {
            header.SetActive(activity);
        }
    }

    public void LoadSavedData() {
        _tavernMoney = YandexGame.savesData.tavernMoney;
        _moneyBonus = YandexGame.savesData.tavernBonus;
        UpdateCounterInterface();
        StartCoroutine("LoadSavedFood");
    }

    private IEnumerator LoadSavedFood() {
        while (!isFoodHolderReady) {
            yield return new WaitForSeconds(1f);
        }
        foreach (var elem in YandexGame.savesData.foodMap) {
            GameObject foodSample = foodHolder.GetFoodSample(elem.Key);
            Transform parent = (foodSample.TryGetComponent<Product>(out Product temp)) ? productContentParent : dishContentParent;
            UpdateDictionary(elem.Key, parent, foodSample, elem.Value, false);
        }
        foodHolder.ClearStorages();
    }
}
