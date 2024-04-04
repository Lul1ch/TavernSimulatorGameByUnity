using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    [SerializeField] private Text hintText;
    [SerializeField] private BoxCollider2D messageBrokerCollider;
    private float _hintLifeTime = 3f;
    public float hintLifeTime {
        get { return _hintLifeTime; }
    }
    public enum EventType {
        NotEnoughMoney, NotEnoughProducts, NotEnoughBonuses, InappropriateTimeForService, BonusAlreadyBought, MaxFoodCooking, AutomaticCookingStarted, isUnfairGuestGone
    }

    Dictionary<EventType, string> hintsForPlayer = new Dictionary<EventType, string>() {
        [EventType.NotEnoughMoney] = "Недостаточно монет для покупки этого продукта. Нужно ещё ",
        [EventType.NotEnoughProducts] = "Недостаточно продуктов для готовки данного блюда. Нужно ещё ",
        [EventType.NotEnoughBonuses] = "Недостаточно очков репутации для покупки этого бонуса. Нужно ещё ",
        [EventType.InappropriateTimeForService] = "Сейчас не требуется выдавать еду клиенту.",
        [EventType.BonusAlreadyBought] = "Бонус уже куплен.",
        [EventType.MaxFoodCooking] = "Сейчас готовится максимальное количество блюд.",
        [EventType.AutomaticCookingStarted] = "Готовка блюда успешно запущена.",
        [EventType.isUnfairGuestGone] = "'Клиент ушёл не заплатив'."
    };

    public void ShowHint(EventType eventType, string additionalString = "") {
        CancelInvoke("HideHint");
        messageBrokerCollider.enabled = false;
        string hintStr = hintsForPlayer[eventType] + additionalString;
        hintText.text = hintStr;
        gameObject.SetActive(true);
        Invoke("HideHint", hintLifeTime);
    }

    public void ShowBonusDescription(string description) {
        CancelInvoke("HideHint");
        hintText.text = description;
        gameObject.SetActive(true);
    }

    public void HideHint() {
        messageBrokerCollider.enabled = true;
        gameObject.SetActive(false);
    }

    private void OnMouseDown() {
        HideHint();
    }
}
