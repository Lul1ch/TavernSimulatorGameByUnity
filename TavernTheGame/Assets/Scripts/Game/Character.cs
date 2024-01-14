using UnityEngine;

public class Character : MonoBehaviour
{
    public enum PreferencesLevel {
        Primal,Normal,Advanced,Extended
    }
    public enum Sex {
        Male, Female
    }
    [SerializeField] private Sex _characterGender;
    private PreferencesLevel _charPrefs;
    public PreferencesLevel charPrefs {
        get { return _charPrefs; }
        set { _charPrefs = value; }
    }

    public Sex characterGender {
        get { return _characterGender; }
    }

    private void Start() {
        //Определяем предпочтения в еде клиента
        var prefsLength = PreferencesLevel.GetNames(typeof(PreferencesLevel)).Length;
        int rand = Random.Range(0, prefsLength);
        charPrefs = (PreferencesLevel)rand;
    }
}
