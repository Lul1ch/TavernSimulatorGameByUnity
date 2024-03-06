using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class TrainingManager : MonoBehaviour
{
    [SerializeField] private Text _messageText;
    [SerializeField] private List<GameObject> objectsToHide;
    [SerializeField] private QueueCreating queueCreating;
    [Header("KeyIndexes")]
    [SerializeField] private int indexToSpawnFirstClient;
    [SerializeField] private int indexToSpawnSecondClient;
    [SerializeField] private int _indexToSaveClientOrder;
    [SerializeField] private int indexToMoveToTheKitchen;
    [SerializeField] private int indexToMoveToTheShop;
    [SerializeField] private int indexToMoveBackToTheKitchen;
    [SerializeField] private int indexToMoveBackToTheTavern;
    [SerializeField] private int indexToGiveCustomerAnOrder;
    [SerializeField] private int indexToTrainBonusPurchase;
    [SerializeField] private int indexToFinishTraining;
    [Header("ScreenSwitchButtons")]
    [SerializeField] private GameObject fromTavernToKitchenButton;
    [SerializeField] private GameObject fromKitchenToShopButton;
    [SerializeField] private GameObject fromShopToKitchenButton;
    [SerializeField] private GameObject fromKitchenToTavernButton;
    [SerializeField] private GameObject bonusesButton;

    private List<string> trainingMessages = new List<string>() { "Для начала обучения нажми стрелку вправо",  "Сейчас выйдет обычный клиент, твоя задача будет заключаться в том, чтобы обслужить его",
    "", "Теперь нажми в левом нижнем углу на стрелку вправо и перейди на экран Кухни", "Теперь нажми на кнопку \"Рецепты\" и посмотри, какие продукты требуются для приготовления заказа клиента",
    "Теперь нажми в левом нижнем углу на стрелку вправо и перейди на экран Закупки", "Теперь нажми на кнопку \"Магазин\" и купи нужные продукты, нажав напротив них кнопку \"Купить\"",
    "Теперь нажми на стрелку влево, чтобы вернуться на экран Кухни", "Теперь нажми на кнопку \"Кухня\" и приготовь нужное блюдо, нажав кнопку \"Готовить\" напротив него", 
    "Теперь нажми на стрелку влево, чтобы вернуться на экран Таверны", "Теперь нажми на кнопку \"Еда\" и отдай приготовленное блюдо, нажав кнопку \"Отдать\" напротив него",
    "После правильно выданного заказа, вам начислено одно очко репутации", "Эти очки могут быть, как положительными, так и отрицательными",
    "Они прибавляются к оплате следующих заказов, также их можно потратить для покупки бонусов",
    "Для этого нажми на кнопку \"Бонусы\" и купи бонус, нажав кнопку \"Купить\" напротив него", "Вам начислено десять монет", 
    "В игре бонусы - это будут модификации, дающие выгоду на постоянной основе, в обучении для простоты понимания бонус имеет разовый эффект",
    "Для того, чтобы лучше понимать, какой эффект даёт тот или иной бонус, наведи на него и зажми левую кнопку мыши, чтобы вывести описание бонуса",
    "Поздравляю, обучение успешно пройдено! Нажмите на стрелку вправо, чтобы закончить обучение."};
    private int _messageIndex = 0;
    private int _creditsForNextStep;
    private Dictionary<int, int> creditsOnCertainStep;
    public int messageIndex {
        set { _messageIndex = value; }
        get { return _messageIndex;}
    }

    public int indexToSaveClientOrder {
        get { return _indexToSaveClientOrder; }
        set { _indexToSaveClientOrder = value; } 
    }
    public int creditsForNextStep {
        get { return _creditsForNextStep; }
        set { _creditsForNextStep = value; 
              if (_creditsForNextStep <= 0){
                _creditsForNextStep = 0;
                ShowOrHideButtons(true);
                CheckForTheAction();
              }                           }
    }
    private int minCreditsNumber = 1;

    private void Start() {
        queueCreating.InitSpawnPoint();
        ChangeMessageText();
        InitializeCreditsDictionary();
        indexToFinishTraining = trainingMessages.Count;
    }

    public void ShowOrHideButtons(bool value) {
        foreach(var elem in objectsToHide) {
            elem.SetActive(value);
        }
    }

    public void ChangeMessageText() {
        _messageText.text = trainingMessages[_messageIndex];
    }

    public void ChangeMessageIndex(int value) {
        if (_messageIndex + value >= 0) {
            _messageIndex += value;
            
            if (_messageIndex == indexToFinishTraining) {
                SceneManager.LoadScene("MainMenu");
                return;
            }

            ShowOrHideButtons(false);
            ChangeMessageText();
            CheckForTheAction();

            if (creditsOnCertainStep.ContainsKey(_messageIndex) && creditsOnCertainStep[_messageIndex] != 0) { 
                _creditsForNextStep = creditsOnCertainStep[_messageIndex];
                creditsOnCertainStep[_messageIndex] = 0;
                ShowOrHideButtons(false); 
            }
        }
    }

    private void CheckForTheAction() {
        if (indexToSpawnFirstClient != -1 && _messageIndex == indexToSpawnFirstClient) {
            InvokeWhenFirstClientSpawned();
        } else if (_messageIndex == indexToMoveToTheKitchen) {
            ActivateGameObjectOnce(fromTavernToKitchenButton, ref indexToMoveToTheKitchen);
        } else if (_messageIndex == indexToMoveToTheShop) {
            ActivateGameObjectOnce(fromKitchenToShopButton, ref indexToMoveToTheShop);
        } else if (_messageIndex == indexToMoveBackToTheKitchen && _creditsForNextStep == 0) {
            ActivateGameObjectOnce(fromShopToKitchenButton, ref indexToMoveBackToTheKitchen);
        } else if (_messageIndex == indexToMoveBackToTheTavern && _creditsForNextStep == 0) {
            ActivateGameObjectOnce(fromKitchenToTavernButton, ref indexToMoveBackToTheTavern);
        } else if (_messageIndex == indexToTrainBonusPurchase) {
            ActivateGameObjectOnce(bonusesButton, ref indexToTrainBonusPurchase);
        } else if (_creditsForNextStep == 0) {
            ShowOrHideButtons(true);
        }
    }

    public void SaveMessage(int index, string message) {
        trainingMessages[index] = message;
    }

    private void InvokeWhenFirstClientSpawned() {
        queueCreating.SpawnCertainClient(queueCreating.orderClient);
        indexToSpawnFirstClient = -1;
    }
    private void ActivateGameObjectOnce(GameObject obj, ref int index) {
        obj.SetActive(true);
        index = -1;
    }
    private void InitializeCreditsDictionary() {
        creditsOnCertainStep = new Dictionary<int, int>() {
            [indexToMoveBackToTheKitchen - 1] = 2, //костыль
            [indexToMoveBackToTheTavern - 1] = minCreditsNumber,
            [indexToGiveCustomerAnOrder - 1] = minCreditsNumber,
            [indexToTrainBonusPurchase] = minCreditsNumber
        };
    }
}
