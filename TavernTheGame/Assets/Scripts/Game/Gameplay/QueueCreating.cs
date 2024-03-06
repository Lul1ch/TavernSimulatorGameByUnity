using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QueueCreating : MonoBehaviour
{
    [SerializeField] private CharactersVariants variants;
    [SerializeField] private Transform spawnPointTransform;
    [SerializeField] private FoodOrdering foodOrdering;
    [SerializeField] private Tavern tavern;
    [SerializeField] private GameEventsManager gameEventsManager;
    [Header("Training")]
    [SerializeField] private GameObject _orderClient;

    private float timeBeforeNewGuest = 20f;
    private int rand;
    private GameObject curGuest;
    private Vector3 spawnPoint;
    private Status _charStatus;
    private float waitTime = 30f;
    private int _eventIntiationBorder = 40, maxEventInitiationBorder = 90, eventIntiationBorderReductionStep = 10;
    private bool _isEventsReadyToCreate;

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

    private void Start() {
        //Заводим куратину на создание нового гостя через определённый временной промежуток
        InitSpawnPoint();
        StartCoroutine(SpawnNewGuestInQueue());
    }

    private IEnumerator SpawnNewGuestInQueue() {
        SpawnNewGuest();
        CreateGuest();

        yield return new WaitForSeconds(timeBeforeNewGuest);  
    }

    public void SpawnNewGuest(){
        if (variants.Characters.Count == 0) {
            CreateGuest();
        }
        int randForEvent = Random.Range(0, 100);
        GameObject guestToInstaniate = variants.Characters[0]; //костыль
        float xOffset = 0;
        float yOffset = 0;
        if ( randForEvent > _eventIntiationBorder && _isEventsReadyToCreate) {
            guestToInstaniate = gameEventsManager.GetRandomEventGuest();
            _charStatus = Status.EventWasGenerated;
            _eventIntiationBorder = maxEventInitiationBorder;
            xOffset = guestToInstaniate.GetComponent<Event>().xOffset;
            yOffset = guestToInstaniate.GetComponent<Event>().yOffset;
        } else {
            if (_eventIntiationBorder > Mathf.Round(maxEventInitiationBorder / 2)) {
                _eventIntiationBorder -= eventIntiationBorderReductionStep;
            }
            guestToInstaniate = variants.Characters[0];
            _charStatus = Status.Waiting;
            xOffset = guestToInstaniate.GetComponent<Character>().xOffset;
            yOffset = guestToInstaniate.GetComponent<Character>().yOffset;
        }
        curGuest = Instantiate(guestToInstaniate, new Vector3(spawnPoint.x + xOffset, spawnPoint.y + yOffset, spawnPoint.z), Quaternion.identity);
        if ( SceneManager.GetActiveScene().name != "Training" ) {
            EventBus.onGuestSpawned?.Invoke();
        }
    }

    public void DestroyServicedGuest(){
        variants.Characters.RemoveAt(0);
        
        Destroy(curGuest);
        SpawnNewGuest();
    }

    public void CreateGuest() {
        rand = Random.Range(0, variants.CharactersSkins.Count);
        GameObject newGuest = variants.CharactersSkins[rand];
        variants.Characters.Add(newGuest);
    }

    public GameObject GetCurGuest() {
        return curGuest;
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint) {
        spawnPoint = newSpawnPoint;
    }

    public void InvokeSetTimeIsUp() {
        if (_charStatus == Status.Waiting) {
            Invoke("SetTimeIsUp", waitTime);
        }
    }
    private void SetTimeIsUp() {
        foodOrdering.AnswerIfClientWasntServiced();
        tavern.tavernBonus -= 1;
        _charStatus = Status.Left;
        EventBus.onGuestReacted?.Invoke();
    }

    public void CancelSTimeIsUpInvoke() {
        CancelInvoke("SetTimeIsUp");
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
            return curGuest.TryGetComponent<FreeFood>(out FreeFood hinge);
        }
        return false;
    }
}
