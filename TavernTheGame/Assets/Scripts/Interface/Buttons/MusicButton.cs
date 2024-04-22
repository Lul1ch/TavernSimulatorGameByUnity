using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> showOrHideObjects;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Slider slider;
    [Header("Button images")]
    [SerializeField] private Image mainButtonImage;
    [SerializeField] private Sprite musicOff;
    [SerializeField] private Sprite musicOn;

    private void Start() {
        ChangeSliderValue(musicSource.volume);
    }

    public void ShowOrHideObjects() {
        foreach(var gameObject in showOrHideObjects) {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    public void ChangeSliderValue(float value) {
        slider.value = value;
        UpdateMusicButtonSprite();
    }

    public void ChangeMusicVolume() {
        musicSource.volume = slider.value;
        UpdateMusicButtonSprite();
    }

    private void UpdateMusicButtonSprite() {
        if (musicSource.volume <= 0f) {
            mainButtonImage.sprite = musicOff;
        } else {
            mainButtonImage.sprite = musicOn;
        }
    }
}
