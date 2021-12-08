// Класс компонент с логикой для игрового режима Игрок против Компьютера

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
        private Player player1 = new Player("Игрок 1", "O");
        private Player player2 = new Player("Игрок 2", "X");

        // игрок который ходит в данный момент
        private Player currentPlayerTurn;

        // переключает ход с одного игрока на другого
        public override void UpdatePlayerTurn()
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
            // Вызвать базовую, общую логику старта
            base.StartGame(whoIsFirst, size);
            // установить первого игрока на место 
            switch (whoIsFirst)
            {
                case 1:
                    player1.playingSymbol = "X";
                    player2.playingSymbol = "O";
                    currentPlayerTurn = player1;
                    break;
                case 2:
                    player1.playingSymbol = "O";
                    player2.playingSymbol = "X";
                    currentPlayerTurn = player2;
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
                TheGameManager.Instance.PlayerWon(currentPlayerTurn.playingSymbol);
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
                    // Если не заполнены, сменить текущего игрока
                    UpdatePlayerTurn();
            }
        }
    }
}