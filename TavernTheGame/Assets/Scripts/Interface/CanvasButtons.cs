using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;

    [Header("Scenes's objects")]
    [SerializeField] private Button musicButton;
    [SerializeField] private  GameObject kitchenWindow;

    private bool isKitchenWindowActive = false;
    private bool isMusicOn = true;

    //Для трёх разных окон заводим функции для показа или скрытия их при нажатии на нужные кнопки
    public void showOrHideWindow(GameObject window) {
        window.SetActive(!window.activeSelf);
    }

    public void showOrHideKitchenWindow() {
        RectTransform kitchenRT = kitchenWindow.GetComponent<RectTransform>();
        isKitchenWindowActive = !isKitchenWindowActive;
        if (isKitchenWindowActive) {
            moveWindow(kitchenRT, 0);
        } else {
            moveWindow(kitchenRT, 1500f);
        }
    }

    private void moveWindow(RectTransform windowRT, float yCoord) {
        windowRT.offsetMin = new Vector2(0f, yCoord);
        windowRT.offsetMax = new Vector2(0f, yCoord);
    }

    public void GiveCustomerAnOrder(ProductInfo productInfo) {
        string foodName = productInfo.productName;
        //Если от клиента есть заказ, он озвучен, также заказ не был ещё выдан и выбранный продукт присутствует на складе(его количество больше 0), то мы выдаём заказ
        if (foodOrder.curOrder != null && tavern.isNumberGreaterThanZero(foodName) && foodOrder.curIssue == null && foodOrder.isOrderTold || events.isItAFreeFoodEvent()) {
            //Уставливаем выданный заказ
            foodOrder.curIssue = tavern.GetFoodObject(foodName);
            //Убавляем количество выданного продукта на складе
            tavern.ReduceFoodNumber(foodName);
            //Визуальное обновление окна склада в игре
            tavern.UpdateStorageInfo(foodName);
            
            PlayTheClip(giveSound);
        } 
    }

    //Загружаем сцену игры, когда нажимаем на кнопку "играть"
    public void StartTheGame() {
        SceneManager.LoadScene("Game");
    }

    //Когда игра закончена перезагружаем её, загружая сцену главного меню
    public void RestartTheGame() {
        SceneManager.LoadScene("MainMenu");
    }

    public void ChangeMusicActivity() {
        isMusicOn = !isMusicOn;
        AudioListener.volume = (isMusicOn) ? 1f : 0f;
        musicButton.GetComponent<Image>().sprite = (isMusicOn) ? musicOn : musicOff;
    }

    public static void PlayOnClickSound(AudioSource objectAudioSource) {
        objectAudioSource.Play();
    }

    public void ShowAnObject(GameObject gameObj) {
        gameObj.SetActive(true);
    }   

    public void HideAnObject(GameObject gameObj) {
        gameObj.SetActive(false);
    }

}
