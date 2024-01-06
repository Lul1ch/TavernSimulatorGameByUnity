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
    [SerializeField] private DishInfo dishInfo;

    [Header("Scene's objects")]
    [SerializeField] private GameObject timerSample;
    [SerializeField] private Button cookButton;
    [SerializeField] private Transform contentElemTransform;

    private void Start() {
        InitializeVariables();
        cookButton.onClick.AddListener(() => CookSelectedDish(dishInfo, contentElemTransform));
    }

    private void InitializeVariables() {
        kitchen = FindObjectOfType<Kitchen>().GetComponent<Kitchen>();
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        contentElemTransform = transform.parent;
    }

    public void CookSelectedDish(DishInfo dishInfo, Transform contentElemTransform) {
        bool isAllComponentsAvailable = true;
        Dictionary<string, int> components = dishInfo.GetDishComponents();
        foreach(var component in components) {
            if(!tavern.IsEnoughFoodInStorage(component.Key, component.Value)) {
                isAllComponentsAvailable = false;
            }
        }
        Debug.Log(isAllComponentsAvailable + "-bool counter-" + curCookingDishCounter);
        //Если компонент был куплен и его количество больше 0, то мы можем приготовить блюдо
        if (isAllComponentsAvailable && curCookingDishCounter > 0) {
            //Создаём таймер готовки блюда
            CanvasButtons.PlayOnClickSound(gameObject.GetComponent<AudioSource>());

            GameObject newTimerObject = Instantiate(timerSample, contentElemTransform.position, contentElemTransform.rotation);
            Timer curTimer = newTimerObject.GetComponent<Timer>();
            //Задаём родительский объект, для корректного отображения таймера
            newTimerObject.transform.SetParent(contentElemTransform);
            //Устанавливаем время готовки
            curTimer.SetMultiplier(dishInfo.productCookingTime);
            curCookingDishCounter--;

            coroutine = addFinishedDish(dishInfo, curTimer, components);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator addFinishedDish(DishInfo productInfo, Timer curTimer, Dictionary<string, int> components) {
        while(curTimer.GetFillAmount() > 0) {
            yield return new WaitForSeconds(1f);
        }
        //Добавляем готовое блюдо
        tavern.UpdateDict(productInfo.productName, kitchen.GetDish(productInfo.productIndex));
        //Обновляем визуальное отображение в окне склада; Убавляем счётчик для компонента блюда; Обновляем визуальное отображение в окне склада
        tavern.UpdateStorageInfo(productInfo.productName, productInfo.productSprite);
        foreach(var component in components) {
            tavern.ReduceFoodNumber(component.Key, component.Value);
            tavern.UpdateStorageInfo(component.Key);
        }
        
        curCookingDishCounter++;

        StopCoroutine(coroutine);
    }
}