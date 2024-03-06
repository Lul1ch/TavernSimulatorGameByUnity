using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Event : MonoBehaviour
{
    public enum Answer {
        Empty, Yes, No, FreeDish
    }   
    [Header("Message")]
    [SerializeField] private Text _messageText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button denyButton;
    [SerializeField] private Button nextButton;
    [Header("EventVariables")]
    [SerializeField] private bool activateConfirmButton;
    [SerializeField] private bool activateDenyButton;
    [SerializeField] private string welcomeMessage;
    [SerializeField] protected float _xOffset = 0f;
    [SerializeField] protected float _yOffset = 0f;

    protected IEnumerator eventCoroutine;
    protected Answer _userAnswer;
    
    public float xOffset {
        get { return _xOffset; }
    }
    public float yOffset {
        get { return _yOffset; }
    }
    public Answer userAnswer {
        get { return _userAnswer; }
        set { _userAnswer = value; }
    }

    public void InitializeMessageVariables(Text _messageText, Button confirmButton, Button denyButton, Button nextButton) {
        this._messageText = _messageText;
        this.confirmButton = confirmButton;
        this.denyButton = denyButton;
        this.nextButton = nextButton;
    }

    protected void ChangeMessageText(string message) {
        _messageText.text = message;
    }

    protected void ChangeMessageButtonsVisibility(bool confirmButtonState, bool denyButtonState) {
        confirmButton.gameObject.SetActive(confirmButtonState);
        denyButton.gameObject.SetActive(denyButtonState);
    }

    protected void InvokeAnEvent() {
        ChangeMessageText(welcomeMessage);
        ChangeMessageButtonsVisibility(activateConfirmButton, activateDenyButton);
        eventCoroutine = TriggerEventConsequences();
        StartCoroutine(eventCoroutine);
    }

    protected virtual IEnumerator TriggerEventConsequences() {
        while (userAnswer == Answer.Empty) {
            yield return new WaitForSeconds(1f);
        }
    }

    protected void InvokeOnUserResponse() {
        ChangeMessageButtonsVisibility(false, false);
        EventBus.onGuestReacted?.Invoke();
    }
}
