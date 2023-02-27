using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestMover : MonoBehaviour
{
    [SerializeField] private CharactersVariants variants;
    
    public Status charStatus;
    public bool timeIsUped = false;
    [SerializeField] private QueueCreating queueCreator;
    private float vSpeed = 35f, hSpeed = 125f;
    private float waitTime = 15f;
    private float guestYpos;
    private bool isReachedWaitPoint;
    private bool isTimeIsUpInvoked;

    public GameObject exitPoint;
    public GameObject waitPoint;
    public GameObject spawnPoint;

    public enum Status {
        Spawned,
        Moving,
        Waiting,
        Out
    }

    private void FixedUpdate() {
        if (variants.Characters.Count == 0) {
            queueCreator.CreateGuestObject();
        }
        //Постоянно обновляем статус текущего клиента
        ChangeGuestStatus();
        //Двигаем персонажа до момента, когда он достигнет точки ожидания
        if (charStatus == Status.Spawned || charStatus == Status.Moving){
            Move();
        } else if (charStatus == Status.Waiting) {
            //Немного меняем анимацию клиента в состоянии ожидания
            Wait();
            //Чтобы клиент продолжил движение запускаем отложенную функцию, которая позволит ему это сделать после заданного времени
            if (!isTimeIsUpInvoked) {
                Invoke("TimeIsUp", waitTime);
                isTimeIsUpInvoked = true;
            }
        }
    }

    private void ChangeGuestStatus() {
        float guestXpos = variants.Characters[0].transform.position.x;
        //Устанавливаем статус ожидания, когда клиент достиг нужной позиции
        if (isReachedWaitPoint){
            charStatus = Status.Waiting;
        } else if (guestXpos < waitPoint.transform.position.x) {
            //Если гость ещё не достиг точки ожидания, то устанавливаем ему статус движения
            if (guestXpos > spawnPoint.transform.position.x){
                charStatus = Status.Moving;
            } else {
                guestYpos = variants.Characters[0].transform.position.y;
            }
        } else if (guestXpos > waitPoint.transform.position.x) {
            //Проверяем, что гость впервые достиг точки ожидания для корректной работы игры
            if (!timeIsUped){
                isReachedWaitPoint = true;
            }
            //После окончания ожидания, если клиент пересёк точку ожидания, но ещё не достиг точки выхода, то устанавливаем ему статус движения
            if (guestXpos < exitPoint.transform.position.x){
                charStatus = Status.Moving;
            //В противном случае статус покидания таверны
            } else {
                charStatus = Status.Out;
                timeIsUped = false;
                isTimeIsUpInvoked = false;
                CancelInvoke();
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

    private void Move() {
        //Берём нужные параметры гостя
        Rigidbody2D guestRb = variants.Characters[0].GetComponent<Rigidbody2D>();
        Transform guestPos = variants.Characters[0].GetComponent<Transform>();
        
        //Задаём движение клиента слева направо (от Входа к "Выходу") 
        guestRb.velocity = transform.TransformDirection(new Vector2(hSpeed * Time.fixedDeltaTime, guestRb.velocity.y));
        //Задаём "Покачивание" для имитации шага и динамики в целом
        if (guestPos.localPosition.y <= guestYpos - 0.5f){
            guestRb.velocity = transform.TransformDirection(new Vector2(guestRb.velocity.x, vSpeed * Time.fixedDeltaTime));
        } else if (guestPos.localPosition.y >= guestYpos || guestRb.velocity.y <= 0){
            guestRb.velocity = transform.TransformDirection(new Vector2(guestRb.velocity.x, -1*vSpeed * Time.fixedDeltaTime));
        }
    }

    //Если время вышло, то для продолжения движения клиента обновляем переменные
    public void TimeIsUp() {
        if (charStatus == Status.Waiting) {   
            isReachedWaitPoint = false;
            timeIsUped = true;
        }
    }

}
