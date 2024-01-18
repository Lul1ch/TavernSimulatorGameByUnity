using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    [Header("Scene's objects")]
    [SerializeField] private Button buyButton;

    private Tavern tavern;
    private Product product;
    private GameObject productObject;
    private void Start() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        buyButton.onClick.AddListener(() => BuySelectedProduct());
    }

    private void BuySelectedProduct() {
        if (tavern.GetTavernMoney() >= product.price) {
            tavern.DecreaseTavernMoney(product.price);
            tavern.UpdateDictionary(product.foodName, productObject);
            tavern.UpdateStorageInfo(product.foodName, productObject);

            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    public void InitProductVariable(GameObject obj) {
        productObject = obj;
        product = obj.GetComponent<Product>();
    }
}
