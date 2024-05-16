using UnityEngine;
using System;

public static class EventBus
{
    public static Action onGuestSpawned;
    public static Action onShopFilled;
    public static Action onDoublePayChanceBought;
    public static Action onAutomaticCookingBought;
    public static Action onTrainGuestToldHisOrder;
    public static Action onGuestLeft;
    public static Action onGuestReacted;
    public static Action onGameFinished;
    public static Action onBonusesIntialized;
    public static Action onFoodHolderReady;
}
