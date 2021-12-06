// Компонент с главной игровой логикой

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance; // Статическая ссылка на этот компонент, для лёгкого доступа 

        public enum GameMode
        {
            Player_vs_AI,
            Player_vs_Player,
            AI_vs_AI
        }

        private GameMode currentGameMode = GameMode.Player_vs_AI;

        void Start()
        {
            Instance = this; // Задать ссылку
        }

        public void SetGameMode(GameMode newGameModeState)
        {
            
        }

        
    }
}
