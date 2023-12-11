using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfGameObserver : MonoBehaviour
{
    [SerializeField] private Tavern tavern;
    [SerializeField] private QueueCreating queue;

    private void FixedUpdate() {
        //Если игрок достигает условий окончания игры, то завершаем её с соответствующим результатом
        if (tavern.GetTavernMoney() < 2 && tavern.isFoodStorageEmpty()) {
            PlayerPrefs.SetString("Result", "Loss");
            queue.SetGuestCounter(0);
            Invoke("LoadEndGameScene", 5f);
        } else if (queue.GetGuestCounter() > 15) {
            PlayerPrefs.SetString("Result", "Loss");
            queue.SetGuestCounter(0);
            Invoke("LoadEndGameScene", 5f);
        } else if (tavern.GetTavernMoney() > 500) {
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
