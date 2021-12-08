// Базовый класс компонент для игровых режимов с базовой логикой

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public class BaseGameMode : MonoBehaviour
    {
        // ссылки на важные игровые компоненты
        public PlayingFieldConfigurator playingFieldConfigurator;
        public PlayingFieldChecker playingFieldChecker;
        public int firstToStart;

        // метод для инициации игрового режима
        public virtual void StartGame(int whoIsFirst, int size)
        {
            // Найходим игровые компоненты в сцене
            playingFieldConfigurator = FindObjectOfType<PlayingFieldConfigurator>();
            playingFieldChecker = FindObjectOfType<PlayingFieldChecker>();
            // узнать кто ходит первым
            firstToStart = whoIsFirst;
            // Создать игровое поле
            playingFieldConfigurator.GeneratePlayingField(size);
        }
        // метод для обновления состояния игрового режима когда игрок ставит символ
        public virtual void PlayerPressedOnCell(int x, int y)
        {
            
        }

        // метод для очистки/выключения игрового режима
        public virtual void StopGame()
        {
            playingFieldConfigurator.WipePlayingField();
        }

        public virtual void UpdatePlayerTurn()
        {

        }
    }
}