using UnityEngine;

public class Food : MonoBehaviour
{
    public Quality foodQuality;
    public int satiety;
    public int price;
    public int cookingTime;
    private int rand;

    public enum Quality {
        Awful = 0, Premitive, Normal, Tasty, Delighful, MasterPiece
    }

    public void InitProductInfo() {
        SetPrice();
        SetSatiety();
        SetCookingTime();
    }

    private void SetCookingTime() {
        cookingTime = (int)foodQuality * 3;
    }

    private void SetPrice() {
        rand = Random.Range(0,1);
        
        if (foodQuality != Quality.Awful) {
            price = (int)foodQuality * 2 - rand;
        } else {
            price = 0;
        }
    }

    private void SetSatiety() {
        rand = Random.Range(0,20);
        
        if (this.foodQuality != Quality.Awful) {
            satiety = (int)foodQuality * 20 - rand;
        } else {
            satiety = 0;
        }
    }
}

