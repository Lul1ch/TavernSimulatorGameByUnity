using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameMessage : MonoBehaviour
{
    static Result gameResult;

    private enum Result {
        Lose, Victory 
    }

    [Header("Samples")]
    [SerializeField] private Sprite lossBG;
    [SerializeField] private Sprite victoryBG;
    [SerializeField] private Sprite finishSprite;

    [Header("Components")]
    [SerializeField] private Image sceneBG;
    [SerializeField] private Text messageText;
    [SerializeField] private Image buttonImage;

    //Пока что текстовые сообщения с результатами задаём строго, потом можно добавить рандом из нескольких вариантов
    private string tempLossMessage = "Вот и всё, кажется, это конец, в твоей таверне больше нет ни единой души. Весь интерьер обветшал, в углах забилась пыль, а редкие лучи солнца - единственные твои гости. Стоит подумать над допущенными ошибками и попробовать начать свой путь с начала.";
    private string tempVictoryMessage = "Это успех! Твоя таверна наконец-то получила славу и признание, которое ты заслужил упорным трудом. Теперь все невзгоды позади, и ты можешь помочь своей семье.";

    //private List<string> LossMessages;
    //private List<string> VictoryMessages;

    private void Start() {
        /*LossMessages = new List<string>() {
            "Ты проиграл, возможно, это просто полоса невезения, не растраивайся, попробуй ещё раз!",
            "Проигрывать всегда больно, но разве конец одной истории не может быть началом новой?",
            "Ты не справился с возложенными на тебя обязанностями, и твоя таверна пришла в упадок, но ты можешь начать всё заново.",
            "Вот и всё, кажется, это конец, в твоей таверне больше нет ни единой души. Весь интерьер обветшал, в углах забилась пыль, а редкие лучи солнца - единственные твои гости. Стоит подумать над допущенными ошибками и начать."
        };*/

        //Получаем результат игры и обновляем интерфейс в зависимости от него
        string gameRes = PlayerPrefs.GetString("Result");

        if (gameRes == "Victory") {
            messageText.text = tempVictoryMessage;
            sceneBG.sprite = victoryBG;
            buttonImage.sprite = finishSprite;
        } else if (gameRes == "Loss") {
            messageText.text = tempLossMessage;
            sceneBG.sprite = lossBG;
        }
    }
}
