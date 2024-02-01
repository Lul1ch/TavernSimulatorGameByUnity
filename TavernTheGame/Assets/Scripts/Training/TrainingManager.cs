using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrainingManager : MonoBehaviour
{
    [SerializeField] private Text _messageText;
    [SerializeField] private int indexToSpawnFirstClient;
    [SerializeField] private int indexToSpawnSecondClient;
    [SerializeField] private List<GameObject> objectsToHide;
    [SerializeField] private QueueCreating queueCreating;
    private List<string> trainingMessages = new List<string>() { "Чтобы начать обучение нажми стрелку вправо",  "Сейчас выйдет обычный клиент, твоя задача будет заключаться в том, чтобы обслужить его",
    "", "Теперь нажми в левом нижнем углу на стрелку вправо и перейди на другой экран", "Теперь нажми на кнопку \"Рецепты\" и посмотри, какие продукты требуются для приготовления заказа клиента",
    "Теперь нажми в левом нижнем углу на стрелку вправо и перейди на другой экран", "Теперь нажми на кнопку \"Магазин\" и купи нужные продукты, нажав напротив них кнопку \"Купить\"",
    "Теперь вернись на экран Кухни и приготовь нужное блюдо, нажав кнопку \"Готовить\" напротив него", "Теперь вернись на экран Таверны и отдай приготовленное блюдо, нажав кнопку \"Дать\" напротив него",
    "Поздравляю, обучение успешно пройдено!"};
    private int _messageIndex = 0;

    public int messageIndex {
        set { _messageIndex = value; }
        get { return _messageIndex;}
    }

    private void Start() {
        ChangeMessageText();
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
            if (_messageIndex == indexToSpawnFirstClient) {
                ShowOrHideButtons(false);
                queueCreating.SpawnNewGuest();
            } else if (_messageIndex == trainingMessages.Count) {
                SceneManager.LoadScene("MainMenu");
            } else {
                ChangeMessageText();
            }
        }
    }
}
