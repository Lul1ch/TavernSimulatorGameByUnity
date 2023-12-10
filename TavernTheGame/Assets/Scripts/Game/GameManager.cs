using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FoodOrdering foodOrdering;
    [SerializeField] private GuestMover guestMover;

    private void OnEnable() {
        EventBus.onGuestSpawned += InvokeWhenANewGuestSpawned;
        
    }

    private void OnDisable() {
        EventBus.onGuestSpawned -= InvokeWhenANewGuestSpawned;
    }

    private void InvokeWhenANewGuestSpawned() {
        foodOrdering.ClearVariablesValues();
        guestMover.SetIsTimeIsUpInvoked(false);
    }

}