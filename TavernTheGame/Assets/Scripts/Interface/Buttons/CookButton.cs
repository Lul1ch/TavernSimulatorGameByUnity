using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CookButton : MonoBehaviour
{
    private IEnumerator coroutine;
    //Счётсиком будем регулировать,чтоб в один момент готовилось не более трёх блюд
    static int curCookingDishCounter = 3;

    [SerializeField] private CanvasButtons canvasButtons;
    [SerializeField] private AudioClip cookSound;
    [Header("Kitchen")]
    [SerializeField] private Kitchen kitchen;
    [SerializeField] private Tavern tavern;
    [SerializeField] private GameObject timerSample;
    [SerializeField] private Button cookButton;
    [SerializeField] private DishInfo dishInfo;
    [SerializeField] private Transform contentElemPos;

    private void Start() {
        InitializeVariables();
        cookButton.onClick.AddListener(() => CookSelectedDish(dishInfo, contentElemPos));
    }

    private void InitializeVariables() {
        kitchen = FindObjectOfType<Kitchen>().GetComponent<Kitchen>();
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        canvasButtons = FindObjectOfType<CanvasButtons>().GetComponent<CanvasButtons>();
        contentElemPos = transform.parent;
    }

    public void CookSelectedDish(DishInfo dishInfo, Transform contentElemPos) {
        bool isAllComponentsAvailable = true;
        Dictionary<string, int> components = dishInfo.GetDishComponents();
        foreach(var component in components) {
            if(!tavern.IsEnoughFoodInStorage(component.Key, component.Value)) {
                isAllComponentsAvailable = false;
            }
        }
        //Если компонент был куплен и его количество больше 0, то мы можем приготовить блюдо
        if (isAllComponentsAvailable && tavern.isNumberGreaterThanZero(dishInfo.componentProductName) && curCookingDishCounter > 0) {
            //Создаём таймер готовки блюда
            canvasButtons.PlayTheClip(cookSound);
            GameObject newTimerObject = Instantiate(timerSample, contentElemPos.position, contentElemPos.rotation);
            Timer curTimer = newTimerObject.GetComponent<Timer>();
            //Задаём родительский объект, для корректного отображения таймера
            newTimerObject.transform.SetParent(contentElemPos);
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
        }
       
        tavern.UpdateStorageInfo(dishInfo.componentProductName);
        
        curCookingDishCounter++;

        StopCoroutine(coroutine);
    }
}
