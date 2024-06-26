using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BuyButton : MonoBehaviour
{
    [Header("Scene's objects")]
    [SerializeField] private Button buyButton;
    [SerializeField] private Hint hint;
    [SerializeField] private TrainingManager trainingManager;

    private Tavern tavern;
    private Product product;
    private GameObject productObject;
    private bool isReadyForNextHint = true;
    private bool isCreditDecreaseAvailable = true;
    private void Start() {
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        hint = GameObject.Find("/Shop/ShopObjectsGroup/ShopInterface/FAQShop").GetComponent<Hint>();
        if ( SceneManager.GetActiveScene().name == "Training") {
            trainingManager = FindObjectOfType<TrainingManager>().GetComponent<TrainingManager>();
            
        }
        buyButton.onClick.AddListener(() => BuySelectedProduct());
    }

    private void BuySelectedProduct() {
        if (tavern.tavernMoney >= product.price) {
            tavern.tavernMoney -= product.price;
            tavern.UpdateDictionary(product.foodName, tavern.productContentParent, productObject);

            gameObject.GetComponent<AudioSource>().Play();
            if ( SceneManager.GetActiveScene().name == "Training" && isCreditDecreaseAvailable) {
                trainingManager.creditsForNextStep--;
                isCreditDecreaseAvailable = false;
                gameObject.SetActive(false);
            }
        } else if (isReadyForNextHint) {
            hint.ShowHint(Hint.EventType.NotEnoughMoney, (product.price - tavern.tavernMoney).ToString() + ".");
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
