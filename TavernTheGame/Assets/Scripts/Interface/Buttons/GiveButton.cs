using UnityEngine;
using UnityEngine.UI;

public class GiveButton : MonoBehaviour
{
    [Header("Scene's objects")]
    [SerializeField] private Button giveButton;
    [SerializeField] private Hint hint;

    private FoodOrdering foodOrdering;
    private Tavern tavern;
    private Food food;
    private EventGenerator events;
    private bool isReadyForNextHint = true;

    private void Start() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        foodOrdering = FindObjectOfType<FoodOrdering>().GetComponent<FoodOrdering>();
        events = FindObjectOfType<EventGenerator>().GetComponent<EventGenerator>();
        hint = GameObject.Find("/Tavern/TavernObjectsGroup/TavernInterface/FAQTavern").GetComponent<Hint>();
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
        } else if (isReadyForNextHint) {
            hint.ShowHint(Hint.EventType.InappropriateTimeForService);
            isReadyForNextHint = false;
            Invoke("IsReadyForNextHint", hint.hintLifeTime);
        }
    }

    public void InitFoodVariable(Food foodScript) {
        food = foodScript;
    }

    private void IsReadyForNextHint() {
        isReadyForNextHint = true;
    }
}
