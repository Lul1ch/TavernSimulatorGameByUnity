using System.Collections;
using UnityEngine;

public class LostGuest : Event
{
    [Header("LostGuestVariables")]
    [SerializeField] private string guestAgreed;
    [SerializeField] private string guestRefused;

    private void Start() {
        InvokeAnEvent();
    }

    protected override IEnumerator TriggerEventConsequences() {
        while (userAnswer == Answer.Empty) {
            yield return new WaitForSeconds(1f);
        }
    
        if (userAnswer == Answer.Yes) {
            ChangeMessageText(guestAgreed);
            //messageText = "Понял, спасибо!";
        } else if (userAnswer == Answer.No) {
            ChangeMessageText(guestRefused);
            //messageText = "Ахх...жаль.";
        }
        InvokeOnUserResponse();
    }
}
