using UnityEngine;

public class Food : MonoBehaviour
{
    [Header("Food Variables")]
    private int rand;
    [SerializeField] private  Quality _foodQuality;
    [SerializeField] private  int _price;
    [SerializeField] private  int _cookingTime;

    public Quality foodQuality {
        get { return _foodQuality; }
        set { _foodQuality = value; }
    }

    public int price {
        get { return _price; }
        set { _price = value; }
    }

    public int cookingTime {
        get { return _cookingTime; }
        set { _cookingTime = value; }
    }


    public enum Quality {
        Awful = 0, Premitive, Normal, Tasty, Delighful, MasterPiece
    }

    public void InitProductInfo() {
        RandPrice();
        SetCookingTime();
    }

    private void SetCookingTime() {
        _cookingTime = (int)_foodQuality * 3;
    }

    private void RandPrice() {
        rand = Random.Range(0,1);
        
        if (_foodQuality != Quality.Awful) {
            _price = (int)_foodQuality * 2 - rand;
        } else {
            _price = 0;
        }
    }

    public void ChangePrice(int number) {
        _price = number;
    }

    public int GetPrice() {
        return _price;
    }

    public int GetFoodQualityInt() {
        return (int)_foodQuality;
    }
}

