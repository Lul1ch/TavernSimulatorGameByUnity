using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodOrdering : MonoBehaviour
{
    [SerializeField] private CharactersVariants variants;
    [SerializeField] private GuestMover curGuest;
    [SerializeField] private Tavern tavern;

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

    private int rand = 0, randomize = 0;
    private bool isReacted = false;

    public enum Mood {
        Sad = -1, Happy = 1
    }

    private void FixedUpdate() {
        //Если клиент дошёл до точки и он не сделал ещё заказ, то формируем заказ
        
        if (curGuest.GetStatus() == GuestMover.Status.Waiting && curOrder == null) {
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
                curGuest.SetStatus(GuestMover.Status.EventWasGenerated);
                events.CreateAnEvent();
            }
        }
        
        if (curIssue != null && !isReacted && isOrderTold) {
            //Формируем реакцию клиента
            Mood guestReaction = React();
            //Формируем сообщение с ответной реакцией
            Answer(guestReaction);
            //Проводим плату за заказ
            Pay(guestReaction);
            isReacted = true;
            //После получения заказа, вызываем функцию, которая заставляет клиента двигаться дальше
            curGuest.SetStatus(GuestMover.Status.Serviced);
        }
    }

    private GameObject MakeOrder() {
        GameObject guestOrder = null;
        rand = Random.Range(0, 100);
        //С определённым шансом клиент заказывает алкоголь
        if (rand < 20) {
            rand = Random.Range(0, variants.AlcoholWarehouse.Count);
            guestOrder = variants.AlcoholWarehouse[rand];
            return guestOrder;
        }

        GameObject curGuest = variants.Characters[0];
        Character charInfo = curGuest.GetComponent<Character>();
        //В зависимости от предпочтений клиента формируем его заказ
        if (charInfo.charPrefs == Character.PreferencesLevel.Primal) {
            rand = Random.Range(0, variants.ComponentWarehouse.Count);
            guestOrder = variants.ComponentWarehouse[rand];
        } else if (charInfo.charPrefs == Character.PreferencesLevel.Normal) {
            rand = Random.Range(0, variants.SnackWarehouse.Count);
            guestOrder = variants.SnackWarehouse[rand];
        } else if (charInfo.charPrefs == Character.PreferencesLevel.Advanced) {
            rand = Random.Range(0, variants.SimpleDishWarehouse.Count);
            guestOrder = variants.SimpleDishWarehouse[rand];
        } else if (charInfo.charPrefs == Character.PreferencesLevel.Extended) {
            rand = Random.Range(0, variants.DishWarehouse.Count);
            guestOrder = variants.DishWarehouse[rand];
        }

        return guestOrder;
    }

    private Mood React() {
        Character curCharacter = variants.Characters[0].GetComponent<Character>();
        Mood reaction = Mood.Sad;
        randomize = Random.Range(0,100);
        //Формируем реакцию клиента в зависимости от того совпадает ли выданный заказ с заказом клиента
        if (curIssue == curOrder) {
            if (randomize > curCharacter.GetSatisfactionBorder()) {
                reaction = Mood.Happy;
            } else {
                reaction = Mood.Sad;
            }
        } else {
            if (randomize > curCharacter.GetSatisfactionBorder() + 20) {
                reaction = Mood.Happy;
            } else {
                reaction = Mood.Sad;
            } 
        }
        return reaction;
    }

    private void Pay(Mood reaction) {
        Food clientOrder = curOrder.GetComponent<Food>();
        Food tavernDish = curIssue.GetComponent<Food>();
        //Если качество заказа выше самого худшего, то вычисляем оплату по специальной формуле
        if (curIssue.GetComponent<Food>().foodQuality != Food.Quality.Awful) {
            float payment = Mathf.Round(clientOrder.price + (int)reaction*3 + ((int)tavernDish.foodQuality - (int)clientOrder.foodQuality) + curIssue.GetComponent<Food>().satiety/10) + tavern.GetTavernBonus(); 
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
            rand = Random.Range(0, variants.GoodEmojis.Count);
            reactEmoji.sprite = variants.GoodEmojis[rand];
        } else if (reaction == Mood.Sad) {
            rand = Random.Range(0, variants.BadReactPharases.Count);
            messageText.text = variants.BadReactPharases[rand];
            rand = Random.Range(0, variants.BadEmojis.Count);
            reactEmoji.sprite = variants.BadEmojis[rand];
        }
        //Проигрывание аудио дорожки реплики клиента
        audioPhrase.Play();
    }

    private void SayHello() {
        //Выводим приветственную фразу и обновляем интерфейс сообщения
        GameObject curCustomer = variants.Characters[0];
        int rand = Random.Range(0, variants.HelloPhrases.Count);
        messageText.text = variants.HelloPhrases[rand];
        
        rand = Random.Range(0, variants.CrazyEmojis.Count);
        reactEmoji.sprite = variants.CrazyEmojis[rand];

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
        rand = Random.Range(0, variants.CrazyEmojis.Count);
        reactEmoji.sprite = variants.CrazyEmojis[rand];
        
        isOrderTold = true;
    }

    public void AnswerIfClientWasntServiced() {
        //Обновляем интерфейс сообщения
        int rand = Random.Range(0, variants.WasntServicedPhrases.Count);
        messageText.text = variants.WasntServicedPhrases[rand];

        rand = Random.Range(0, variants.CrazyEmojis.Count);
        reactEmoji.sprite = variants.CrazyEmojis[rand];

        audioPhrase.Play();

        //Обнуляем заказ клиента и выданный пользователем
        curOrder = null;
        curIssue = null;
        isReacted = false;
        eventWasGenerated = false;
    }

    public bool GetEventWasGenerated() {
        return eventWasGenerated;
    }
}
