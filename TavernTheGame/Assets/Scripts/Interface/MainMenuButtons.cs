using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject infoWindow;

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
}
