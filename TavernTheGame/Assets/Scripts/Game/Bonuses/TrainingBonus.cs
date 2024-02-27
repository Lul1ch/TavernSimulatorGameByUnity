using UnityEngine;

public class TrainingBonus : Bonus
{
    [SerializeField] private Tavern tavern;
    [SerializeField] private int bonusMoneyNumber = 10;
    private void Start() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
    }
    public override void Buy() {
        tavern.IncreaseTavernMoney(bonusMoneyNumber);
    }
}
