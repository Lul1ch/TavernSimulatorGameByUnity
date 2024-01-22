using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodOrdering : MonoBehaviour
{
    [SerializeField] private CharactersVariants variants;
    [SerializeField] private QueueCreating queueCreator;
    [SerializeField] private Tavern tavern;
    [SerializeField] private Kitchen kitchen;

    private AudioSource audioPhrase;
    private bool eventWasGenerated;
    private int eventIntiationBorder = 60, maxEventInitiationBorder = 90, eventIntiationBorderReductionStep = 10;

    private GameObject _curOrder = null;
    private GameObject _curIssue = null;
    private bool _isOrderTold;

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

    [Header("GuestMessage")]
    [SerializeField] private GameObject messageCloud;
    [SerializeField] private Text _messageText;

    [Header("EventManager")]
    [SerializeField] private EventGenerator events;

    private string messageText {
        set { _messageText.text = UpdateAllGenderRelatedWords(value); }
    }

    private int rand = 0;
    private bool isReacted = false;

    public enum Mood {
        Sad = -1, Happy = 1
    }

    private void FixedUpdate() {
        //Если клиент дошёл до точки и он не сделал ещё заказ, то формируем заказ
        Debug.Log((queueCreator.charStatus == QueueCreating.Status.Waiting).ToString() + " " + queueCreator.charStatus);
        if (queueCreator.charStatus == QueueCreating.Status.Waiting && _curOrder == null) {
            int randForEvent = Random.Range(0, 100);
            messageCloud.SetActive(true);
            if (randForEvent < eventIntiationBorder) {
                if (eventIntiationBorder > Mathf.Round(maxEventInitiationBorder / 2)) {
                    eventIntiationBorder -= eventIntiationBorderReductionStep;
                }
                _curOrder = MakeOrder();
                //Формируем сообщение приветствия и заказа
                SayHello();
                _isOrderTold = false;
                //После преветствия с задержкой вызываем функцию, в которой выводится сообщение с заказом
                Invoke("SayWhatYouWant", 3f);
            } else {
                queueCreator.charStatus = QueueCreating.Status.EventWasGenerated;
                events.CreateAnEvent();
                eventIntiationBorder = maxEventInitiationBorder;
            }
        }
        
        if (_curIssue != null && !isReacted && _isOrderTold && queueCreator.charStatus == QueueCreating.Status.Waiting) {
            //Формируем реакцию клиента
            Mood guestReaction = React();
            //Формируем сообщение с ответной реакцией
            Answer(guestReaction);
            //Проводим плату за заказ
            Pay(guestReaction);
            isReacted = true;
            //После получения заказа, вызываем функцию, которая заставляет клиента двигаться дальше
            queueCreator.charStatus = QueueCreating.Status.Serviced;
        }
    }

    private GameObject MakeOrder() {
        GameObject guestOrder = null;

        Character charInfo = queueCreator.GetCurGuest().GetComponent<Character>();

        rand = Random.Range(0, kitchen.GetKitchenDishesCount());
        guestOrder = kitchen.GetDishByIndex(rand);

        return guestOrder;
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
        int tips = (int)reaction*3;
        //Если качество заказа выше самого худшего, то вычисляем оплату по специальной формуле
        if (_curIssue.GetComponent<Food>().foodQuality != Food.Quality.Awful) {
            float payment = Mathf.Round(clientOrder.price + tips) + tavern.GetTavernBonus(); 
            tavern.IncreaseTavernMoney((int)payment);
        }
        tavern.ChangeTavernBonus((int)reaction);
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
        //Выводим приветственную фразу и обновляем интерфейс сообщения
        GameObject curCustomer = queueCreator.GetCurGuest();
        int rand = Random.Range(0, variants.HelloPhrases.Count);
        messageText = variants.HelloPhrases[rand];
        
        //Рандомим аудио дорожку реплики клиента
        audioPhrase = curCustomer.GetComponent<AudioSource>();
        rand = Random.Range(0, variants.SpeechSounds.Count);
        audioPhrase.clip = variants.SpeechSounds[rand];
        audioPhrase.Play();
    }

    private void SayWhatYouWant() {
        //Обновляем интерфейс сообщения
        int rand = Random.Range(0, variants.OrderPhrases.Count);
        messageText = variants.OrderPhrases[rand].Replace("^", _curOrder.name);
        
        _isOrderTold = true;
    }

    public void AnswerIfClientWasntServiced() {
        //Обновляем интерфейс сообщения
        int rand = Random.Range(0, variants.WasntServicedPhrases.Count);
        messageText = variants.WasntServicedPhrases[rand];

        audioPhrase.Play();
    }

    public bool GetEventWasGenerated() {
        return eventWasGenerated;
    }

    public void ClearVariablesValues() {
        _curOrder = null;
        _curIssue = null;
        isReacted = false;
        eventWasGenerated = false;
    }

    private string UpdateAllGenderRelatedWords(string str) {
        Character.Sex curGuestGender = queueCreator.GetCurGuest().GetComponent<Character>().characterGender;
        return str = (curGuestGender == Character.Sex.Male) ? str.Replace("(а)", "") : str.Replace("(а)", "а");
    }
}
