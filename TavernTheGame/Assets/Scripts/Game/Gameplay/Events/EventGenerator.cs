using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventGenerator : MonoBehaviour
{
    [Header("Message")]
    [SerializeField] private Text _messageText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button denyButton;

    [Header("Scripts")]
    [SerializeField] private CharactersVariants variants;
    [SerializeField] private QueueCreating queueCreator;
    [SerializeField] private Tavern tavern;

    private string messageText {
        set { _messageText.text = queueCreator.UpdateAllGenderRelatedWords(value); }
    }

    public enum Event {
        None = 0, FreeFood, SpecialOffer, Whisper, Kazahstan
    }

    public enum Answer {
        Yes, No, Empty, FreeDish
    }

    private Event curEvent;
    private Answer userChoice = Answer.Empty;
    private IEnumerator coroutine;
    private int whisperCounter;
    private int whisperMultiplier = 10;

    public void CreateAnEvent() {
        int enumLength = Event.GetNames(typeof(Event)).Length;
        int rand = Random.Range(1, enumLength);
        curEvent = (Event)rand;
        InvokeAnEvent(curEvent);
    }

    private void InvokeAnEvent(Event newEvent) {
        userChoice = Answer.Empty;
        if (newEvent == Event.FreeFood) {
            messageText = "<size=32>Слушай, друг, у меня в кармане ни гроша, дай что-нибудь подкрепиться, уже и не помню, когда ел(а) в последний раз...</size>";
            denyButton.gameObject.SetActive(true);
        } else {
            ActivateMessageButtons();
            if (newEvent == Event.SpecialOffer) {
                messageText = "<size=32>Пссс... не хочешь прикупить секретную приправу от королевского повара? Только она ... не совсем обычная</size>";
            } else if (newEvent == Event.Whisper) {
                messageText = "'Шёпотом' Ты с нами???";
            } else if (newEvent == Event.Kazahstan) {
                messageText = "Это Казахстан? Казахстан, да?";
            }
        }
        coroutine = TriggerEventConsequences(newEvent);
        StartCoroutine(coroutine);
        
    }

    private void ActivateMessageButtons() {
        confirmButton.gameObject.SetActive(true);
        denyButton.gameObject.SetActive(true);
    }

    private void HideMessageButtons() {
        confirmButton.gameObject.SetActive(false);
        denyButton.gameObject.SetActive(false);
    }

    public void UserSaidYes() {
        userChoice = Answer.Yes;
    }

    public void UserSaidNo() {
        userChoice = Answer.No;
    }

    public void UserGaveFreeFood() {
        if (curEvent == Event.FreeFood) {
            userChoice = Answer.FreeDish;
        }
    }

    private IEnumerator TriggerEventConsequences(Event newEvent) {
        while (userChoice == Answer.Empty) {
            yield return new WaitForSeconds(1f);
        }
        
        if (newEvent == Event.SpecialOffer) {
            if (userChoice == Answer.Yes) {
                int randForPunishment = Random.Range(0, 100);
                if (randForPunishment < 15) {
                    messageText = "<size=32>'Приправа действительно помогла, клиенты хвалят вашу еду за необычный вкус, лояльность к вашему заведению повышена'</size>";
                    tavern.ChangeTavernBonus(10);
                } else {
                    if (tavern.GetTavernBonus() > 0) {
                        tavern.ChangeTavernBonus(-1*tavern.GetTavernBonus());
                    }
                    tavern.DecreaseTavernMoney((int)(tavern.GetTavernMoney() / 2));
                    messageText = "<size=32>'Через некоторое время в дверях вашей таверны появляются стражники. После недолгой проверки они находят эти приправы и выписывают штраф в половину вашего бюджета, за незаконное хранение эльфийской соли. Кажется, в следующий раз стоит быть осторожнее.'</size>";
                }
            } else if (userChoice == Answer.No) {
                messageText = "Сам не знаешь, какие возможности упускаешь...";
                tavern.ChangeTavernBonus(1);
            }
        } else if (newEvent == Event.Whisper) {
            if (userChoice == Answer.Yes) {
                whisperCounter++;
            } else if (userChoice == Answer.No) {
                whisperCounter--;
            }
            int randRewardChance = Random.Range(0, 100);
            if (whisperCounter*whisperMultiplier > randRewardChance && whisperMultiplier > 0) {
                whisperMultiplier -=5;
                tavern.IncreaseTavernMoney(100);
                messageText = "<size=32>'После того,как вы одобрительно киваете головой на эту странную фразу, клиент протягивает увесистый мешок с сотней золотых монет. Совершенно не понятно была это какая-то секта или секретная организация, но вы рады вашей удаче'</size>";
            }
        } else if (newEvent == Event.Kazahstan) {
            if (userChoice == Answer.Yes) {
                messageText = "Понял, спасибо!";
            } else if (userChoice == Answer.No) {
                messageText = "Ахх...жаль.";
            }
        } else if (newEvent == Event.FreeFood) {
            if (userChoice == Answer.No) {
                messageText = "Сердца у тебя нет!";
                int randDecreseBonus = Random.Range(-3, -1);
                tavern.ChangeTavernBonus(randDecreseBonus);
            } else if (userChoice == Answer.FreeDish) {
                messageText = "Спасибо тебе, добрый человек, никогда тебя не забуду!";
                int randIncreaseBonus = Random.Range(1, 2);
                tavern.ChangeTavernBonus(randIncreaseBonus);
            }
        }
        HideMessageButtons();
        queueCreator.charStatus = QueueCreating.Status.EventIsFinished;
        StopCoroutine(coroutine);
        curEvent = Event.None;
    }

    public bool IsItAFreeFoodEvent() {
        return (curEvent == Event.FreeFood);
    }

    public void SetCurEventToNone() {
        curEvent = Event.None;
    }
}