using UnityEngine;

public class DoubledTips : Bonus
{
    [SerializeField] private FoodOrdering foodOrdering;
    private void Start() {
        foodOrdering = FindObjectOfType<FoodOrdering>().GetComponent<FoodOrdering>();
    }
    public override void Buy() {
        foodOrdering.tipsPrice = foodOrdering.tipsPrice * 2;
    }
}
