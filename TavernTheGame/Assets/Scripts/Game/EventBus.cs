using UnityEngine;
using System;

public static class EventBus
{
    public static Action onGuestSpawned;
    public static Action onShopFilled;
    public static Action onDoublePayChanceBought;
    public static Action onAutomaticCookingBought;
    public static Action onTrainGuestToldHisOrder;
}
