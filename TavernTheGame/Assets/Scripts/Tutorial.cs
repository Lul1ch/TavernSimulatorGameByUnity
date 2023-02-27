using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] private List<Sprite> tutorialPages;
    [SerializeField] private List<string> tutorialAdvices;

    [Header("Objects")]
    [SerializeField] private Image tutorialImage;
    [SerializeField] private Text tutorialText;

    private int index = 0;

    private void Update() {
        UpdateTutorialInterface();
    }

    private void UpdateTutorialInterface() {
        tutorialImage.sprite = tutorialPages[index];
        tutorialText.text = tutorialAdvices[index];
    }

    public void ChangeIndex(int value) {
        if (index + value >= 0 && index + value < tutorialAdvices.Count) {
            index += value;
        }
    }
}
