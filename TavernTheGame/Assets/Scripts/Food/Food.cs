using UnityEngine;

public class Food : MonoBehaviour
{
    [Header("Food Variables")]
    private int rand;
    [SerializeField] private string _foodName;
    [SerializeField] private  Quality _foodQuality;
    [SerializeField] private int _price;

    public string foodName {
        get { return _foodName; }
        set { _foodName = value; }
    }

    public Quality foodQuality {
        get { return _foodQuality; }
        set { _foodQuality = value; }
    }

    public int price {
        get { return _price; }
        set { _price = value; }
    }

    public enum Quality {
        Awful = 0, Premitive, Normal, Tasty, Delighful, MasterPiece
    }

    public void InitProductInfo() {
        RandPrice();
    }

    private void RandPrice() {
        rand = Random.Range(0,1);
        
        if (_foodQuality != Quality.Awful) {
            _price = (int)_foodQuality * 2 - rand;
        } else {
            _price = 0;
        }
    }
}

