using System;
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
    [SerializeField] private CustomTextWriter textWriter;
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
            SayWhatYouWant();
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

        rand = UnityEngine.Random.Range(0, kitchen.GetKitchenDishesCount());
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
        int rand = UnityEngine.Random.Range(0, 100), chanceToDoubleThePayment = 20;
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
        string messageTextStr = "Хорошо.";
        if (reaction == Mood.Happy) {
            rand = UnityEngine.Random.Range(0, variants.GoodReactPharases.Count);
            messageTextStr = variants.GoodReactPharases[rand];
        } else if (reaction == Mood.Sad) {
            rand = UnityEngine.Random.Range(0, variants.BadReactPharases.Count);
            messageTextStr = variants.BadReactPharases[rand];
        }
        Say(messageTextStr);
    }

    private void SayHello() {
        GameObject curCustomer = queueCreator.GetCurGuest();
        int rand = UnityEngine.Random.Range(0, variants.HelloPhrases.Count);
        string messageTextStr = variants.HelloPhrases[rand];
        Say(messageTextStr);
        audioPhrase = curCustomer.GetComponent<AudioSource>();
        rand = UnityEngine.Random.Range(0, variants.SpeechSounds.Count);
        audioPhrase.clip = variants.SpeechSounds[rand];

    }

    public void SayWhatYouWant() {
        //Обновляем интерфейс сообщения
        int rand = UnityEngine.Random.Range(0, variants.OrderPhrases.Count);
        string messageTextStr = variants.OrderPhrases[rand].Replace("^", "\"" + _curOrder.name + "\"");

        void ReadyToGiveOrder() => _isOrderTold = true; 
        Action onComplete = null;
        onComplete += ReadyToGiveOrder;
        Say(messageTextStr, onComplete);
        //_isOrderTold = true;
        
        if (isAutomaticCookingBought && tavern.GetNumberOfFoodInStorage(_curOrder.name) == 0) { kitchen.AutomaticCookStart(_curOrder.name); }
        if ( SceneManager.GetActiveScene().name == "Training" ) {
            EventBus.onTrainGuestToldHisOrder?.Invoke();
            trainingManager.SaveMessage(trainingManager.indexToSaveClientOrder, messageTextStr);
        }
    }

    public void AnswerIfClientWasntServiced() {
        //Обновляем интерфейс сообщения
        int rand = UnityEngine.Random.Range(0, variants.WasntServicedPhrases.Count);
        string messageTextStr = variants.WasntServicedPhrases[rand];
        Say(messageTextStr);
    }

    public void ClearVariablesValues() {
        _curOrder = null;
        _curIssue = null;
        isReacted = false;
        _isOrderTold = false;
    }

    private void Say(string messageTextStr, Action onComplete = null) {
        textWriter.CallMessageWriting(_messageText, queueCreator.UpdateAllGenderRelatedWords(messageTextStr), 0.05f, onComplete);
    }
}
