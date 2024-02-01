using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FoodOrdering foodOrdering;
    [SerializeField] private QueueCreating queueCreating;
    [SerializeField] private Kitchen kitchen;

    private void OnEnable() {
        EventBus.onGuestSpawned += InvokeWhenANewGuestSpawned;
        EventBus.onShopFilled += FillKitchen;
        EventBus.onDoublePayChanceBought += IsDoublePayChanceBought;
        EventBus.onAutomaticCookingBought += IsAutomaticCookingBought;
    }

    private void OnDisable() {
        EventBus.onGuestSpawned -= InvokeWhenANewGuestSpawned;
        EventBus.onShopFilled -= FillKitchen;
        EventBus.onDoublePayChanceBought -= IsDoublePayChanceBought;
        EventBus.onAutomaticCookingBought -= IsAutomaticCookingBought;
    }

    private void InvokeWhenANewGuestSpawned() {
        foodOrdering.ClearVariablesValues();
        queueCreating.CancelSTimeIsUpInvoke();
        queueCreating.InvokeSetTimeIsUp();
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

}