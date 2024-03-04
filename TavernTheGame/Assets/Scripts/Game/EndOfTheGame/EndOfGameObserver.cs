using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndOfGameObserver : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private Tavern tavern;
    [SerializeField] private Shop shop;
    [SerializeField] private QueueCreating queue;
    [Header("End of the game variables")]
    [SerializeField] private GameObject endOfTheGameWindow;
    [SerializeField] private TMP_Text mainBody;
    [SerializeField] private TMP_Text finalCoinsNumber;

    private int numberOfCoinsToWin = 500;

    private string noMoneyMessage = "У вас нет больше средств, чтобы продолжать игру.";
    private string maxQueueCapacity;
    private string victoryMessage;

    private void Start() {
        victoryMessage = "Поздравляем вы достигли нужной суммы = " + numberOfCoinsToWin + " монет.";
    }
    private void FixedUpdate() {
        //Если игрок достигает условий окончания игры, то завершаем её с соответствующим результатом
        if (tavern.tavernMoney < shop.GetMinPrice() && tavern.IsFoodStorageEmpty()) {
            ShowEndOfTheGameMessage(noMoneyMessage);
            PlayerPrefs.SetString("Result", "Loss");
        } else if (tavern.tavernMoney > numberOfCoinsToWin) {
            ShowEndOfTheGameMessage(victoryMessage);
            PlayerPrefs.SetString("Result", "Victory");
        }
    }

    private void ShowEndOfTheGameMessage(string message) {
        endOfTheGameWindow.SetActive(true);
        mainBody.text = message;
        finalCoinsNumber.text = "Итоговое количество монет = " + tavern.tavernMoney;
    }

    public void LoadEndGameScene() {
        SceneManager.LoadScene("EndGameScene");
    }
}
