using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfGameObserver : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private Tavern tavern;
    [SerializeField] private Shop shop;
    [SerializeField] private QueueCreating queue;

    private int maxGuestsInQueue = 15;
    private int numberOfCoinsToWin = 500;

    private void FixedUpdate() {
        //Если игрок достигает условий окончания игры, то завершаем её с соответствующим результатом
        if (tavern.GetTavernMoney() < shop.GetMinPrice() && tavern.IsFoodStorageEmpty()) {
            PlayerPrefs.SetString("Result", "Loss");
            queue.SetGuestCounter(0);
            Invoke("LoadEndGameScene", 5f);
        } else if (queue.GetGuestCounter() > maxGuestsInQueue) {
            PlayerPrefs.SetString("Result", "Loss");
            queue.SetGuestCounter(0);
            Invoke("LoadEndGameScene", 5f);
        } else if (tavern.GetTavernMoney() > numberOfCoinsToWin) {
            PlayerPrefs.SetString("Result", "Victory");
            queue.SetGuestCounter(0);
            Invoke("LoadEndGameScene", 5f);
        }
    }

    //Загружаем сцену окончания игры
    private void LoadEndGameScene() {
        SceneManager.LoadScene("EndGameScene");
    }
}
