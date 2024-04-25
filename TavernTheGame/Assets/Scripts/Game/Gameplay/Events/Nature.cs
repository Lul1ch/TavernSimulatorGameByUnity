using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nature : Event
{
    [Header("NatureVariables")]
    [SerializeField] private string userAgreedMessage;
    [SerializeField] private string userRejectedMessage;
    [SerializeField] private string doubtMessage;
    [SerializeField] private Shop shop;
    [SerializeField] private Tavern tavern;

    private static int _relations = 0;
    private int lowestRelationsBorder = -3;
    private int highestRelationsBorder = 5;
    public int relations { 
        get { return _relations; }
        set { if (value <= highestRelationsBorder && value >= lowestRelationsBorder) { _relations = value; } }
    }

    private void Start() {
        InitializeVariables();
        InvokeAnEvent();
    }

    protected override IEnumerator TriggerEventConsequences() {
        while (userAnswer == Answer.Empty) {
            yield return new WaitForSeconds(1f);
        }

        if (userAnswer == Event.Answer.Yes) {
            relations++;
            if (relations > 0) {
                ChangeMessageText(userAgreedMessage);
                for (int i = 0; i <= relations; i++) {
                    GameObject productObject = shop.GetRandomNatureFood();
                    Food product = productObject.GetComponent<Food>();

                    tavern.UpdateDictionary(product.foodName, tavern.foodContentParent, productObject);
                }
            } else {
                ChangeMessageText(doubtMessage);
            }
        } else if (userAnswer == Event.Answer.No) {
            relations--;
            ChangeMessageText(userRejectedMessage);
        }
        InvokeOnUserResponse();
    }

    private void InitializeVariables() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        shop = FindObjectOfType<Shop>().GetComponent<Shop>();
    }
}
