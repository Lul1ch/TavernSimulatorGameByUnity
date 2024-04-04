using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideObjectsButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToShowOrHide;

    private void Start() {
        gameObject.GetComponent<Button>().onClick.AddListener(() => ShowOrHideObjects());
    }

    private void ShowOrHideObjects() {
        foreach(var sceneObject in objectsToShowOrHide) {
            sceneObject.SetActive(!sceneObject.activeSelf);
        }
    }
}
