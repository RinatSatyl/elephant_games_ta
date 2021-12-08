// Класс компонент с логикой для игрового режима Игрок против Игрока

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTT
{
    public class PCVsPCGameMode : BaseGameMode
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
        private Player PCPlayer1 = new Player("PC 1", "O");
        private Player PCPlayer2 = new Player("PC 2", "X");

        // игрок который ходит в данный момент
        private Player currentPlayerTurn;
        private PCAI PCPlayerAI1;
        private PCAI PCPlayerAI2;

        const float turnTime = 0.3f;

        // Заставляет пк думать (делать ход)
        void PC1MakeAMove()
        {
            // вычислить найлучший ход
            PCPlayerAI1.MakeMove(playingFieldConfigurator.PlayingFiledReference, out int moveX, out int moveY);
            // сделать ход на вычисленой ячейке
            PlayerPressedOnCell(moveX, moveY);
        }
        // Тоже самое что выше, но с объектами второго ПК игрока
        void PC2MakeAMove()
        {
            // вычислить найлучший ход
            PCPlayerAI2.MakeMove(playingFieldConfigurator.PlayingFiledReference, out int moveX, out int moveY);
            // сделать ход на вычисленой ячейке
            PlayerPressedOnCell(moveX, moveY);
        }
        // переключает ход с одного игрока на другого
        public override void UpdatePlayerTurn()
        {
            if (currentPlayerTurn.name == PCPlayer1.name)
            {
                currentPlayerTurn = PCPlayer2;
                PC2MakeAMove();
            }
            else
            {
                currentPlayerTurn = PCPlayer1;
                PC1MakeAMove();
            }
        }
        // метод для инициации игрового режима
        public override void StartGame(int whoIsFirst, int size)
        {
            base.StartGame(whoIsFirst, size);

            PCPlayerAI1 = new PCAI();
            PCPlayerAI2 = new PCAI();

            PCPlayerAI1.Configure(PCPlayer1.playingSymbol, PCPlayer2.playingSymbol, -10, 10, 5);
            PCPlayerAI2.Configure(PCPlayer2.playingSymbol, PCPlayer1.playingSymbol, -10, 10, 5);

            switch (whoIsFirst)
            {
                case 1:
                    currentPlayerTurn = PCPlayer1;
                    break;
                case 2:
                    currentPlayerTurn = PCPlayer2;
                    break;
                default:
                    goto case 1;
            }

            StartCoroutine(DelayedUpdatePlayerTurn());
            playingFieldConfigurator.DisableButtons(false);
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
            Destroy(PCPlayerAI1);
            Destroy(PCPlayerAI2);
            base.StopGame();
        }
    }
}