using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DishInfo : MonoBehaviour
{
    public int productCookingTime;
    public string productName;
    public int productIndex;
    public Sprite productSprite;
    public Dictionary<string, int> componentsNames = new Dictionary<string,int>();

    public Dictionary<string, int> GetDishComponents() {
        return componentsNames;
    }
    
}
