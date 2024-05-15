using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bonuses : MonoBehaviour
{
    [SerializeField] private List<GameObject> bonusesList;
    [SerializeField] private GameObject bonusesContentElementSample;
    [SerializeField] private Transform parent;

    private void Start() {
        InitBonusesShowcase();
    }

    private void InitBonusesShowcase() { 
        int index = 0;
        //Для каждого элемента, который лежит в массиве блюд создаём визуальное отображение в скролере с помощью ранее созданного объекта шаблона
        foreach(var elem in bonusesList) {
            GameObject curElement = Instantiate(bonusesContentElementSample, bonusesContentElementSample.transform.position, bonusesContentElementSample.transform.rotation);
            GameObject bonus = InstantiateBonusIcon(curElement, elem);
            curElement.name = bonus.GetComponent<Bonus>().bonusName; 

            Bonus curBonusScript = bonus.GetComponent<Bonus>();

            curElement.transform.Find("Price").GetComponent<Text>().text = curBonusScript.price.ToString();
            curElement.transform.Find("Name").GetComponent<TMP_Text>().text = elem.name;

            curElement.transform.SetParent(parent, false);
            index++;
        }
    }

    private GameObject InstantiateBonusIcon(GameObject curContentElement, GameObject objToInstantiate) {
        Transform curIconTransform = curContentElement.transform.Find("PositionForIcon");
        GameObject iconObject = GameObject.Instantiate(objToInstantiate, curIconTransform.position, Quaternion.identity);

        iconObject.transform.SetParent(curContentElement.transform, false);
        Destroy(curIconTransform.gameObject);

        iconObject.name = "Icon";
        
        return iconObject;
    }
}
