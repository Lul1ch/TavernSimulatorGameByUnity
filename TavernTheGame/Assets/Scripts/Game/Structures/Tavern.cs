using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Tavern : MonoBehaviour
{
    private Dictionary<string,int> foodStorage = new Dictionary<string,int>();
    private Dictionary<string, GameObject> foodSamples = new Dictionary<string, GameObject>();
    [SerializeField] private TMP_Text moneyAmount, guestsNumber, bonusNumber;

    [SerializeField] private GameObject contentSample;
    [SerializeField] private Transform parent;

    private float _tavernMoney = 100f;
    private int _moneyBonus;

    public float tavernMoney {
        set { _tavernMoney = value; UpdateCounterInterface();}
        get { return _tavernMoney; }
    }
    public int tavernBonus {
        set { _moneyBonus = value; UpdateCounterInterface();}
        get { return _moneyBonus; }
    }

    public void UpdateDictionary(string name, GameObject foodObject) {
        //Если еда уже есть на складе, то просто увеличиваем её счётчик
        if (foodStorage.ContainsKey(name) == true) {
            foodStorage[name]++;
        } else {
        //В противном случае добавляем её на склад и образец в отдельный массив
            foodStorage.Add(name, 1);
            foodSamples.Add(name, foodObject);
        }
    }

    public void UpdateStorageInfo(string foodName, GameObject foodObject = null) {
        //Пытаемся найти созданный элемент интерфейса
        Transform curFood = parent.Find(foodName);
        //Если не находим, то создаём новый и добавляем в скроллер
        if (curFood == null) {
            GameObject newContentElem = Instantiate(contentSample, contentSample.transform.position, contentSample.transform.rotation);
            GameObject curFoodObject = InstantiateFoodIcon(newContentElem, foodObject);
            newContentElem.transform.Find("Number").GetComponent<Text>().text = "1";
            newContentElem.transform.Find("Name").GetComponent<Text>().text = foodName;
            newContentElem.transform.Find("Give").GetComponent<GiveButton>().InitFoodVariable(curFoodObject.GetComponent<Food>());

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
            Debug.Log(curFood + " " + curFood.Find("Number") + " " + curFood.Find("Number").GetComponent<Text>() + " " + foodStorage[foodName].ToString());
        }
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
        moneyAmount.text = "<size=56>" + _tavernMoney;
        bonusNumber.text = "<size=56>" + _moneyBonus;
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
        if (foodStorage[foodName] <= 0) {
            DestroyTavernContentElement(foodName);
        }
    }

    private void DestroyTavernContentElement(string foodName) {
        Transform curFood = parent.Find(foodName);
        if (curFood.gameObject != null) {
            Destroy(curFood.gameObject);
        }
    }

    public bool IsNumberGreaterThanZero(string foodName) {
        return (foodStorage[foodName] > 0);
    }

    public bool IsFoodStorageEmpty() {
        return (parent.GetComponentsInChildren<Transform>().Length == 1);
    }
}
