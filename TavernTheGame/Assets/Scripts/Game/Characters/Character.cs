using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    public enum Sex {
        Male, Female
    }
    public enum Type {
        Normal, Unfair
    }
    public enum Mood {
        Sad = -1, Happy = 1
    }
    [Header("Character's variables")]
    [SerializeField] protected Sex _characterGender = Sex.Male;
    [SerializeField] protected Type _characterType = Type.Normal;
    [SerializeField] protected Mood _characterReaction = Mood.Happy;
    [Header("Scripts")]
    [SerializeField] protected FoodOrdering foodOrdering;

    public Sex characterGender {
        get { return _characterGender; }
    }
    public Type characterType {
        get { return _characterType; }
    }
    public Mood characterReaction {
        get { return _characterReaction; }
    }

    public virtual void SayHello(List<string> list = null) {
        int rand = UnityEngine.Random.Range(0, list.Count);
        string messageTextStr = list[rand];
        Say(messageTextStr);
    }
    public virtual void MakeOrder() {
        foodOrdering.curOrder = foodOrdering.kitchen.GetDishByIndex(0);

        string orderPhrase = "Можно, пожалуйста, ^!";
        string messageTextStr = orderPhrase.Replace("^", "\"" + foodOrdering.curOrder.name + "\"");

        Action onComplete = PrepareOnCompleteAction();
        Say(messageTextStr, onComplete);
    }
    protected Action PrepareOnCompleteAction() {
        void ReadyToGiveOrder() => foodOrdering.isOrderTold = true; 
        void StartTimer() => foodOrdering.queueCreating.InvokeSetTimeIsUp();
        
        Action onComplete = null;
        onComplete += ReadyToGiveOrder;
        onComplete += StartTimer;
        if (foodOrdering.isAutomaticCookingBought && foodOrdering.tavern.GetNumberOfFoodInStorage(foodOrdering.curOrder.name) == 0) {
            void AutomaticDishCook() => foodOrdering.kitchen.AutomaticCookStart(foodOrdering.curOrder.name); 
            onComplete += AutomaticDishCook;
        }
        return onComplete;
    }
    protected void UpdateTavernBonus() {
        _characterReaction = (foodOrdering.curIssue.GetComponent<Food>().foodName == foodOrdering.curOrder.GetComponent<Food>().foodName) ? Mood.Happy : Mood.Sad;
        foodOrdering.tavern.tavernBonus += (int)_characterReaction;
    }
    public virtual bool React(List<string> goodReactList = null, List<string> badReactList = null) {
        int rand = 0;
        string messageTextStr = "Хорошо.";
        if (_characterReaction == Mood.Happy) {
            rand = UnityEngine.Random.Range(0, goodReactList.Count);
            messageTextStr = goodReactList[rand];
        } else if (_characterReaction == Mood.Sad) {
            rand = UnityEngine.Random.Range(0, badReactList.Count);
            messageTextStr = badReactList[rand];
        }
        Say(messageTextStr);
        return true;
    }
    public virtual void AnswerIfClientWasntServiced(List<string> list = null) {
        int rand = UnityEngine.Random.Range(0, list.Count);
        string messageTextStr = list[rand];
        Say(messageTextStr);
    }
    public virtual void Pay(float paymentModifier = 1f) {
        Food clientOrder = foodOrdering.curOrder.GetComponent<Food>();
        Food tavernDish = foodOrdering.curIssue.GetComponent<Food>();
        int rand = UnityEngine.Random.Range(0, 100), chanceToDoubleThePayment = 20;
        int tips = foodOrdering.tavern.tavernBonus, priceToPay = tavernDish.price;
        priceToPay = (foodOrdering.isDoublePayChanceBought && chanceToDoubleThePayment > rand) ? priceToPay*2 : priceToPay;
        //Костыль
        if (foodOrdering.isDoublePayChanceBought && chanceToDoubleThePayment > rand) { Debug.Log("Price is doubled " + priceToPay.ToString()); }
        float payment = Mathf.Round(priceToPay) + tips; 
        foodOrdering.tavern.tavernMoney += Mathf.Round((int)payment * paymentModifier);
    }
    protected void Say(string messageTextStr, Action onComplete = null) {
        foodOrdering.textWriter.CallMessageWriting(foodOrdering.message, foodOrdering.queueCreating.UpdateAllGenderRelatedWords(messageTextStr), 0.05f, onComplete);
    }

    private void Start() {
        if ( SceneManager.GetActiveScene().name != "Training" ) {
            InitiateCharacterVariables();
        }
    }

    private void InitiateCharacterVariables() {
        foodOrdering = FindObjectOfType<FoodOrdering>().GetComponent<FoodOrdering>();
        foodOrdering.InitiateServicingProcess();
    }

}
