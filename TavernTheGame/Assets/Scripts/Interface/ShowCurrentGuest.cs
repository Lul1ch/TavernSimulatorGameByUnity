using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCurrentGuest : MonoBehaviour
{
    [SerializeField] private QueueCreating queueCreator;
    [SerializeField] private Transform cameraPos;

    public void ShowCurGuest() {
        GameObject curGuest = queueCreator.GetCurGuest();

        Vector3 guestPosition = curGuest.transform.position;
        curGuest.transform.position = new Vector3(guestPosition.x, guestPosition.y, cameraPos.position.z / 2);
        queueCreator.SetSpawnPoint(curGuest.transform.position);
    }
}
