// Скрипт главного меню

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTT
{
    public class MainMenuScript : MonoBehaviour
    {
        [SerializeField] Dropdown playingFieldSizeDropdown;
        [SerializeField] Dropdown playOrderDropdown;

        private int playOrder = 1;
        private int playSize = 3;

        public void HumanVsPC()
        {
            TheGameManager.Instance.SetGameMode("Игрок против Компьютера");
            TheGameManager.Instance.StartGame(playOrder, playSize);
            gameObject.SetActive(false);
        }
        
        public void HumanVsHuman()
        {
            TheGameManager.Instance.SetGameMode("Игрок против Игрока");
            TheGameManager.Instance.StartGame(playOrder, playSize);
            gameObject.SetActive(false);
        }

        public void PCvsPC()
        {
            TheGameManager.Instance.SetGameMode("Компьютер против Компьютера");
            TheGameManager.Instance.StartGame(playOrder, playSize);
            gameObject.SetActive(false);
        }

        public void PlayingFieldSizeChange(int value)
        {
            switch (playingFieldSizeDropdown.options[value].text)
            {
                case "3x3":
                    playSize = 3;
                    break;
                case "4x4":
                    playSize = 4;
                    break;
                case "5x5":
                    playSize = 5;
                    break;

            }
        }

        public void PlayOrder(int value)
        {
            switch (playOrderDropdown.options[value].text)
            {
                case "X":
                    playOrder = 1;
                    break;
                case "O":
                    playOrder = 2;
                    break;

            }
        }
    }
}