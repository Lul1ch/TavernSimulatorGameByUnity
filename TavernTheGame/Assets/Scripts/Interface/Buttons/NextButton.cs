using UnityEngine;
using UnityEngine.UI;

public class NextButton : MonoBehaviour
{
    private void Start() {
        gameObject.GetComponent<Button>().onClick.AddListener(() => InvokeOnClick());
    }

    private void InvokeOnClick() {
        EventBus.onGuestLeft?.Invoke();
    }
}
