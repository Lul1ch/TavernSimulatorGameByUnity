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
        NotEnoughMoney, NotEnoughProducts, NotEnoughBonuses, InappropriateTimeForService
    }

    Dictionary<EventType, string> hintsForPlayer = new Dictionary<EventType, string>() {
        [EventType.NotEnoughMoney] = "Недостаточно денег для покупки этого продукта. Нужно ещё ",
        [EventType.NotEnoughProducts] = "Недостаточно продуктов для готовки данного блюда. Нужно ещё ",
        [EventType.NotEnoughBonuses] = "Недостаточно очков репутации для покупки этого бонуса. Нужно ещё ",
        [EventType.InappropriateTimeForService] = "Сейчас не требуется выдавать еду клиенту."
    };

    public void ShowHint(EventType eventType, string additionalString = "") {
        string hintStr = hintsForPlayer[eventType] + additionalString;
        hintText.text = hintStr;
        gameObject.SetActive(true);
        Invoke("HideHint", hintLifeTime);
    }

    private void HideHint() {
        gameObject.SetActive(false);
    }
}
