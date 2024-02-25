using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEventsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> eventGuests;
    [Header("MessageVariables")]
    [SerializeField] private Text _messageText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button denyButton;
    [SerializeField] private Button nextButton;
    [Header("SceneObjects")]
    [SerializeField] private QueueCreating queueCreating;
    private void Start() {
        InitiateEventsMessageVariables();
    }

    public GameObject GetRandomEventGuest() {
        return eventGuests[Random.Range(0, eventGuests.Count)];
    }

    private void InitiateEventsMessageVariables() {
        foreach(var eventGuest in eventGuests) {
            eventGuest.GetComponent<Event>().InitializeMessageVariables(_messageText, confirmButton, denyButton, nextButton);
        }
        queueCreating.isEventsReadyToCreate = true;
    }
}
