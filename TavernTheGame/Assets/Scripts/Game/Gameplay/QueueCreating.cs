using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QueueCreating : MonoBehaviour
{
    [SerializeField] private CharactersVariants variants;
    [SerializeField] private Transform spawnPointTransform;
    [SerializeField] private FoodOrdering foodOrdering;
    [SerializeField] private Tavern tavern;
    [SerializeField] private GameEventsManager gameEventsManager;
    [SerializeField] private int timeBeforeDestroyingLeftGuest = 0;
    [SerializeField] private Text destroyLeftGuestTimerText, timeBeforeClientLeaveText;
    [Header("Training")]
    [SerializeField] private GameObject _orderClient;

    private GameObject curGuest;
    private Vector3 spawnPoint = new Vector3(0, 0, 0);
    private Status _charStatus = Status.NotSpawned;
    private float waitTime = 45f;
    private int _eventIntiationBorder = 40, maxEventInitiationBorder = 90, eventIntiationBorderReductionStep = 10;
    private bool _isEventsReadyToCreate;
    //Время, начиная с которого, показывается таймер отсчёта до ухода текущего клиента
    private int timeToShowDestroyTimer = 0;
    private IEnumerator leftClientDestroyCoroutine, clientLeaveCoroutine;
    private bool _gameIsNotEnd = true;

    public enum Status {
        NotSpawned,
        Waiting,
        EventWasGenerated,
        Serviced,
        EventIsFinished,
        Left
    }   

    public Status charStatus {
        set { _charStatus = value; }
        get { return _charStatus; }
    }
    public GameObject orderClient {
        get { return _orderClient; }
    }
    public bool isEventsReadyToCreate {
        get { return _isEventsReadyToCreate; }
        set { _isEventsReadyToCreate = value; }
    }
    public bool gameIsNotEnd {
        get { return _gameIsNotEnd; }
        set { _gameIsNotEnd = value; }
    }

    private void Start() {
        //Заводим куратину на создание нового гостя через определённый временной промежуток
        InitSpawnPoint();
        SpawnNewGuest();
        InitTimeToShowDestroyTimer();
    }

    public void SpawnNewGuest() {
        if (!gameIsNotEnd) {
            return;
        }
        if (leftClientDestroyCoroutine != null) {
            StopCoroutine(leftClientDestroyCoroutine);
        }
        int randForEvent = Random.Range(0, 100);
        int randForGuest = Random.Range(0, variants.CharactersSkins.Count);

        GameObject guestToInstaniate = variants.CharactersSkins[randForGuest];
        float xOffset = 0;
        float yOffset = 0;
        if ( randForEvent > _eventIntiationBorder && _isEventsReadyToCreate ) {
            guestToInstaniate = gameEventsManager.GetRandomEventGuest();
            _charStatus = Status.EventWasGenerated;
            _eventIntiationBorder = maxEventInitiationBorder;
            xOffset = guestToInstaniate.GetComponent<Event>().xOffset;
            yOffset = guestToInstaniate.GetComponent<Event>().yOffset;
        } else {
            if (_eventIntiationBorder > Mathf.Round(maxEventInitiationBorder / 2)) {
                _eventIntiationBorder -= eventIntiationBorderReductionStep;
            }
            _charStatus = Status.Waiting;
            xOffset = guestToInstaniate.GetComponent<Character>().xOffset;
            yOffset = guestToInstaniate.GetComponent<Character>().yOffset;
        }
        Debug.Log(spawnPoint);
        float xCoord = spawnPoint.x; xCoord += xOffset;
        float yCoord = spawnPoint.y; yCoord += yOffset;
        float zCoord = spawnPoint.z;
        Vector3 vec = new Vector3(xCoord, yCoord, zCoord);
        curGuest = Instantiate(guestToInstaniate, vec, Quaternion.identity);
        //curGuest = Instantiate(guestToInstaniate, new Vector3(xCoord + xOffset, yCoord + yOffset, zCoord), Quaternion.identity);
        foodOrdering.InitiateServicingProcess();
        if ( SceneManager.GetActiveScene().name != "Training" ) {
            EventBus.onGuestSpawned?.Invoke();
        }
    }

    public void DestroyServicedGuest() {
        destroyLeftGuestTimerText.gameObject.SetActive(false);
        Destroy(curGuest);
        SpawnNewGuest();
    }

    public GameObject GetCurGuest() {
        return curGuest;
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint) {
        spawnPoint = newSpawnPoint;
    }

    public void InvokeSetTimeIsUp() {
        if (_charStatus == Status.Waiting) {
            clientLeaveCoroutine = SetTimeIsUp();
            StartCoroutine(clientLeaveCoroutine);
        }
    }
    private IEnumerator SetTimeIsUp() {
        int counter = (int)waitTime;
        ShowTimeBeforeClientLeaveText();
        while (counter >= 0) {
            timeBeforeClientLeaveText.text = "Время до ухода: " + counter;
            if (counter <= 10) {
                timeBeforeClientLeaveText.text = "<color=red>" + timeBeforeClientLeaveText.text + "</color>";
            }
            counter--;
            yield return new WaitForSeconds(1f);
        }
        foodOrdering.AnswerIfClientWasntServiced();
        tavern.tavernBonus -= 1;
        _charStatus = Status.Left;
        EventBus.onGuestReacted?.Invoke();
    }

    public void CancelSTimeIsUpInvoke() {
        if (clientLeaveCoroutine != null) {
            StopCoroutine(clientLeaveCoroutine);
        }
    }

    public void InvokeDeferredClientDestroy() {
        leftClientDestroyCoroutine = DeferredClientDestroyCoroutine();
        StartCoroutine(leftClientDestroyCoroutine);
    }

    private IEnumerator DeferredClientDestroyCoroutine() {
        int counter = timeBeforeDestroyingLeftGuest;
        while (counter > 0) {
            counter--;
            if (counter == timeToShowDestroyTimer) {
                destroyLeftGuestTimerText.gameObject.SetActive(true);
            } 
            destroyLeftGuestTimerText.text = counter.ToString();
            yield return new WaitForSeconds(1f);
        }
        DestroyServicedGuest();
    }

    public string UpdateAllGenderRelatedWords(string str) {
        Character.Sex curGuestGender = curGuest.GetComponent<Character>().characterGender;
        return str = (curGuestGender == Character.Sex.Male) ? str.Replace("(а)", "") : str.Replace("(а)", "а");
    }

    public void SpawnCertainClient(GameObject client) {
        curGuest = Instantiate(client, spawnPoint, Quaternion.identity);
    }

    public void InitSpawnPoint() {
        spawnPoint = new Vector3(spawnPointTransform.position.x, spawnPointTransform.position.y, 0);
    }

    public void SetPlayerAnswer(Event.Answer answer) {
        if (_charStatus == Status.EventWasGenerated) {
            curGuest.GetComponent<Event>().userAnswer = answer;
        }
    }
    public void PlayerAnsweredYes() {
        if (_charStatus == Status.EventWasGenerated) {
            curGuest.GetComponent<Event>().userAnswer = Event.Answer.Yes;
        }
    }
    public void PlayerAnsweredNo() {
        if (_charStatus == Status.EventWasGenerated) {
            curGuest.GetComponent<Event>().userAnswer = Event.Answer.No;
        }
    }

    public bool IsItAFreeFoodEvent() {
        if (_charStatus == Status.EventWasGenerated) {
            return curGuest.TryGetComponent<FreeFood>(out FreeFood hinge) && foodOrdering.isOrderTold;
        }
        return false;
    }

    private void InitTimeToShowDestroyTimer() {
        float coeff = 0.6f;
        timeToShowDestroyTimer = (int)Mathf.Round(coeff * timeBeforeDestroyingLeftGuest);
    }

    public void ShowTimeBeforeClientLeaveText() {
        timeBeforeClientLeaveText.gameObject.SetActive(true);
    }

    public void HideTimeBeforeClientLeaveText() {
        timeBeforeClientLeaveText.gameObject.SetActive(false);
    }
}
