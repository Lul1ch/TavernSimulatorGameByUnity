using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FoodOrdering foodOrdering;
    [SerializeField] private QueueCreating queueCreating;
    [SerializeField] private Kitchen kitchen;

    private void OnEnable() {
        EventBus.onGuestSpawned += InvokeWhenANewGuestSpawned;
        EventBus.onShopFilled += FillKitchen;
    }

    private void OnDisable() {
        EventBus.onGuestSpawned -= InvokeWhenANewGuestSpawned;
        EventBus.onShopFilled -= FillKitchen;
    }

    private void InvokeWhenANewGuestSpawned() {
        foodOrdering.ClearVariablesValues();
        queueCreating.CancelSTimeIsUpInvoke();
        queueCreating.InvokeSetTimeIsUp();
    }

    private void FillKitchen() {
        kitchen.InitKitchenShowcase();
    }

}