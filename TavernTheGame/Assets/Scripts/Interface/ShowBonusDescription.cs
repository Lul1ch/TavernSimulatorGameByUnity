using UnityEngine;

public class ShowBonusDescription : MonoBehaviour
{
    private bool isReadyToShowDescription = true;
    private float timeBeforeNextDisplay = 1f;
    [SerializeField] private Hint hint;

    private void Start() {
        hint = GameObject.Find("/Tavern/TavernObjectsGroup/TavernInterface/FAQTavern").GetComponent<Hint>();
    }

    private void OnMouseEnter() {
        if (!isReadyToShowDescription) {
            return;
        }
        string bonusDescription = "<color=black>Описание: </color>" + transform.Find("Icon").gameObject.GetComponent<Bonus>().description;
        hint.ShowBonusDescription(bonusDescription);
        isReadyToShowDescription = false;
        Invoke("InvokeReadyToShowDescription",timeBeforeNextDisplay);
    }

    private void OnMouseExit() {
        hint.HideHint();
    }

    private void InvokeReadyToShowDescription() {
        isReadyToShowDescription = true;
    }
}
