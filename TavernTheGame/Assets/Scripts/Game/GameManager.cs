using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FoodOrdering foodOrdering;
    [SerializeField] private QueueCreating queueCreating;
    [SerializeField] private Kitchen kitchen;
    [SerializeField] private TrainingManager trainingManager;
    [SerializeField] private Button nextButton;

    private void OnEnable() {
        EventBus.onGuestSpawned += InvokeWhenANewGuestSpawned;
        EventBus.onShopFilled += FillKitchen;
        EventBus.onDoublePayChanceBought += IsDoublePayChanceBought;
        EventBus.onAutomaticCookingBought += IsAutomaticCookingBought;
        EventBus.onTrainGuestToldHisOrder += ShowButtons;
        EventBus.onGuestLeft += InvokeonGuestLeft;
        EventBus.onGuestReacted += InvokeOnGuestReacted;
    }

    private void OnDisable() {
        EventBus.onGuestSpawned -= InvokeWhenANewGuestSpawned;
        EventBus.onShopFilled -= FillKitchen;
        EventBus.onDoublePayChanceBought -= IsDoublePayChanceBought;
        EventBus.onAutomaticCookingBought -= IsAutomaticCookingBought;
        EventBus.onTrainGuestToldHisOrder -= ShowButtons;
        EventBus.onGuestLeft -= InvokeonGuestLeft;
        EventBus.onGuestReacted -= InvokeOnGuestReacted;
    }

    private void InvokeWhenANewGuestSpawned() {
        nextButton.gameObject.SetActive(false);
        queueCreating.InvokeSetTimeIsUp();
    }

    private void InvokeOnGuestReacted() {
        nextButton.gameObject.SetActive(true);
        queueCreating.CancelSTimeIsUpInvoke();
        queueCreating.InvokeDeferredClientDestroy();
        queueCreating.HideTimeBeforeClientLeaveText();
        foodOrdering.ClearVariablesValues();
    }
    
    private void InvokeonGuestLeft() {
        queueCreating.DestroyServicedGuest();
    }

    private void FillKitchen() {
        kitchen.InitKitchenShowcase();
    }

    private void IsDoublePayChanceBought() {
        foodOrdering.isDoublePayChanceBought = true;
    }

    private void IsAutomaticCookingBought() {
        foodOrdering.isAutomaticCookingBought = true;
    }

    private void ShowButtons(){
        trainingManager.ShowOrHideButtons(true);
    }

}