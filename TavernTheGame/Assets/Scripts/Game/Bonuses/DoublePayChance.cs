using UnityEngine;

public class DoublePayChance : Bonus
{
    public override void Buy() {
        EventBus.onDoublePayChanceBought?.Invoke();
    }
}
