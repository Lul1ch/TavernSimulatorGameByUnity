using UnityEngine;
using YG;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] private Tavern tavern;
    private void OnEnable() => YandexGame.GetDataEvent += LoadData;
    private void OnDisable() => YandexGame.GetDataEvent -= LoadData;

    private void Start() {
        if (YandexGame.SDKEnabled == true) {
		    LoadData();
        }
    }

    public void LoadData() {
        Debug.Log("YandexGame.LoadData called");
        tavern.tavernMoney = YandexGame.savesData.tavernMoney;
        tavern.tavernBonus = YandexGame.savesData.tavernBonus;
    }
    public void SaveData() {
        Debug.Log("YandexGame.SaveData called");
        YandexGame.savesData.tavernMoney = tavern.tavernMoney;
        YandexGame.savesData.tavernBonus = tavern.tavernBonus;

        YandexGame.SaveProgress();
    }

    public void ResetData() {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }
}
