using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CookButton : MonoBehaviour
{
    private IEnumerator coroutine;
    private bool isReadyForNextHint = true;
    //Счётсиком будем регулировать,чтоб в один момент готовилось не более трёх блюд
    static int curCookingDishCounter = 3;

    [Header("Scripts")]
    [SerializeField] private Kitchen kitchen;
    [SerializeField] private Tavern tavern;
    [SerializeField] private Dish dishScript;
    [SerializeField] private TrainingManager trainingManager;

    [Header("Scene's objects")]
    [SerializeField] private GameObject timerSample;
    [SerializeField] private Button cookButton;
    [SerializeField] private Transform contentElemTransform;
    [SerializeField] private GameObject dishObj;
    [SerializeField] private Hint hint;

    private void Start() {
        InitializeVariables();
        cookButton.onClick.AddListener(() => CookSelectedDish());
    }

    private void InitializeVariables() {
        kitchen = FindObjectOfType<Kitchen>().GetComponent<Kitchen>();
        tavern = FindObjectOfType<Tavern>().GetComponent<Tavern>();
        hint = GameObject.Find("/Kitchen").transform.Find("KitchenObjectsGroup").transform.Find("KitchenInterface").transform.Find("FAQKitchen").GetComponent<Hint>();
        contentElemTransform = transform.parent;

        if ( SceneManager.GetActiveScene().name == "Training" ) {
            trainingManager = FindObjectOfType<TrainingManager>().GetComponent<TrainingManager>();
        }
    }

    public string CookSelectedDish(bool isItAutomaticCook = false) {
        bool isAllComponentsAvailable = true;
        string additionalStringForHint = "";
        Dictionary<GameObject, int> components = dishScript.componentsObjects;
        foreach(var component in components) {
            if(!tavern.IsEnoughFoodInStorage(component.Key.name, component.Value)) {
                additionalStringForHint += component.Key.name + " : " + (component.Value - tavern.GetNumberOfFoodInStorage(component.Key.name)).ToString() +", ";
                isAllComponentsAvailable = false;
            }
        }
        additionalStringForHint += '.';
        additionalStringForHint = additionalStringForHint.Replace(", .", ".");
        
        if (isAllComponentsAvailable && curCookingDishCounter > 0) {
                    
            foreach(var component in components) {
                tavern.ReduceFoodNumber(component.Key.name, component.Value);
            }
            //Создаём таймер готовки блюда
            CanvasButtons.PlayOnClickSound(gameObject.GetComponent<AudioSource>());

            GameObject newTimerObject = Instantiate(timerSample, contentElemTransform.position, contentElemTransform.rotation);
            Timer curTimer = newTimerObject.GetComponent<Timer>();
            //Задаём родительский объект, для корректного отображения таймера
            newTimerObject.transform.SetParent(contentElemTransform);
            //Устанавливаем время готовки
            curTimer.GetTimerTextObject();
            curTimer.SetMultiplier((float)dishScript.dishCookingTime);
            
            tavern.UpdateStorageInfo(dishScript.foodName, tavern.dishContentParent, dishObj, 0);
            
            curCookingDishCounter--;

            coroutine = addFinishedDish(curTimer, components);
            StartCoroutine(coroutine);
        } else if (isReadyForNextHint && !isItAutomaticCook) {
            Hint.EventType hintType = Hint.EventType.MaxFoodCooking;
            if (curCookingDishCounter == 0) {
                additionalStringForHint = "";
            } else {
                hintType = Hint.EventType.NotEnoughProducts;
            }
            hint.ShowHint(hintType, additionalStringForHint);
            isReadyForNextHint = false;
            Invoke("IsReadyForNextHint", hint.hintLifeTime);
        } else if (isItAutomaticCook) {
            return additionalStringForHint;
        }
        
        return null;
    }

    private IEnumerator addFinishedDish(Timer curTimer, Dictionary<GameObject, int> components) {
        while(curTimer.GetFillAmount() > 0) {
            yield return new WaitForSeconds(1f);
        }
        tavern.UpdateDictionary(dishScript.foodName, tavern.dishContentParent, dishObj);
        
        curCookingDishCounter++;
        if ( SceneManager.GetActiveScene().name == "Training" ) {
            trainingManager.creditsForNextStep--;
        }
        StopCoroutine(coroutine);
    }

    public void InitDishVariable(GameObject obj) {
        dishObj = obj;
        dishScript = obj.GetComponent<Dish>();
    }

    private void IsReadyForNextHint() {
        isReadyForNextHint = true;
    }
}
