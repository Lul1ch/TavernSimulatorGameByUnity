using UnityEngine;

public class VoiceOverText : MonoBehaviour
{
    [SerializeField] private AudioSource talkingAudioSource;
    private static VoiceOverText instance;
    private static bool isTalkingNow;

    private void Awake() {
        instance = this;
    }

    public static void StartTalkingSound() {
        if (isTalkingNow) {
            return;
        }
        isTalkingNow = true;
        instance.talkingAudioSource.Play();
    }

    public static void StopTalkingSound() {
        isTalkingNow = false;
        instance.talkingAudioSource.Stop();
    }
}
