using System.Collections.Generic;
using UnityEngine;

public class CharactersVariants : MonoBehaviour {
    [Header("Characters")]
    public List<GameObject> Characters;
    public List<GameObject> UnfairCharacters;
    public List<GameObject> CharactersSkins;

    [Header("Phrases")]
    public List<string> HelloPhrases;
    public List<string> OrderPhrases;

    public List<string> BadReactPharases;
    public List<string> GoodReactPharases;

    public List<string> WasntServicedPhrases;

    [Header("Sounds")]
    public List<AudioClip> SpeechSounds;


    public GameObject GetRandomUnfairGuest() {
        return UnfairCharacters[Random.Range(0, UnfairCharacters.Count)];
    }
}
