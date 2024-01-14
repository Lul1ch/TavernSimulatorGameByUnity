using UnityEngine;
using UnityEngine.UI;

public class GiveButton : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private FoodOrdering foodOrdering;

    [SerializeField] private Tavern tavern;

    [SerializeField] private ProductInfo productInfo;
    [SerializeField] private EventGenerator events;

    [Header("Scene's objects")]
    [SerializeField] private Button giveButton;

    private void Start() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        foodOrdering = FindObjectOfType<FoodOrdering>().GetComponent<FoodOrdering>();
        events = FindObjectOfType<EventGenerator>().GetComponent<EventGenerator>();
        giveButton.onClick.AddListener(() => GiveCustomerSelectedOrder());
    }

    private void GiveCustomerSelectedOrder() {
        string foodName = productInfo.productName;
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
}
