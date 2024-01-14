using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image bar;
    private float fillAmount = 1f;
    private float multiplier = 1f;
    private TMP_Text textTimer;
    private int cookingTime;

    private void Update() {
        UpdateFillAmount();
    }

    private void UpdateFillAmount() {
        //Уменьшаем переменную и равняем её проценту заполненности шкалы объекта таймера
        fillAmount -= Time.deltaTime * multiplier;
        bar.fillAmount = fillAmount;
        textTimer.text = Mathf.Round(bar.fillAmount * cookingTime).ToString();
        //Если таймер кончился, то удаляем его
        if (fillAmount <= 0) {
            textTimer.text = cookingTime.ToString();
            Destroy(gameObject);
        }
    }

    //С помощью этой функции регулируем время действия таймера, условно устанавливаем его на определённый промежуток времени
    public void SetMultiplier(int cookingTime) {
        multiplier /= cookingTime;
        SaveCookingTime(cookingTime);
    }

    private void SaveCookingTime(int cookingTime) {
        this.cookingTime = cookingTime;
    }

    public float GetFillAmount() {
        return fillAmount;
    }

    public void GetTimerTextObject() {
        textTimer = gameObject.transform.parent.Find("Time").gameObject.GetComponent<TMP_Text>();
    }
}
