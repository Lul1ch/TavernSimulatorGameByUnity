using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> showOrHideObjects;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float minMusicChangeStep = 0.05f;
    [Header("Button images")]
    [SerializeField] private Image mainButtomImage;
    [SerializeField] private Sprite musicOff;
    [SerializeField] private Sprite musicOn;


    public void ShowOrHideObjects() {
        foreach(var gameObject in showOrHideObjects) {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    public void MakeMusicVolumeLouder() {
        if (musicSource.volume <= 0f) {
            mainButtomImage.sprite = musicOn;
        }
        musicSource.volume += minMusicChangeStep;
    }

    public void MakeMusicVolumeQuieter() {
        if (musicSource.volume <= 0f) {
            return;
        }
        musicSource.volume -= minMusicChangeStep;
        if (musicSource.volume <= 0f) {
            mainButtomImage.sprite = musicOff;
        }
    }
}
