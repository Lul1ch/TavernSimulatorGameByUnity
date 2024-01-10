using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodOrdering : MonoBehaviour
{
    [SerializeField] private CharactersVariants variants;
    [SerializeField] private GuestMover guestMover;
    [SerializeField] private QueueCreating queueCreator;
    [SerializeField] private Tavern tavern;
    [SerializeField] private Kitchen kitchen;

    private AudioSource audioPhrase;

    public GameObject curOrder = null;
    public GameObject curIssue = null;
    public bool isOrderTold;
    private bool eventWasGenerated;

    [Header("GuestMessage")]
    [SerializeField] private GameObject messageCloud;
    [SerializeField] private Text messageText;
    [SerializeField] private SpriteRenderer reactEmoji;
    [Header("EventManager")]
    [SerializeField] private EventGenerator events;

    private int rand = 0;
    private bool isReacted = false;

    public enum Mood {
        Sad = -1, Happy = 1
    }

    private void FixedUpdate() {
        //Если клиент дошёл до точки и он не сделал ещё заказ, то формируем заказ
        
        if (guestMover.GetStatus() == GuestMover.Status.Waiting && curOrder == null) {
            int randForEvent = Random.Range(0, 100);
            messageCloud.SetActive(true);
            if (randForEvent > 40) {
                curOrder = MakeOrder();
                //Формируем сообщение приветствия и заказа
                SayHello();
                isOrderTold = false;
                //После преветствия с задержкой вызываем функцию, в которой выводится сообщение с заказом
                Invoke("SayWhatYouWant", 3f);
            } else {
                guestMover.SetStatus(GuestMover.Status.EventWasGenerated);
                events.CreateAnEvent();
            }
        }
        
        if (curIssue != null && !isReacted && isOrderTold && guestMover.GetStatus() == GuestMover.Status.Waiting) {
            //Формируем реакцию клиента
            Mood guestReaction = React();
            //Формируем сообщение с ответной реакцией
            Answer(guestReaction);
            //Проводим плату за заказ
            Pay(guestReaction);
            isReacted = true;
            //После получения заказа, вызываем функцию, которая заставляет клиента двигаться дальше
            guestMover.SetStatus(GuestMover.Status.Serviced);
        }
    }

    private GameObject MakeOrder() {
        GameObject guestOrder = null;

        GameObject guestMover = queueCreator.GetCurGuest();
        Character charInfo = guestMover.GetComponent<Character>();

        rand = Random.Range(0, kitchen.GetKitchenDishesCount());
        guestOrder = kitchen.GetDishByIndex(rand);

        return guestOrder;
    }

    private Mood React() {
        Mood reaction = Mood.Sad;
        //Формируем реакцию клиента в зависимости от того совпадает ли выданный заказ с заказом клиента
        reaction = (curIssue == curOrder) ? Mood.Happy : Mood.Sad;
        return reaction;
    }

    private void Pay(Mood reaction) {
        Food clientOrder = curOrder.GetComponent<Food>();
        Food tavernDish = curIssue.GetComponent<Food>();
        int tips = (int)reaction*3;
        //Если качество заказа выше самого худшего, то вычисляем оплату по специальной формуле
        if (curIssue.GetComponent<Food>().foodQuality != Food.Quality.Awful) {
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
            messageText.text = variants.GoodReactPharases[rand];
        } else if (reaction == Mood.Sad) {
            rand = Random.Range(0, variants.BadReactPharases.Count);
            messageText.text = variants.BadReactPharases[rand];
        }
        //Проигрывание аудио дорожки реплики клиента
        audioPhrase.Play();
    }

    private void SayHello() {
        //Выводим приветственную фразу и обновляем интерфейс сообщения
        GameObject curCustomer = queueCreator.GetCurGuest();
        int rand = Random.Range(0, variants.HelloPhrases.Count);
        messageText.text = variants.HelloPhrases[rand];
        
        //Рандомим аудио дорожку реплики клиента
        audioPhrase = curCustomer.GetComponent<AudioSource>();
        rand = Random.Range(0, variants.SpeechSounds.Count);
        audioPhrase.clip = variants.SpeechSounds[rand];
        audioPhrase.Play();
    }

    private void SayWhatYouWant() {
        //Обновляем интерфейс сообщения
        int rand = Random.Range(0, variants.OrderPhrases.Count);
        messageText.text = variants.OrderPhrases[rand].Replace("^", curOrder.name);
        
        isOrderTold = true;
    }

    public void AnswerIfClientWasntServiced() {
        //Обновляем интерфейс сообщения
        int rand = Random.Range(0, variants.WasntServicedPhrases.Count);
        messageText.text = variants.WasntServicedPhrases[rand];

        audioPhrase.Play();
        guestMover.SetStatus(GuestMover.Status.Left);
    }

    public bool GetEventWasGenerated() {
        return eventWasGenerated;
    }

    public void ClearVariablesValues() {
        curOrder = null;
        curIssue = null;
        isReacted = false;
        eventWasGenerated = false;
    }
}
