using UnityEngine;

public class AutomaticDishCooking : Bonus
{
    public override void Buy() {
        EventBus.onAutomaticCookingBought?.Invoke();
    }
}
