using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    

    [Header("Scenes's objects")]
    
    [SerializeField] private  GameObject kitchenWindow;

    private bool isKitchenWindowActive = false;

    private float kitchenWindowYpos1 = 0f, kitchenWindowYpos2 = 1500f;

    //Для трёх разных окон заводим функции для показа или скрытия их при нажатии на нужные кнопки
    public void ShowOrHideWindow(GameObject window) {
        window.SetActive(!window.activeSelf);
    }

    public void HideAnotherWindow(GameObject window) {
        window.SetActive(false);
    }

    public void ShowOrHideKitchenWindow() {
        RectTransform kitchenRT = kitchenWindow.GetComponent<RectTransform>();
        isKitchenWindowActive = !isKitchenWindowActive;
        if (isKitchenWindowActive) {
            MoveWindow(kitchenRT, kitchenWindowYpos1);
        } else {
            MoveWindow(kitchenRT, kitchenWindowYpos2);
        }
    }

    public void HideKitchenWindowOnExit() {
        isKitchenWindowActive = true;
        ShowOrHideKitchenWindow();
    }

    private void MoveWindow(RectTransform windowRT, float yCoord) {
        windowRT.offsetMin = new Vector2(0f, yCoord);
        windowRT.offsetMax = new Vector2(0f, yCoord);
    }

    public void StartTheGame() {
        SceneManager.LoadScene("Game");
    }
    public void LoadTrainingScene() {
        SceneManager.LoadScene("Training");
    }

    //Когда игра закончена перезагружаем её, загружая сцену главного меню
    public void RestartTheGame() {
        SceneManager.LoadScene("MainMenu");
    }

    public static void PlayOnClickSound(AudioSource objectAudioSource) {
        if (objectAudioSource.enabled == false) {
            objectAudioSource.enabled = true;
        }
        objectAudioSource.Play();
    }

    public void ShowAnObject(GameObject gameObj) {
        gameObj.SetActive(true);
    }   

    public void HideAnObject(GameObject gameObj) {
        gameObj.SetActive(false);
    }

}
