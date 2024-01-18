using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CookButton : MonoBehaviour
{
    private IEnumerator coroutine;
    //Счётсиком будем регулировать,чтоб в один момент готовилось не более трёх блюд
    static int curCookingDishCounter = 3;

    [Header("Scripts")]
    [SerializeField] private Kitchen kitchen;
    [SerializeField] private Tavern tavern;
    [SerializeField] private Dish dishScript;

    [Header("Scene's objects")]
    [SerializeField] private GameObject timerSample;
    [SerializeField] private Button cookButton;
    [SerializeField] private Transform contentElemTransform;
    [SerializeField] private GameObject dishObj;

    private void Start() {
        InitializeVariables();
        cookButton.onClick.AddListener(() => CookSelectedDish(contentElemTransform));
    }

    private void InitializeVariables() {
        kitchen = FindObjectOfType<Kitchen>().GetComponent<Kitchen>();
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        contentElemTransform = transform.parent;
    }

    public void CookSelectedDish(Transform contentElemTransform) {
        bool isAllComponentsAvailable = true;
        Dictionary<GameObject, int> components = dishScript.componentsObjects;
        foreach(var component in components) {
            if(!tavern.IsEnoughFoodInStorage(component.Key.name, component.Value)) {
                isAllComponentsAvailable = false;
            }
        }
        
        //Если компонент был куплен и его количество больше 0, то мы можем приготовить блюдо
        if (isAllComponentsAvailable && curCookingDishCounter > 0) {
            //Создаём таймер готовки блюда
            CanvasButtons.PlayOnClickSound(gameObject.GetComponent<AudioSource>());

            GameObject newTimerObject = Instantiate(timerSample, contentElemTransform.position, contentElemTransform.rotation);
            Timer curTimer = newTimerObject.GetComponent<Timer>();
            //Задаём родительский объект, для корректного отображения таймера
            newTimerObject.transform.SetParent(contentElemTransform);
            //Устанавливаем время готовки
            curTimer.GetTimerTextObject();
            curTimer.SetMultiplier(dishScript.dishCookingTime);
            curCookingDishCounter--;

            coroutine = addFinishedDish(curTimer, components);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator addFinishedDish(Timer curTimer, Dictionary<GameObject, int> components) {
        while(curTimer.GetFillAmount() > 0) {
            yield return new WaitForSeconds(1f);
        }

        tavern.UpdateDictionary(dishScript.foodName, dishObj);
        tavern.UpdateStorageInfo(dishScript.foodName, dishObj);
        
        foreach(var component in components) {
            tavern.ReduceFoodNumber(component.Key.name, component.Value);
            tavern.UpdateStorageInfo(component.Key.name);
        }
        
        curCookingDishCounter++;

        StopCoroutine(coroutine);
    }

    public void InitDishVariable(GameObject obj) {
        dishObj = obj;
        dishScript = obj.GetComponent<Dish>();
    }
}
