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

    [SerializeField] private CharactersVariants variants;
    [SerializeField] private GameObject contentSample;
    [SerializeField] private Transform parent;

    private float tavernMoney = 10f;
    private int moneyBonus;

    private void Start() {
        Debug.Log(parent.GetComponentsInChildren<Transform>().Length);
    }

    private void FixedUpdate() {
        //Обновляем интерфейс со счётчиками клиентов и денег таверны
        UpdateCounterInterface();
    }

    public void UpdateDict(string name, GameObject foodObject) {
        //Если еда уже есть на складе, то просто увеличиваем её счётчик
        if (foodStorage.ContainsKey(name) == true) {
            foodStorage[name]++;
        } else {
        //В противном случае добавляем её на склад и образец в отдельный массив
            foodStorage.Add(name, 1);
            foodSamples.Add(name, foodObject);
        }
    }

    public void UpdateStorageInfo(string foodName, Sprite icon = null) {
        //Пытаемся найти созданный элемент интерфейса
        Transform curFood = parent.Find(foodName);
        //Если не находим, то создаём новый и добавляем в скроллер
        if (curFood == null) {
            GameObject newContentElem = Instantiate(contentSample, contentSample.transform.position, contentSample.transform.rotation);
            newContentElem.name = foodName;
            newContentElem.transform.Find("Icon").GetComponent<Image>().sprite = icon;
            newContentElem.transform.Find("Number").GetComponent<Text>().text = "1";
            newContentElem.transform.Find("Give").GetComponent<ProductInfo>().productName = foodName;
            newContentElem.transform.Find("Name").GetComponent<Text>().text = foodName;
            
            newContentElem.transform.SetParent(parent, false);
        } else {
        //Если нашли, то просто обновляем счётчик
            curFood.Find("Number").GetComponent<Text>().text = foodStorage[foodName].ToString();
        }
    }

    private void UpdateCounterInterface() {
        guestsNumber.text = "<size=64>" + variants.Characters.Count;
        moneyAmount.text = "<size=56>" + tavernMoney;
        bonusNumber.text = "<size=56>" + moneyBonus;
    }

    public GameObject GetFoodObject(string foodName) {
        if (foodSamples.ContainsKey(foodName)){
            return foodSamples[foodName];
        }
        return null;
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
        Destroy(curFood.gameObject);
    }

    public bool isNumberGreaterThanZero(string foodName) {
        return (foodStorage[foodName] > 0);
    }

    public bool isFoodStorageEmpty() {
        return (parent.GetComponentsInChildren<Transform>().Length == 1);
    }

    public void IncreaseTavernMoney(int number) {
        tavernMoney += number;
    }

    public void DecreaseTavernMoney(int number) {
        tavernMoney -= number;
    }

    public float GetTavernMoney() {
        return tavernMoney;
    }

    public void ChangeTavernBonus(int number) {
        moneyBonus += number;
    }

    public int GetTavernBonus() {
        return moneyBonus;
    }

    public void SetTavernMoney(float number) {
        if (PlayerPrefs.GetString("Result") != null) {
            tavernMoney = number;
        }
    }
}
