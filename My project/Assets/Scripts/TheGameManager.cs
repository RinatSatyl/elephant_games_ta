// Компонент с главной игровой логикой

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTT
{
    public class TheGameManager : MonoBehaviour
    {
        public static TheGameManager Instance; // Статическая ссылка на этот компонент, для лёгкого доступа 

        [SerializeField] GameObject resultScreen;
        [SerializeField] GameObject menuScreen;

        struct GameModeInfo
        {
            public BaseGameMode gameModeObject;
            public string gameModeName;

            public GameModeInfo(BaseGameMode gameModeObject, string gameModeName)
            {
                this.gameModeObject = gameModeObject;
                this.gameModeName = gameModeName;
            }
        }

        private GameModeInfo currentGameMode = new GameModeInfo(new BaseGameMode(), "Пустой"); // текущий игровой режим
        private List<GameModeInfo> gameModes = new List<GameModeInfo>(); // список игровых режимов

        public BaseGameMode CurrentGameModeRunningReference { get { return currentGameMode.gameModeObject; } }

        void Start()
        {
            Instance = this; // Задать ссылку
            // Добавить игровые режимы в список доступных режимов
            GameModeInfo plrVSplr = new GameModeInfo(gameObject.AddComponent(typeof(PlayerVsPlayerGameMode)) as PlayerVsPlayerGameMode, "Игрок против Игрока");
            gameModes.Add(plrVSplr);
            GameModeInfo plrVSpc = new GameModeInfo(gameObject.AddComponent(typeof(PlayerVsPCGameMode)) as PlayerVsPCGameMode, "Игрок против Компьютера");
            gameModes.Add(plrVSpc);
            GameModeInfo pcVSpc = new GameModeInfo(gameObject.AddComponent(typeof(PCVsPCGameMode)) as PCVsPCGameMode, "Компьютер против Компьютера");
            gameModes.Add(pcVSpc);
        }

        public void SetGameMode(string newGameModeState)
        {
            foreach (GameModeInfo gameMode in gameModes)
            {
                if (gameMode.gameModeName == newGameModeState)
                {
                    currentGameMode = gameMode; // поставить новый выбранный режим в текущий
                    return;
                }
            }
        }
        // Метод для запуска текущего игрового режима
        public void StartGame(int playOrder, int playSize)
        {
            currentGameMode.gameModeObject.StartGame(playOrder, playSize); // Запустить текущий игровой режим
        }

        public void PlayerWon(string playerName)
        {
            currentGameMode.gameModeObject.StopGame();
            ToggleResultScreen();
            resultScreen.GetComponentInChildren<Text>(includeInactive: true).text = playerName + " победили!";
        }

        public void PlayerDraw()
        {
            currentGameMode.gameModeObject.StopGame();
            ToggleResultScreen();
            resultScreen.GetComponentInChildren<Text>(includeInactive: true).text = "Ничья!";
        }

        public void ToggleResultScreen()
        {
            resultScreen.SetActive(!resultScreen.activeSelf);
        }

        public void ToggleMenuScreen()
        {
            menuScreen.SetActive(!menuScreen.activeSelf);
        }
    }
}
