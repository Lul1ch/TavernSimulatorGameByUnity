using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GiveButton : MonoBehaviour
{
    [Header("Scene's objects")]
    [SerializeField] private Button giveButton;
    [SerializeField] private Hint hint;
    [SerializeField] private TrainingManager trainingManager;

    private FoodOrdering foodOrdering;
    private Tavern tavern;
    private Food food;
    private QueueCreating queueCreating;
    private bool isReadyForNextHint = true;

    private void Start() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        foodOrdering = FindObjectOfType<FoodOrdering>().GetComponent<FoodOrdering>();
        queueCreating = FindObjectOfType<QueueCreating>().GetComponent<QueueCreating>();
        hint = GameObject.Find("/Tavern/TavernObjectsGroup/TavernInterface/FAQTavern").GetComponent<Hint>();
        if ( SceneManager.GetActiveScene().name == "Training" ) {
            trainingManager = FindObjectOfType<TrainingManager>().GetComponent<TrainingManager>();
        }
        giveButton.onClick.AddListener(() => GiveCustomerSelectedOrder());
    }

    private void GiveCustomerSelectedOrder() {
        string foodName = food.foodName;
        if (foodOrdering.curOrder != null && tavern.IsNumberGreaterThanZero(foodName) && foodOrdering.curIssue == null && foodOrdering.isOrderTold || queueCreating.IsItAFreeFoodEvent()) {
            if ( SceneManager.GetActiveScene().name == "Training") {
                string orderFoodName = foodOrdering.curOrder.GetComponent<Food>().foodName;
                string issueFoodName = tavern.GetFoodObject(foodName).GetComponent<Food>().foodName; 
                if ( orderFoodName == issueFoodName) {
                    trainingManager.creditsForNextStep--;
                    tavern.tavernBonus += 1;
                } else {
                    return;
                }
            } 
            foodOrdering.curIssue = tavern.GetFoodObject(foodName);
            //Убавляем количество выданного продукта на складе
            tavern.ReduceFoodNumber(foodName);
            //Визуальное обновление окна склада в игре
            tavern.UpdateStorageInfo(foodName);
            
            queueCreating.SetPlayerAnswer(Event.Answer.FreeDish);
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
