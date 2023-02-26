using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersVariants : MonoBehaviour {
    [Header("Characters")]
    public List<GameObject> Characters;
    public List<GameObject> CharactersSkins;

    [Header("Food")]
    public List<GameObject> AlcoholWarehouse;

    public List<GameObject> GarbageWarehouse;
    public List<GameObject> ComponentWarehouse;

    public List<GameObject> SnackWarehouse;

    public List<GameObject> SimpleDishWarehouse;

    public List<GameObject> DishWarehouse;

    [Header("Phrases")]
    public List<string> HelloPhrases;
    public List<string> OrderPhrases;

    public List<string> BadReactPharases;
    public List<string> GoodReactPharases;

    public List<string> WasntServicedPhrases;

    [Header("Emojis")]
    public List<Sprite> CrazyEmojis;
    public List<Sprite> GoodEmojis;
    public List<Sprite> BadEmojis;
    public List<Sprite> SadEmojis;

    [Header("Sounds")]
    public List<AudioClip> SpeechSounds;

    private void Start() {
    }

}
