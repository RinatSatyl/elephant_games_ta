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

        // Заставляет пк думать (делать ход)
        void PCMakeAMove()
        {
            // вычислить куда ходить
            PCPlayerAI.MakeMove(playingFieldConfigurator.PlayingFiledReference, out int moveX, out int moveY);
            // сделать ход на вычисленой ячейке
            PlayerPressedOnCell(moveX, moveY);
        }
        // переключает ход с одного игрока на другого
        public override void UpdatePlayerTurn()
        {
            if (currentPlayerTurn.name == HumanPlayer.name)
            {
                // Во время хода ПК
                currentPlayerTurn = PCPlayer;
                PCMakeAMove();
                playingFieldConfigurator.DisableButtons(true);
            }
            else
            {
                currentPlayerTurn = HumanPlayer;
                playingFieldConfigurator.DisableButtons(false);
            }
        }
        // метод для инициации игрового режима
        public override void StartGame(int whoIsFirst, int size)
        {
            base.StartGame(whoIsFirst, size);

            PCPlayerAI = new PCAI();

            switch (whoIsFirst)
            {
                case 1:
                    currentPlayerTurn = HumanPlayer;
                    // Пк будет играть на победу/ничью
                    PCPlayerAI.Configure(PCPlayer.playingSymbol, HumanPlayer.playingSymbol, -10, 10, 0);
                    break;
                case 2:
                    currentPlayerTurn = HumanPlayer;
                    // Пк будет играть на ничью, стараясь не победить
                    PCPlayerAI.Configure(PCPlayer.playingSymbol, HumanPlayer.playingSymbol, -20, -10, 30);
                    StartCoroutine(DelayedUpdatePlayerTurn());
                    break;
                default:
                    goto case 1;
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
                    StartCoroutine(DelayedUpdatePlayerTurn());
                }
            }
        }

        IEnumerator DelayedUpdatePlayerTurn()
        {
            yield return new WaitForSeconds(turnTime);
            UpdatePlayerTurn();
        }

        public override void StopGame()
        {
            Destroy(PCPlayerAI);
            base.StopGame();
        }
    }
}