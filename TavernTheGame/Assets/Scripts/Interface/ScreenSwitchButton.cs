using UnityEngine;
using UnityEngine.UI;
public class ScreenSwitchButton : MonoBehaviour
{
    [SerializeField] private GameObject nextScreen;
    [SerializeField] private GameObject curScreen;

    [SerializeField] private Button button;

    private void Start() {
        button.onClick.AddListener(() => SwitchScreen());
    }

    private void SwitchScreen() {
        curScreen.SetActive(false);
        nextScreen.SetActive(true);
    }
}
