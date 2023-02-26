using UnityEngine;
using UnityEngine.UI;

public class DishCond : MonoBehaviour
{
    [SerializeField] private GameObject componentProduct;

    public GameObject GetComponentObject() {
        return componentProduct;
    }

    public string GetComponentName() {
        return componentProduct.name;
    }

    public Sprite GetComponentSprite() {
        return componentProduct.GetComponent<SpriteRenderer>().sprite;
    }
}
