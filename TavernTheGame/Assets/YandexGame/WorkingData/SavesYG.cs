using UnityEngine;
using System.Collections.Generic;

namespace YG
{
    public class Pair {
        public string key;
        public int value;
        public Pair(string key, int value = 0) {
            this.key = key;
            this.value = value;
        }
    }
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения
        private const int BONUSES_NUMBER = 4; 
        public int bonus_number {
            get { return BONUSES_NUMBER; }
        }

        public int tavernMoney = 100;
        public int tavernBonus = 0;
        public List<string> foodList = new List<string>();
        public Dictionary<string, int> foodMap = new Dictionary<string, int>();
        public Dictionary<string, bool> boughtBonusesMap = new Dictionary<string, bool>();
        public string jsonDictionary = "{}";
        public string jsonBonuses = "{}";

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[1] = true;
        }
    }
}
