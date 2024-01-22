using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FoodOrdering foodOrdering;
    [SerializeField] private QueueCreating queueCreating;

    private void OnEnable() {
        EventBus.onGuestSpawned += InvokeWhenANewGuestSpawned;
        
    }

    private void OnDisable() {
        EventBus.onGuestSpawned -= InvokeWhenANewGuestSpawned;
    }

    private void InvokeWhenANewGuestSpawned() {
        foodOrdering.ClearVariablesValues();
        queueCreating.CancelSTimeIsUpInvoke();
        queueCreating.InvokeSetTimeIsUp();
    }

}