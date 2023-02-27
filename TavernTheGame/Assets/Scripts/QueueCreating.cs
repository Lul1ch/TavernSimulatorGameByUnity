using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueCreating : MonoBehaviour
{
    [SerializeField] private CharactersVariants variants;
    [SerializeField] private GuestMover curGuest;

    private static int guestCounter = 0;
    private float waitTime = 18f;
    private int rand;

    public GameObject guestSample;
    public GameObject spawner;

    private void Start() {
        //Заводим куратину на создание нового гостя через определённый временной промежуток
        StartCoroutine(SpawnNewCharacter());
    }

    private void Update() {
        //Если клиент ушёл, то удаляем его со сцены
        if (curGuest.charStatus == GuestMover.Status.Out){
            DestroyServicedGuest();
        }
    }

    private IEnumerator SpawnNewCharacter(){
        //Тут ссылаемся на конкретные координаты, в случае чего поменять
        while (guestCounter < 16) {
            CreateGuestObject();

            yield return new WaitForSeconds(waitTime);   
        }

    }

    private void DestroyServicedGuest(){
        GameObject servicedGuest = variants.Characters[0];
        variants.Characters.RemoveAt(0);
        
        Destroy(servicedGuest,0.5f);

        curGuest.charStatus = GuestMover.Status.Spawned;
        curGuest.timeIsUped = false;

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

    public void CreateGuestObject() {
        rand = Random.Range(0, variants.CharactersSkins.Count);
        GameObject newGuest = Instantiate(variants.CharactersSkins[rand], variants.CharactersSkins[rand].transform.position, guestSample.transform.rotation);
        variants.Characters.Add(newGuest);
        guestCounter++;
    }
}
