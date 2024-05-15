using UnityEngine;

public class Bonus : MonoBehaviour
{
    [Header("BonusVariables")]
    [SerializeField] private string _description;
    [SerializeField] private int _price;
    [SerializeField] private int _bonusIndex;
    [SerializeField] private string _bonusName;

    public string description {
        set { _description = value; }
        get { return _description; }
    }
    public int price {
        set { _price = value; }
        get { return _price; }
    }
    public int bonusIndex{
        set { _bonusIndex = value; }
        get { return _bonusIndex; }
    }
    public string bonusName {
        set { bonusName = value; }
        get { return _bonusName; }
    }

    virtual public void Buy() {}
}
