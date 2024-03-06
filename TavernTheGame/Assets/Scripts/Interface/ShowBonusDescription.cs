using UnityEngine;

public class ShowBonusDescription : MonoBehaviour
{
    [SerializeField] private Hint hint;

    private void Start() {
        hint = GameObject.Find("/Tavern/TavernObjectsGroup/TavernInterface/FAQTavern").GetComponent<Hint>();
    }

    private void OnMouseDown() {
        string bonusDescription = "<color=black>Описание: </color>" + transform.Find("Icon").gameObject.GetComponent<Bonus>().description;
        hint.ShowBonusDescription(bonusDescription);
    }

    private void OnMouseUp() {
        hint.HideHint();
    }
}
