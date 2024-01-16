using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestMover : MonoBehaviour
{
    [SerializeField] private CharactersVariants variants;
    [SerializeField] private FoodOrdering foodOrdering;
    [SerializeField] private QueueCreating queueCreator;
    [SerializeField] private Tavern tavern;
    
    private Status charStatus;
    private bool timeIsUped = false;
    private float waitTime = 30f;
    private float guestYpos;
    private bool isTimeIsUpInvoked;

    public enum Status {
        Waiting,
        EventWasGenerated,
        Serviced,
        EventIsFinished,
        Left
    }

    private void Update() {
        if (charStatus == Status.Waiting) {
            if (!isTimeIsUpInvoked) {
                Invoke("SetTimeIsUp", waitTime);
                isTimeIsUpInvoked = true;
            }
        }
    }

    //Если время вышло, то для продолжения движения клиента обновляем переменные
    public void SetTimeIsUp() {
        if (charStatus == Status.Waiting) {
            foodOrdering.AnswerIfClientWasntServiced();
            tavern.ChangeTavernBonus(-1);
        }
    }

    public void CancelSTimeIsUpInvoke() {
        CancelInvoke("SetTimeIsUp");
    }

    public bool GetTimeIsUp() {
        return timeIsUped;
    }

    public void SetStatus(Status value) {
        charStatus = value;
    }

    public Status GetStatus() {
        return charStatus;
    }

    public void SetIsTimeIsUpInvoked(bool value) {
        isTimeIsUpInvoked = value;
    }
}
