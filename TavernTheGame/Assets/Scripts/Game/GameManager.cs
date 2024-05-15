using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FoodOrdering foodOrdering;
    [SerializeField] private QueueCreating queueCreating;
    [SerializeField] private Kitchen kitchen;
    [SerializeField] private TrainingManager trainingManager;
    [SerializeField] private Button nextButton;
    [SerializeField] private ProgressManager progressManager;
    private int bonusCounter = YandexGame.savesData.bonus_number;

    private void OnEnable() {
        EventBus.onGuestSpawned += InvokeWhenANewGuestSpawned;
        EventBus.onShopFilled += FillKitchen;
        EventBus.onDoublePayChanceBought += IsDoublePayChanceBought;
        EventBus.onAutomaticCookingBought += IsAutomaticCookingBought;
        EventBus.onTrainGuestToldHisOrder += ShowButtons;
        EventBus.onGuestLeft += InvokeonGuestLeft;
        EventBus.onGuestReacted += InvokeOnGuestReacted;
        EventBus.onGameFinished += InvokeOnGameFinished;
        EventBus.onBonusesIntialized += InvokeOnBonusesIntialized;
    }

    private void OnDisable() {
        EventBus.onGuestSpawned -= InvokeWhenANewGuestSpawned;
        EventBus.onShopFilled -= FillKitchen;
        EventBus.onDoublePayChanceBought -= IsDoublePayChanceBought;
        EventBus.onAutomaticCookingBought -= IsAutomaticCookingBought;
        EventBus.onTrainGuestToldHisOrder -= ShowButtons;
        EventBus.onGuestLeft -= InvokeonGuestLeft;
        EventBus.onGuestReacted -= InvokeOnGuestReacted;
        EventBus.onGameFinished -= InvokeOnGameFinished;
        EventBus.onBonusesIntialized -= InvokeOnBonusesIntialized;
    }

    private void InvokeWhenANewGuestSpawned() {
        nextButton.gameObject.SetActive(false);
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

    private void InvokeOnGameFinished() {
        queueCreating.gameIsNotEnd = false;
    }

    private void InvokeOnBonusesIntialized() {
        bonusCounter--;
        if (bonusCounter <= 0) {
            progressManager.isBonusesInitialized = true;
        }
    }
}