// Класс компонент с логикой для игрового режима Игрок против Игрока

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public class PlayerVsPlayerGameMode : BaseGameMode
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
        private Player player1 = new Player("Player 1", "O");
        private Player player2 = new Player("Player 2", "X");

        // игрок который ходит в данный момент
        private Player currentPlayerTurn;

        // переключает ход с одного игрока на другого
        void UpdatePlayerTurn()
        {
            if (currentPlayerTurn.name == player1.name)
            {
                currentPlayerTurn = player2;
            }
            else
            {
                currentPlayerTurn = player1;
            }
        }
        // метод для инициации игрового режима
        public override void StartGame(int whoIsFirst, int size)
        {
            base.StartGame(whoIsFirst, size);
            switch(whoIsFirst)
            {
                case 1:
                    currentPlayerTurn = player1;
                    break;
                case 2:
                    currentPlayerTurn = player2;
                    break;
                default:
                    currentPlayerTurn = player1;
                    break;
            }
        }
        // метод для обновления состояния игрового режима когда игрок ставит символ
        public override void PlayerPressedOnCell(int x, int y)
        {
            // вставить символ игрока в нажатую ячейку
            playingFieldConfigurator.SetCellState(currentPlayerTurn.playingSymbol, x, y);
            // проверить, победил ли текущий игрок 
            if (playingFieldChecker.CheckForWin(x, y))
            {
                // если да, объявить победу
                TheGameManager.Instance.PlayerWon(currentPlayerTurn.name);
            }
            else 
            {
                // если нет, проверить заполнены ли все ячейки
                if (playingFieldChecker.CheckForFieldBeingFilled())
                {
                    // Если заполнены, объявить ничью, т.к в проверке на победителя выше, он был не выявлен а ходить больше негде
                    TheGameManager.Instance.PlayerWon(currentPlayerTurn.name);
                }
                else 
                    // Если не заполнены, сменить текущего игрока
                    UpdatePlayerTurn();
            }
        }
    }
}