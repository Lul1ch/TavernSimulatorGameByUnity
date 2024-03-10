using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FoodOrdering : MonoBehaviour
{
    [SerializeField] private CharactersVariants variants;
    [SerializeField] private QueueCreating queueCreator;
    [SerializeField] private Tavern tavern;
    [SerializeField] private Kitchen kitchen;
    [SerializeField] private TrainingManager trainingManager;
    private AudioSource audioPhrase;

    private GameObject _curOrder = null;
    private GameObject _curIssue = null;
    private bool _isOrderTold, _isDoublePayChanceBought, _isAutomaticCookingBought;

    public GameObject curOrder {
        get { return _curOrder; }
        set { _curOrder = value; }
    }
    public GameObject curIssue {
        get { return _curIssue; }
        set { _curIssue = value; }
    }
    public bool isOrderTold {
        get { return _isOrderTold; }
        set { _isOrderTold = value; }
    }
    public bool isDoublePayChanceBought {
        get { return _isDoublePayChanceBought; }
        set { _isDoublePayChanceBought = value; }
    }
    public bool isAutomaticCookingBought {
        get { return _isAutomaticCookingBought; }
        set { _isAutomaticCookingBought = value; }
    }

    [Header("GuestMessage")]
    [SerializeField] private GameObject messageCloud;
    [SerializeField] private Text _messageText;

    private string messageText {
        set { _messageText.text = queueCreator.UpdateAllGenderRelatedWords(value); }
        get { return _messageText.text; }
    }

    private int rand = 0;
    private bool isReacted = false;

    public enum Mood {
        Sad = -1, Happy = 1
    }

    public void InitiateServicingProcess() {
        if (queueCreator.charStatus == QueueCreating.Status.Waiting && _curOrder == null) {
            MakeOrder();
            SayHello();
            _isOrderTold = false;
            Invoke("SayWhatYouWant", 2.5f);
        }
    }

    public void EndServicingProcess() {
        if (_curIssue != null && !isReacted && _isOrderTold && queueCreator.charStatus == QueueCreating.Status.Waiting) {
            Mood guestReaction = React();
            Answer(guestReaction);
            Pay(guestReaction);
            isReacted = true;
            queueCreator.charStatus = QueueCreating.Status.Serviced;
            EventBus.onGuestReacted?.Invoke();
        }
    }

    private void MakeOrder() {
        if ( SceneManager.GetActiveScene().name == "Training" ) {
            trainingManager.ShowOrHideButtons(false);
        }

        rand = Random.Range(0, kitchen.GetKitchenDishesCount());
        curOrder = kitchen.GetDishByIndex(rand);
    }

    private Mood React() {
        Mood reaction = Mood.Sad;
        //Формируем реакцию клиента в зависимости от того совпадает ли выданный заказ с заказом клиента
        reaction = (_curIssue.GetComponent<Food>().foodName == _curOrder.GetComponent<Food>().foodName) ? Mood.Happy : Mood.Sad;
        return reaction;
    }

    private void Pay(Mood reaction) {
        Food clientOrder = _curOrder.GetComponent<Food>();
        Food tavernDish = _curIssue.GetComponent<Food>();
        int rand = Random.Range(0, 100), chanceToDoubleThePayment = 20;
        int tips = tavern.tavernBonus, priceToPay = tavernDish.price;
        priceToPay = (isDoublePayChanceBought && chanceToDoubleThePayment > rand) ? priceToPay*2 : priceToPay;
        //Костыль
        if (isDoublePayChanceBought && chanceToDoubleThePayment > rand) { Debug.Log("Price is doubled " + priceToPay.ToString()); }
        if (_curIssue.GetComponent<Food>().foodQuality != Food.Quality.Awful) {
            float payment = Mathf.Round(priceToPay) + tips; 
            tavern.tavernMoney += (int)payment;
        }
        tavern.tavernBonus += (int)reaction*tavern.bonusesValueModifier;
    }

    private void Answer(Mood reaction) {
        int rand = 0;
        //В зависимости от реакции клиента обновляем сообщение с реакцией клиента
        if (reaction == Mood.Happy) {
            rand = Random.Range(0, variants.GoodReactPharases.Count);
            messageText = variants.GoodReactPharases[rand];
        } else if (reaction == Mood.Sad) {
            rand = Random.Range(0, variants.BadReactPharases.Count);
            messageText = variants.BadReactPharases[rand];
        }
        //Проигрывание аудио дорожки реплики клиента
        audioPhrase.Play();
    }

    private void SayHello() {
        GameObject curCustomer = queueCreator.GetCurGuest();
        int rand = Random.Range(0, variants.HelloPhrases.Count);
        messageText = variants.HelloPhrases[rand];
        
        audioPhrase = curCustomer.GetComponent<AudioSource>();
        rand = Random.Range(0, variants.SpeechSounds.Count);
        audioPhrase.clip = variants.SpeechSounds[rand];
        audioPhrase.Play();
    }

    public void SayWhatYouWant() {
        //Обновляем интерфейс сообщения
        int rand = Random.Range(0, variants.OrderPhrases.Count);
        messageText = variants.OrderPhrases[rand].Replace("^", "\"" + _curOrder.name + "\"");
        if (isAutomaticCookingBought && tavern.GetNumberOfFoodInStorage(_curOrder.name) == 0) { kitchen.AutomaticCookStart(_curOrder.name); }
        _isOrderTold = true;
        if ( SceneManager.GetActiveScene().name == "Training" ) {
            EventBus.onTrainGuestToldHisOrder?.Invoke();
            trainingManager.SaveMessage(trainingManager.indexToSaveClientOrder, messageText);
        }
    }

    public void AnswerIfClientWasntServiced() {
        //Обновляем интерфейс сообщения
        int rand = Random.Range(0, variants.WasntServicedPhrases.Count);
        messageText = variants.WasntServicedPhrases[rand];

        audioPhrase.Play();
    }

    public void ClearVariablesValues() {
        _curOrder = null;
        _curIssue = null;
        isReacted = false;
    }
}
