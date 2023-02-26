using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public GameObject shopWindow;
    public GameObject storageWindow;
    public GameObject kitchenWindow;

    public GameObject shopWindowContent;

    [Header("Objects")]
    [SerializeField] private GameObject messageCloud;

    [Header("Scripts")]
    [SerializeField] private Tavern tavern;
    [SerializeField] private Shop shop;
    [SerializeField] private FoodOrdering foodOrder;
    [SerializeField] private EventGenerator events;

    [Header("Sprites")]
    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;

    [Header("Buttons")]
    [SerializeField] private Button musicButton;

    [Header("Sounds")]
    [SerializeField] private AudioClip buySound;
    [SerializeField] private AudioClip giveSound;

    private bool isShopWindowActive = false;
    private bool isStorageWindowActive = false;
    private bool isKitchenWindowActive = false;
    private bool isMusicOn = true;
    private bool isMessageCloudActive = false;

    //Для трёх разных окон заводим функции для показа или скрытия их при нажатии на нужные кнопки
    public void showOrHideShopWindow() {
        isShopWindowActive = !isShopWindowActive;
        shopWindow.SetActive(isShopWindowActive);
    }

    public void showOrHideStorageWindow() {
        isStorageWindowActive = !isStorageWindowActive;
        storageWindow.SetActive(isStorageWindowActive);
    }

    public void showOrHideKitchenWindow() {
        RectTransform kitchenRT = kitchenWindow.GetComponent<RectTransform>();
        isKitchenWindowActive = !isKitchenWindowActive;
        if (isKitchenWindowActive) {
            moveWindow(kitchenRT, 0);
        } else {
            moveWindow(kitchenRT, 1500);
        }
    }

    public void showOrHideMessageCloud() {
        isMessageCloudActive = !isMessageCloudActive;
        messageCloud.SetActive(isMessageCloudActive);
    }

    private void moveWindow(RectTransform windowRT, float xCoord) {
        windowRT.offsetMin = new Vector2(xCoord, 0);
        windowRT.offsetMax = new Vector2(xCoord, 0);
    }

    public void BuyProduct(ProductInfo productInfo) {
        if (tavern.GetTavernMoney() >= productInfo.productPrice) {
            //Проводиться оплата за покупку выбранного товара
            tavern.DecreaseTavernMoney(productInfo.productPrice);
            //Добавление продукта на склад
            tavern.UpdateDict(productInfo.productName, shop.FoodStore[productInfo.productIndex]);
            //Визуальное обновление окна склада в игре
            tavern.UpdateStorageInfo(productInfo.productName, productInfo.productSprite);

            //Закостылил, чтоб работало, если что поменять
            PlayTheClip(buySound);
        }
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
            
            //Закостылил, чтоб работало, если что поменять
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

    public void ChangeMusicActivity(AudioSource music) {
        isMusicOn = !isMusicOn;
        music.enabled = isMusicOn;
        if (isMusicOn) {
            musicButton.GetComponent<Image>().sprite = musicOn;
        } else {
            musicButton.GetComponent<Image>().sprite = musicOff;
        }
    }

    public void PlayOnClickSound(AudioSource objectAudioSource) {
        objectAudioSource.Play();
    }

    public void PlayTheClip(AudioClip sound) {
        gameObject.GetComponent<AudioSource>().PlayOneShot(sound);
    }

}
