using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image bar;
    private float fillAmount = 1f;
    private float multiplier = 1f;

    private void Update() {
        UpdateFillAmount();
    }

    private void UpdateFillAmount() {
        //Уменьшаем переменную и равняем её проценту заполненности шкалы объекта таймера
        fillAmount -= Time.deltaTime * multiplier;
        bar.fillAmount = fillAmount;
        //Если таймер кончился, то удаляем его
        if (fillAmount <= 0) {
            Destroy(gameObject);
        }
    }

    //С помощью этой функции регулируем время действия таймера, условно устанавливаем его на определённый промежуток времени
    public void SetMultiplier(int cookingTime) {
        multiplier /= cookingTime;
    }

    public float GetFillAmount() {
        return fillAmount;
    }
}
