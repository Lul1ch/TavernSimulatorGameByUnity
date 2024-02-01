using UnityEngine;

public class HalfCookTime : Bonus
{
    [SerializeField] private Kitchen kitchen;

    private void Start() {
        kitchen = FindObjectOfType<Kitchen>().GetComponent<Kitchen>();
    }

    public override void Buy() {
        kitchen.HalfDishesCookTime();
    }
}
