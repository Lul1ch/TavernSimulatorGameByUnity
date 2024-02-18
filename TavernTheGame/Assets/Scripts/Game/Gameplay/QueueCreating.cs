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
    [Header("Training")]
    [SerializeField] private GameObject _orderClient;
    [SerializeField] private GameObject _eventClient;

    private static int guestCounter = 0;
    private float timeBeforeNewGuest = 20f;
    private int rand;
    private GameObject curGuest;
    private Vector3 spawnPoint;
    private Status _charStatus;
    private float waitTime = 30f;

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

    public GameObject eventClient {
        get { return _eventClient; }
    }

    private void Start() {
        //Заводим куратину на создание нового гостя через определённый временной промежуток
        InitSpawnPoint();
        StartCoroutine(SpawnNewGuestInQueue());
    }

    private void FixedUpdate() {
        //Если клиент ушёл, то удаляем его со сцены
        if (_charStatus != Status.Waiting && _charStatus != Status.EventWasGenerated){
            System.Threading.Thread.Sleep(1000);
            DestroyServicedGuest();
            SpawnNewGuest();
        }
    }

    private IEnumerator SpawnNewGuestInQueue() {
        SpawnNewGuest();
        //Тут ссылаемся на конкретные координаты, в случае чего поменять
        while (guestCounter < 16) {
            CreateGuest();

            yield return new WaitForSeconds(timeBeforeNewGuest);   
        }

    }

    public void SpawnNewGuest(){
        if (variants.Characters.Count == 0) {
            CreateGuest();
        }
        curGuest = Instantiate(variants.Characters[0], spawnPoint, Quaternion.identity);
        _charStatus = Status.Waiting;
        if ( SceneManager.GetActiveScene().name != "Training" ) {
            EventBus.onGuestSpawned?.Invoke();
        }
    }

    private void DestroyServicedGuest(){
        variants.Characters.RemoveAt(0);
        
        Destroy(curGuest);
        guestCounter--;
    }

    public int GetGuestCounter() {
        return guestCounter;
    }

    public void SetGuestCounter(int number) {
        if (PlayerPrefs.GetString("Result") != null) {
            guestCounter = number;
        }
    }

    public void CreateGuest() {
        rand = Random.Range(0, variants.CharactersSkins.Count);
        GameObject newGuest = variants.CharactersSkins[rand];
        variants.Characters.Add(newGuest);
        guestCounter++;
    }

    public GameObject GetCurGuest() {
        return curGuest;
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint) {
        spawnPoint = newSpawnPoint;
    }

    public void InvokeSetTimeIsUp() {
        Invoke("SetTimeIsUp", waitTime);
    }
    private void SetTimeIsUp() {
        if (_charStatus == Status.Waiting) {
            foodOrdering.AnswerIfClientWasntServiced();
            tavern.ChangeTavernBonus(-1);
            _charStatus = Status.Left;
        }
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
}
