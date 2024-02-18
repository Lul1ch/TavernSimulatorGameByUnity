using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ScreenSwitchButton : MonoBehaviour
{
    [SerializeField] private GameObject nextScreen;
    [SerializeField] private GameObject curScreen;
    [SerializeField] private TrainingManager trainingManager;
    [SerializeField] private Button button;

    private void Start() {
        button.onClick.AddListener(() => SwitchScreen());
    }

    private void SwitchScreen() {
        curScreen.SetActive(false);
        nextScreen.SetActive(true);
        if ( SceneManager.GetActiveScene().name == "Training" ) {
            EventBus.onTrainGuestToldHisOrder?.Invoke();
            trainingManager.ChangeMessageIndex(1);
        }
    }
}
