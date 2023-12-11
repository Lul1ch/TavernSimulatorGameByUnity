using UnityEngine;

public class Character : MonoBehaviour
{
    public Type charType;
    private SpriteRenderer spriteRenderer;
    public PreferencesLevel charPrefs;
    private int satisfactionBorder;

    private CharactersVariants variants;

    private int rand;
    //Массив, хранящий в себе значения для satisfactionBorder в зависимости от типа персонажа
    private int[] satMas = new int[] {40, 60, 5, 50};

    public enum Type {
        Default = 0,Drunk,Tired,Pleasant
    }

    public enum PreferencesLevel {
        Primal,Normal,Advanced,Extended
    }

    private void Awake() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start() {
        //Определяем тип клиента через рандом
        var typeLength = Type.GetNames(typeof(Type)).Length;
        rand = Random.Range(0, typeLength);
        charType = (Type)rand;

        //Определяем предпочтения в еде клиента
        var prefsLength = PreferencesLevel.GetNames(typeof(PreferencesLevel)).Length;
        rand = Random.Range(0,prefsLength);
        charPrefs = (PreferencesLevel)rand;

        //На основе типа клиента задаём границу удовлетворения для клиента, для дальнейшего определения реакции его при выдаче заказа
        satisfactionBorder = satMas[(int)charType];
    }

    public int GetSatisfactionBorder() {
        return satisfactionBorder;
    }
}
