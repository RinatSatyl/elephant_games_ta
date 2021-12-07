// Класс компонент с логикой для игрового режима Игрок против Игрока

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTT
{
    public class PlayerVsPCGameMode : BaseGameMode
    {
        struct Player // стракт игрока
        {
            public string name;
            public string playingSymbol;

            public Player(string name, string playingSymbol)
            {
                this.name = name;
                this.playingSymbol = playingSymbol;
            }
        }

        // игроки
        private Player HumanPlayer = new Player("Human", "O");
        private Player PCPlayer = new Player("PC", "X");

        // игрок который ходит в данный момент
        private Player currentPlayerTurn;
        private PCAI PCPlayerAI;

        const float turnTime = 0.3f;
        float turnTimeTimer = 0;
        bool activateTimer = false;

        // Заставляет пк думать (делать ход)
        void PCMakeAMove()
        {
            // вычислить куда ходить
            PCPlayerAI.MakeMove(false, playingFieldConfigurator.PlayingFiledReference, out int moveX, out int moveY);
            // сделать ход на вычисленой ячейке
            PlayerPressedOnCell(moveX, moveY);
            DisableButtons(true);
        }
        // переключает ход с одного игрока на другого
        public override void UpdatePlayerTurn()
        {
            if (currentPlayerTurn.name == HumanPlayer.name)
            {
                // Во время хода ПК
                currentPlayerTurn = PCPlayer;
                PCMakeAMove();
            }
            else
            {
                currentPlayerTurn = HumanPlayer;
            }
        }
        // метод для инициации игрового режима
        public override void StartGame(int whoIsFirst, int size)
        {
            base.StartGame(whoIsFirst, size);

            PCPlayerAI = new PCAI();
            PCPlayerAI.GiveSymbols(PCPlayer.playingSymbol, HumanPlayer.playingSymbol);

            switch (whoIsFirst)
            {
                case 1:
                    currentPlayerTurn = HumanPlayer;
                    break;
                case 2:
                    currentPlayerTurn = HumanPlayer;
                    activateTimer = true;
                    turnTimeTimer = turnTime;
                    DisableButtons(true);
                    break;
                default:
                    currentPlayerTurn = HumanPlayer;
                    break;
            }
        }
        // метод для обновления состояния игрового режима когда игрок ставит символ
        public override void PlayerPressedOnCell(int x, int y)
        {
            // вставить символ игрока в нажатую ячейку
            playingFieldConfigurator.SetCellState(currentPlayerTurn.playingSymbol, x, y);
            // проверить, победил ли текущий игрок 
            if (playingFieldChecker.CheckForWin(currentPlayerTurn.playingSymbol, playingFieldConfigurator.PlayingFiledReference))
            {
                // если да, объявить победу
                TheGameManager.Instance.PlayerWon(currentPlayerTurn.name);
            }
            else 
            {
                // если нет, проверить заполнены ли все ячейки
                if (playingFieldChecker.CheckForFieldBeingFilled(playingFieldConfigurator.PlayingFiledReference))
                {
                    // Если заполнены, объявить ничью, т.к в проверке на победителя выше, он был не выявлен а ходить больше негде
                    TheGameManager.Instance.PlayerDraw();
                }
                else
                {
                    // Если не заполнены, сменить текущего игрока
                    activateTimer = true;
                    turnTimeTimer = turnTime;
                }
            }
        }

        public override void UpdateMe()
        {
            if (activateTimer)
                if (turnTimeTimer > 0)
                {
                    turnTimeTimer -= Time.deltaTime;
                }
                else
                {
                    DisableButtons(false);
                    activateTimer = false;
                    UpdatePlayerTurn();
                }
        }

        void DisableButtons(bool value)
        {
            if (playingFieldConfigurator.PlayingFiledReference != null)
                for (int x = 0; x < playingFieldConfigurator.PlayingFiledReference.GetLength(0); x++)
                    for (int y = 0; y < playingFieldConfigurator.PlayingFiledReference.GetLength(0); y++)
                    {
                        playingFieldConfigurator.PlayingFiledReference[x, y].cellObject.gameObject.GetComponent<Button>().interactable = !value;
                    }
        }

        public override void StopGame()
        {
            Destroy(PCPlayerAI);
            base.StopGame();
        }
    }
}