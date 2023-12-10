using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestMover : MonoBehaviour
{
    [SerializeField] private CharactersVariants variants;
    [SerializeField] private FoodOrdering foodOrdering;
    
    private Status charStatus;
    private bool timeIsUped = false;
    [SerializeField] private QueueCreating queueCreator;
    private float vSpeed = 35f;
    private float waitTime = 30f;
    private float guestYpos;
    private bool isTimeIsUpInvoked;

    public enum Status {
        Waiting,
        EventWasGenerated,
        Serviced,
        EventIsFinished
    }

    private void Update() {
        if (charStatus == Status.Waiting) {
            
            //Немного меняем анимацию клиента в состоянии ожидания
            Wait();
            //Чтобы клиент продолжил движение запускаем отложенную функцию, которая позволит ему это сделать после заданного времени
            if (!isTimeIsUpInvoked) {
                Debug.Log("In updater");
                Invoke("SetTimeIsUp", waitTime);
                isTimeIsUpInvoked = true;
            }
        }
    }

    private void Wait() {
        //Берём нужные параметры гостя
        Rigidbody2D guestRb = variants.Characters[0].GetComponent<Rigidbody2D>();
        Transform guestPos = variants.Characters[0].GetComponent<Transform>();

        //Убираем движение и задаём вертикаьные покачивания, как имитацию анимации ожидания
        guestRb.velocity = transform.TransformDirection(new Vector2(0, guestRb.velocity.y));
        if (guestPos.position.y <= guestYpos - 0.5f){
            guestRb.velocity = transform.TransformDirection(new Vector2(guestRb.velocity.x, vSpeed*0.8f * Time.fixedDeltaTime));
        } else if (guestPos.position.y >= guestYpos){
            guestRb.velocity = transform.TransformDirection(new Vector2(guestRb.velocity.x, -0.8f*vSpeed * Time.fixedDeltaTime));
        }
    }

    //Если время вышло, то для продолжения движения клиента обновляем переменные
    public void SetTimeIsUp() {
        if (charStatus == Status.Waiting) {
            foodOrdering.AnswerIfClientWasntServiced();
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
}
