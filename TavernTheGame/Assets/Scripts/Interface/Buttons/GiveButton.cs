using UnityEngine;
using UnityEngine.UI;

public class GiveButton : MonoBehaviour
{
    [Header("Scene's objects")]
    [SerializeField] private Button giveButton;

    private FoodOrdering foodOrdering;
    private Tavern tavern;
    private Food food;
    private EventGenerator events;

    private void Start() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        foodOrdering = FindObjectOfType<FoodOrdering>().GetComponent<FoodOrdering>();
        events = FindObjectOfType<EventGenerator>().GetComponent<EventGenerator>();
        giveButton.onClick.AddListener(() => GiveCustomerSelectedOrder());
    }

    private void GiveCustomerSelectedOrder() {
        string foodName = food.foodName;
        //Если от клиента есть заказ, он озвучен, также заказ не был ещё выдан и выбранный продукт присутствует на складе(его количество больше 0), то мы выдаём заказ
        if (foodOrdering.curOrder != null && tavern.IsNumberGreaterThanZero(foodName) && foodOrdering.curIssue == null && foodOrdering.isOrderTold || events.IsItAFreeFoodEvent()) {
            //Уставливаем выданный заказ
            foodOrdering.curIssue = tavern.GetFoodObject(foodName);
            //Убавляем количество выданного продукта на складе
            tavern.ReduceFoodNumber(foodName);
            //Визуальное обновление окна склада в игре
            tavern.UpdateStorageInfo(foodName);
            
            events.UserGaveFreeFood();
            //gameObject.GetComponent<AudioSource>().enabled = true;
            CanvasButtons.PlayOnClickSound(gameObject.GetComponent<AudioSource>());
        } 
    }

    public void InitFoodVariable(Food foodScript) {
        food = foodScript;
    }
}
