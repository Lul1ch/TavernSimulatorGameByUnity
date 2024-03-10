using UnityEngine;

public class DoubledTips : Bonus
{
    
    [SerializeField] private Tavern tavern;
    private void Start() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
    }
    public override void Buy() {
        tavern.bonusesValueModifier = 2;
    }
}
