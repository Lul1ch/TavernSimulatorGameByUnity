using UnityEngine;

public class HideCurrentGuest : MonoBehaviour
{
    [SerializeField] private QueueCreating queueCreator;
    [SerializeField] private Transform cameraPos;

    public void HideCurGuest() {
        GameObject curGuest = queueCreator.GetCurGuest();

        Vector3 guestPosition = curGuest.transform.position;
        curGuest.transform.position = new Vector3(guestPosition.x, guestPosition.y, cameraPos.position.z*2);
        queueCreator.SetSpawnPoint(curGuest.transform.position);
    }
}
