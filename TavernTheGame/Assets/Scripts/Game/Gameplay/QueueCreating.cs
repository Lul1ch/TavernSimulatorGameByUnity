using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueCreating : MonoBehaviour
{
    [SerializeField] private CharactersVariants variants;
    [SerializeField] private GuestMover guestMover;
    [SerializeField] private Transform cameraTransform;

    private static int guestCounter = 0;
    private float waitTime = 20f;
    private int rand;
    private GameObject curGuest;
    private Vector3 spawnPoint;

    private void Start() {
        //Заводим куратину на создание нового гостя через определённый временной промежуток
        spawnPoint = new Vector3(cameraTransform.position.x, cameraTransform.position.y, 0);
        StartCoroutine(SpawnNewGuestInQueue());
    }

    private void FixedUpdate() {
        //Если клиент ушёл, то удаляем его со сцены
        if (guestMover.GetStatus() != GuestMover.Status.Waiting && guestMover.GetStatus() != GuestMover.Status.EventWasGenerated){
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

            yield return new WaitForSeconds(waitTime);   
        }

    }

    private void SpawnNewGuest(){
        if (variants.Characters.Count == 0) {
            CreateGuest();
        }
        curGuest = Instantiate(variants.Characters[0], spawnPoint, Quaternion.identity);
        guestMover.SetStatus(GuestMover.Status.Waiting);
        EventBus.onGuestSpawned?.Invoke();
    }

    private void DestroyServicedGuest(){
        variants.Characters.RemoveAt(0);
        
        Destroy(curGuest);
        guestMover.CancelSTimeIsUpInvoke();
        guestMover.SetStatus(GuestMover.Status.Waiting);
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
}