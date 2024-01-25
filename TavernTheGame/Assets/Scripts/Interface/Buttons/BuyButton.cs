using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    [Header("Scene's objects")]
    [SerializeField] private Button buyButton;
    [SerializeField] private Hint hint;

    private Tavern tavern;
    private Product product;
    private GameObject productObject;
    private bool isReadyForNextHint = true;
    private void Start() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        hint = GameObject.Find("/Shop/ShopObjectsGroup/ShopInterface/FAQShop").GetComponent<Hint>();
        buyButton.onClick.AddListener(() => BuySelectedProduct());
    }

    private void BuySelectedProduct() {
        if (tavern.GetTavernMoney() >= product.price) {
            tavern.DecreaseTavernMoney(product.price);
            tavern.UpdateDictionary(product.foodName, productObject);
            tavern.UpdateStorageInfo(product.foodName, productObject);

            gameObject.GetComponent<AudioSource>().Play();
        } else if (isReadyForNextHint) {
            hint.ShowHint(Hint.EventType.NotEnoughMoney, string.Concat((product.price - tavern.GetTavernMoney()).ToString()," монет."));
            isReadyForNextHint = false;
            Invoke("IsReadyForNextHint", hint.hintLifeTime);
        }
    }

    public void InitProductVariable(GameObject obj) {
        productObject = obj;
        product = obj.GetComponent<Product>();
    }

    private void IsReadyForNextHint() {
        isReadyForNextHint = true;
    }
}
