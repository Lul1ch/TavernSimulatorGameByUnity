using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image bar;
    private float fillAmount = 1f;
    private float multiplier = 1f;
    private Text _textTimer = null;
    private float cookingTime;

    private string textTimer{
        set { if (_textTimer != null) { _textTimer.text = value; } }
        get { return _textTimer.text; }
    }

    private void Update() {
        UpdateFillAmount();
    }

    private void UpdateFillAmount() {
        //Уменьшаем переменную и равняем её проценту заполненности шкалы объекта таймера
        fillAmount -= Time.deltaTime * multiplier;
        bar.fillAmount = fillAmount;
        textTimer = Mathf.Round(bar.fillAmount * cookingTime).ToString();
        //Если таймер кончился, то удаляем его
        if (fillAmount <= 0) {
            textTimer = cookingTime.ToString();
            Destroy(gameObject);
        }
    }

    //С помощью этой функции регулируем время действия таймера, условно устанавливаем его на определённый промежуток времени
    public void SetMultiplier(float cookingTime) {
        multiplier /= cookingTime;
        SaveCookingTime(cookingTime);
    }

    private void SaveCookingTime(float cookingTime) {
        this.cookingTime = cookingTime;
    }

    public float GetCurCookingTime() {
        return bar.fillAmount * cookingTime;
    }

    public float GetFillAmount() {
        return fillAmount;
    }

    public void GetTimerTextObject(string textName = "Time") {
        _textTimer = gameObject.transform.parent.Find(textName).gameObject.GetComponent<Text>();
    }
}
