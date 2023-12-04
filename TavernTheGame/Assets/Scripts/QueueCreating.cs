using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueCreating : MonoBehaviour
{
    [SerializeField] private CharactersVariants variants;
    [SerializeField] private GuestMover guestMover;
    [SerializeField] private Transform spawnPoint;

    private static int guestCounter = 0;
    private float waitTime = 18f;
    private int rand;
    private GameObject curGuest;

    private void Start() {
        //Заводим куратину на создание нового гостя через определённый временной промежуток
        StartCoroutine(SpawnNewCharacter());
    }

    private void FixedUpdate() {
        //Если клиент ушёл, то удаляем его со сцены
        if (guestMover.GetStatus() != GuestMover.Status.Waiting && guestMover.GetStatus() != GuestMover.Status.EventWasGenerated){
            DestroyServicedGuest();
            SpawnNewGuest();
        }
    }

    private IEnumerator SpawnNewCharacter() {
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
        curGuest = Instantiate(variants.Characters[0], spawnPoint.position, Quaternion.identity);
        guestMover.SetStatus(GuestMover.Status.Waiting);
    }

    private void DestroyServicedGuest(){
        variants.Characters.RemoveAt(0);
        
        Destroy(curGuest);

        guestMover.SetStatus(GuestMover.Status.Waiting);
        guestMover.SetTimeIsUp(false);

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
        //Debug.Log("Counter of guests " + guestCounter);
        guestCounter++;
    }
}
