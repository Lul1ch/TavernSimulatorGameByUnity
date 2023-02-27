using UnityEngine;
using System.Collections;
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
        cookButton.onClick.AddListener(() => CookSelectedDish(dishInfo, contentElemPos));
    }

    public void CookSelectedDish(DishInfo productInfo, Transform contentElemPos) {
        GameObject componentProduct = tavern.GetFoodObject(dishInfo.componentProductName);
        //Если компонент был куплен и его количество больше 0, то мы можем приготовить блюдо
        if (componentProduct != null && tavern.isNumberGreaterThanZero(dishInfo.componentProductName) && curCookingDishCounter > 0) {
            //Создаём таймер готовки блюда
            canvasButtons.PlayTheClip(cookSound);
            GameObject newTimerObject = Instantiate(timerSample, contentElemPos.position, contentElemPos.rotation);
            Timer curTimer = newTimerObject.GetComponent<Timer>();
            curTimer.enabled = true;
            //Задаём родительский объект, для корректного отображения таймера
            newTimerObject.transform.SetParent(contentElemPos);
            //Устанавливаем время готовки
            curTimer.SetMultiplier(productInfo.productCookingTime);
            curCookingDishCounter--;

            coroutine = addFinishedDish(productInfo, curTimer);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator addFinishedDish(DishInfo productInfo, Timer curTimer) {
        while(curTimer.GetFillAmount() > 0) {
            yield return new WaitForSeconds(1f);
        }
        //Добавляем готовое блюдо
        tavern.UpdateDict(productInfo.productName, kitchen.GetDish(productInfo.productIndex));
        //Обновляем визуальное отображение в окне склада; Убавляем счётчик для компонента блюда; Обновляем визуальное отображение в окне склада
        tavern.UpdateStorageInfo(productInfo.productName, productInfo.productSprite);
        tavern.ReduceFoodNumber(dishInfo.componentProductName);
        tavern.UpdateStorageInfo(dishInfo.componentProductName);
        
        curCookingDishCounter++;

        StopCoroutine(coroutine);
    }
}
