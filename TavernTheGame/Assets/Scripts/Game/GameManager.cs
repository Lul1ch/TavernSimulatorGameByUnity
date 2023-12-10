using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FoodOrdering foodOrdering;

    private void OnEnable() {
        EventBus.onGuestSpawned += foodOrdering.ClearVariablesValues;
        
    }

    private void OnDisable() {
        EventBus.onGuestSpawned -= foodOrdering.ClearVariablesValues;
    }

}