using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tavern : MonoBehaviour
{
    private Dictionary<string,int> foodStorage = new Dictionary<string,int>();
    private Dictionary<string, GameObject> foodSamples = new Dictionary<string, GameObject>();
    [SerializeField] private Text moneyAmount, bonusNumber;

    [SerializeField] private GameObject contentSample;
    [Header("StorageWindow")]
    [SerializeField] private Transform _foodContentParent;
    [SerializeField] private Transform _dishContentParent;
    [SerializeField] private GameObject foodHeader;
    [SerializeField] private GameObject dishHeader;

    private float _tavernMoney = 100f;
    private int _moneyBonus = 0;
    private int _bonusesValueModifier = 1;

    public float tavernMoney {
        set { _tavernMoney = value; UpdateCounterInterface();}
        get { return _tavernMoney; }
    }
    public int tavernBonus {
        set { int oldBonusValue = _moneyBonus; _moneyBonus += (value - oldBonusValue)*_bonusesValueModifier; UpdateCounterInterface();}
        get { return _moneyBonus; }
    }
    public int bonusesValueModifier {
        set { _bonusesValueModifier = value; }
        get { return _bonusesValueModifier; }
    }
    public Transform foodContentParent {
        get { return _foodContentParent; }
    }
    public Transform dishContentParent {
        get { return _dishContentParent; }
    }

    public void UpdateDictionary(string name, Transform parent, GameObject foodObject, int foodNumber = 1) {
        if (IsFoodStorageEmpty(parent)) {
            if (parent == _dishContentParent) {
                ChangeHeaderVisuability(dishHeader, true);
            } else {
                ChangeHeaderVisuability(foodHeader, true);
            }
        }
        if (foodStorage.ContainsKey(name) == true) {
            foodStorage[name]++;
        } else {
            foodStorage.Add(name, foodNumber);
            foodSamples.Add(name, foodObject);
        }
        UpdateStorageInfo(name, parent, foodObject, foodNumber);
        MoveFoodToTheTop(name, parent);
    }

    public void UpdateStorageInfo(string foodName, Transform parent, GameObject foodObject = null, int foodNumber = 1) {
        //Пытаемся найти созданный элемент интерфейса
        Transform curFood = parent.Find(foodName);
        //Если не находим, то создаём новый и добавляем в скроллер
        if (curFood == null) {
            GameObject newContentElem = Instantiate(contentSample, contentSample.transform.position, contentSample.transform.rotation);
            GameObject curFoodObject = InstantiateFoodIcon(newContentElem, foodObject);
            newContentElem.transform.Find("Number").GetComponent<Text>().text = foodNumber.ToString();
            newContentElem.transform.Find("Name").GetComponent<Text>().text = foodName;
            newContentElem.transform.Find("Give").GetComponent<GiveButton>().InitFoodVariable(curFoodObject.GetComponent<Food>());

            curFood = newContentElem.transform;

            newContentElem.name = foodName;
            newContentElem.transform.SetParent(parent, false);
        } else {
        //Если нашли, то просто обновляем счётчик
        try
        {
            curFood.Find("Number").GetComponent<Text>().text = foodStorage[foodName].ToString();
        }
        catch
        {
            Debug.Log(curFood + " " + curFood.Find("Number") + " " + curFood.Find("Number").GetComponent<Text>() + " " + foodStorage.ContainsKey(foodName).ToString());
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
        Transform parent = (foodSamples[foodName].TryGetComponent<Food>(out Food temp)) ? foodContentParent : dishContentParent;
        if (foodStorage[foodName] <= 0) {
            DestroyTavernContentElement(foodName, parent);
        } else {
            UpdateStorageInfo(foodName, parent);
        }
    }

    private void DestroyTavernContentElement(string foodName, Transform parent) {
        Transform curFood = parent.Find(foodName);
        if (curFood.gameObject != null) {
            Destroy(curFood.gameObject);
        }
        Debug.Log(IsFoodStorageEmpty(parent));
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
        return (foodContentParent.GetComponentsInChildren<Transform>().Length == 1 && dishContentParent.GetComponentsInChildren<Transform>().Length == 1);
    }

    public bool IsFoodStorageEmpty(Transform parent) {
        Debug.Log(parent.GetComponentsInChildren<Transform>().Length);
        return parent.GetComponentsInChildren<Transform>().Length == 1;
    }

    public void ChangeTavernBonusWithOutModifier(int value) {
        _moneyBonus += value;
        UpdateCounterInterface();
    }

    private void ChangeHeaderVisuability(GameObject header, bool activity) {
        header.SetActive(activity);
    }
}
