using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FoodOrdering : MonoBehaviour
{
    [SerializeField] private CharactersVariants _variants;
    [SerializeField] private QueueCreating _queueCreator;
    [SerializeField] private Tavern _tavern;
    [SerializeField] private Kitchen _kitchen;
    [SerializeField] private Shop shop;
    [SerializeField] private TrainingManager trainingManager;
    [SerializeField] private CustomTextWriter _textWriter;
    [SerializeField] private Hint tavernHint;
    [Header("ForUnfairGuests")]
    [SerializeField] private int chanceNotToPay = 55;
    [SerializeField] private int chanceToPayHalfOfPrice = 90;

    public CharactersVariants variants {
        get { return _variants; }
    }
    public QueueCreating queueCreating {
        get { return _queueCreator; }
    }
    public Tavern tavern {
        get { return _tavern; }
    }
    public Kitchen kitchen {
        get { return _kitchen; }
    }
    public CustomTextWriter textWriter {
        get { return _textWriter; }
    }

    private GameObject _curOrder = null;
    private GameObject _curIssue = null;
    private Character _curGuestScript = null;
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

    public Text message {
        get { return _messageText; }
    }
    public string messageText {
        set { _messageText.text = queueCreating.UpdateAllGenderRelatedWords(value); }
        get { return _messageText.text; }
    }


    public enum Mood {
        Sad = -1, Happy = 1
    }

    public void InitiateServicingProcess() {
        _curGuestScript = queueCreating.curGuest.GetComponent<Character>();
        if (queueCreating.charStatus == QueueCreating.Status.Waiting && _curOrder == null) {
            _curGuestScript.SayHello();
            _curGuestScript.MakeOrder();
        }
    }

    public void EndServicingProcess() {
        if (_curIssue != null && _isOrderTold && queueCreating.charStatus == QueueCreating.Status.Waiting) {
            _curGuestScript.React();
            _curGuestScript.Pay();
            queueCreating.charStatus = QueueCreating.Status.Serviced;
            EventBus.onGuestReacted?.Invoke();
        }
    }

    private void MakeOrder() {
        if ( SceneManager.GetActiveScene().name == "Training" ) {
            trainingManager.ShowOrHideButtons(false);
        }

        if (queueCreating.isUnfairGuestSpawned) {
            curOrder = shop.GetRandomAlcohol();
            return;
        }
        curOrder = kitchen.GetDishByIndex(UnityEngine.Random.Range(0, kitchen.GetKitchenDishesCount()));
    }

    private Mood React() {
        Mood reaction = Mood.Sad;
        //Формируем реакцию клиента в зависимости от того совпадает ли выданный заказ с заказом клиента
        reaction = (_curIssue.GetComponent<Food>().foodName == _curOrder.GetComponent<Food>().foodName) ? Mood.Happy : Mood.Sad;
        return reaction;
    }

    private void Pay(Mood reaction) {
        float paymentModifier = 1;
        if (queueCreating.isUnfairGuestSpawned) {
            int randForUnfairGuest = UnityEngine.Random.Range(0, 100);
            if (randForUnfairGuest < chanceNotToPay) {
                tavernHint.ShowHint(Hint.EventType.isUnfairGuestGone);
                EventBus.onGuestLeft?.Invoke();
                return;
            } else if ( randForUnfairGuest < chanceToPayHalfOfPrice) {
                paymentModifier = 0.5f;
            }
        }
        Food clientOrder = _curOrder.GetComponent<Food>();
        Food tavernDish = _curIssue.GetComponent<Food>();
        int rand = UnityEngine.Random.Range(0, 100), chanceToDoubleThePayment = 20;
        int tips = tavern.tavernBonus, priceToPay = tavernDish.price;
        priceToPay = (isDoublePayChanceBought && chanceToDoubleThePayment > rand) ? priceToPay*2 : priceToPay;
        //Костыль
        if (isDoublePayChanceBought && chanceToDoubleThePayment > rand) { Debug.Log("Price is doubled " + priceToPay.ToString()); }
        if (_curIssue.GetComponent<Food>().foodQuality != Food.Quality.Awful) {
            float payment = Mathf.Round(priceToPay) + tips; 
            tavern.tavernMoney += Mathf.Round((int)payment * paymentModifier);
        }
        tavern.tavernBonus += (int)reaction;
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
        GameObject curCustomer = queueCreating.curGuest;
        int rand = UnityEngine.Random.Range(0, variants.HelloPhrases.Count);
        string messageTextStr = variants.HelloPhrases[rand];
        Say(messageTextStr);
    }

    public void SayWhatYouWant() {
        //Обновляем интерфейс сообщения
        int rand = UnityEngine.Random.Range(0, variants.OrderPhrases.Count);
        string messageTextStr = variants.OrderPhrases[rand].Replace("^", "\"" + _curOrder.name + "\"");

        void ReadyToGiveOrder() => _isOrderTold = true; 
        void StartTimer() => queueCreating.InvokeSetTimeIsUp();
        
        Action onComplete = null;
        onComplete += ReadyToGiveOrder;
        onComplete += StartTimer;
        if (isAutomaticCookingBought && tavern.GetNumberOfFoodInStorage(_curOrder.name) == 0) {
            void AutomaticDishCook() => kitchen.AutomaticCookStart(_curOrder.name); 
            onComplete += AutomaticDishCook;
        }
        Say(messageTextStr, onComplete);
        
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
        _isOrderTold = false;
    }

    private void Say(string messageTextStr, Action onComplete = null) {
        textWriter.CallMessageWriting(_messageText, queueCreating.UpdateAllGenderRelatedWords(messageTextStr), 0.05f, onComplete);
    }
}
