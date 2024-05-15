using UnityEngine;
using System.Collections;

public class HalfCookTime : Bonus
{
    [SerializeField] private Kitchen kitchen = null;

    private void Start() {
        kitchen = FindObjectOfType<Kitchen>().GetComponent<Kitchen>();
    }

    public override void Buy() {
        StartCoroutine("PurchaseCoroutine");
    }

    private IEnumerator PurchaseCoroutine() {
        while(kitchen == null) {
            yield return new WaitForSeconds(1f);
        }
        kitchen.HalfDishesCookTime();
    }
}
