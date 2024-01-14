using UnityEngine;

public class Food : MonoBehaviour
{
    [Header("Food Variables")]
    public Quality foodQuality;
    public int price;
    public int cookingTime;
    private int rand;

    public enum Quality {
        Awful = 0, Premitive, Normal, Tasty, Delighful, MasterPiece
    }

    public void InitProductInfo() {
        RandPrice();
        SetCookingTime();
    }

    private void SetCookingTime() {
        cookingTime = (int)foodQuality * 3;
    }

    private void RandPrice() {
        rand = Random.Range(0,1);
        
        if (foodQuality != Quality.Awful) {
            price = (int)foodQuality * 2 - rand;
        } else {
            price = 0;
        }
    }

    public void ChangePrice(int number) {
        price = number;
    }

    public int GetPrice() {
        return price;
    }

    public int GetFoodQualityInt() {
        return (int)foodQuality;
    }
}

