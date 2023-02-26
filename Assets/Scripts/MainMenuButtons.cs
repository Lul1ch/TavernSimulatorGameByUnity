using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject infoWindow;
    [SerializeField] private GameObject tutorialWindow;

    //Функции изменения отображения окна с информационной сводкой по игре
    public void ChangeInfoWindowActivity(bool isActive) {
        if (isActive) {
            infoWindow.SetActive(true);
        } else {
            Invoke("CloseInfoWindowInvoke", 0.5f);
        }
    }

    private void CloseInfoWindowInvoke() {
        infoWindow.SetActive(false);
    }

    public void ChangeTutorialWindowActivity(bool isActive) {
        if (isActive) {
            tutorialWindow.SetActive(true);
        } else {
           Invoke("CloseTutorialWindowInvoke", 0.5f); 
        }
    }

    private void CloseTutorialWindowInvoke() {
        tutorialWindow.SetActive(false);
    }
}
