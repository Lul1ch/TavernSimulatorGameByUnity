using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookBook : MonoBehaviour
{
    [SerializeField] private Kitchen kitchen;
    [SerializeField] private GameObject contentElementSample;
    [SerializeField] private Transform parent;

    private List<GameObject> dishes;

    private void Start() {
        dishes = kitchen.GetDishesList();
        FillCookBookWindow();
    }

    private void FillCookBookWindow() {
        foreach(var elem in dishes) {
            GameObject curElement = Instantiate(contentElementSample, contentElementSample.transform.position, Quaternion.identity);

            Transform elemTransform = curElement.transform;
            Sprite curDishSprite = elem.GetComponent<Image>().sprite;
            Dish elemDishScript = elem.GetComponent<Dish>();

            elemTransform.Find("DishIcon").GetComponent<Image>().sprite = curDishSprite;
            List<GameObject> components = elemDishScript.dishComponents;
            Transform componentsParent = elemTransform.Find("ComponentsParent");
            Transform componentIcon = elemTransform.Find("ComponentIcon");
            foreach(var component in components) {
                GameObject curIcon = Instantiate(componentIcon.gameObject, componentIcon.position, Quaternion.identity);

                curIcon.GetComponent<Image>().sprite = component.GetComponent<Image>().sprite;
                curIcon.transform.SetParent(componentsParent, false);
            }
            Destroy(componentIcon.gameObject);
            elemTransform.SetParent(parent, false);
        }
    }
}
