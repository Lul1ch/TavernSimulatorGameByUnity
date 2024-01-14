using UnityEngine;

public class Character : MonoBehaviour
{
    private PreferencesLevel _charPrefs;
    public PreferencesLevel charPrefs {
        get { return _charPrefs; }
        set { _charPrefs = value; }
    }

    public enum PreferencesLevel {
        Primal,Normal,Advanced,Extended
    }

    private void Start() {
        //Определяем предпочтения в еде клиента
        var prefsLength = PreferencesLevel.GetNames(typeof(PreferencesLevel)).Length;
        int rand = Random.Range(0, prefsLength);
        charPrefs = (PreferencesLevel)rand;
    }
}
