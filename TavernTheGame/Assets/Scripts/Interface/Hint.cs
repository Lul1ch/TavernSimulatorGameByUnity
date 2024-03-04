using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hint : MonoBehaviour
{
    [SerializeField] private TMP_Text hintText;
    private float _hintLifeTime = 3f;
    public float hintLifeTime {
        get { return _hintLifeTime; }
    }
    public enum EventType {
        NotEnoughMoney, NotEnoughProducts, NotEnoughBonuses, InappropriateTimeForService, BonusAlreadyBought
    }

    Dictionary<EventType, string> hintsForPlayer = new Dictionary<EventType, string>() {
        [EventType.NotEnoughMoney] = "Недостаточно денег для покупки этого продукта. Нужно ещё ",
        [EventType.NotEnoughProducts] = "Недостаточно продуктов для готовки данного блюда. Нужно ещё ",
        [EventType.NotEnoughBonuses] = "Недостаточно очков репутации для покупки этого бонуса. Нужно ещё ",
        [EventType.InappropriateTimeForService] = "Сейчас не требуется выдавать еду клиенту.",
        [EventType.BonusAlreadyBought] = "Бонус уже куплен."
    };

    public void ShowHint(EventType eventType, string additionalString = "") {
        CancelInvoke("HideHint");
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
        gameObject.SetActive(false);
    }
}
