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
        private Player PCPlayer1 = new Player("ПК 1", "O");
        private Player PCPlayer2 = new Player("ПК 2", "X");

        // игрок который ходит в данный момент
        private Player currentPlayerTurn;

        // объекты алгоритма пк игроков
        private PCAI PCPlayerAI1;
        private PCAI PCPlayerAI2;

        // задержка между ходами
        const float turnTime = 0.3f;

        // Заставляет пк 1 думать (делать ход)
        void PC1MakeAMove()
        {
            // вычислить найлучший ход
            PCPlayerAI1.MakeMove(playingFieldConfigurator.PlayingFiledReference, out int moveX, out int moveY);
            // сделать ход на вычисленой ячейке
            PlayerPressedOnCell(moveX, moveY);
        }
        // Тоже самое что выше, но с пк 2
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
            // Вызвать базовую, общую логику старта
            base.StartGame(whoIsFirst, size);

            // создать объекты пк алгоритма
            PCPlayerAI1 = new PCAI();
            PCPlayerAI2 = new PCAI();
            // установить первого игрока на место 
            switch (whoIsFirst)
            {
                case 1:
                    PCPlayer1.playingSymbol = "X";
                    PCPlayer2.playingSymbol = "O";
                    currentPlayerTurn = PCPlayer1;
                    break;
                case 2:
                    PCPlayer1.playingSymbol = "O";
                    PCPlayer2.playingSymbol = "X";
                    currentPlayerTurn = PCPlayer2;
                    break;
                default:
                    goto case 1;
            }

            // задать настройки - обычная игра, оптимальная тактика
            PCPlayerAI1.Configure(PCPlayer1.playingSymbol, PCPlayer2.playingSymbol, -10, 10, 5);
            PCPlayerAI2.Configure(PCPlayer2.playingSymbol, PCPlayer1.playingSymbol, -10, 10, 5);

            // заставить пк игрока ходить
            StartCoroutine(DelayedUpdatePlayerTurn());
            // отключить UI кнопки чтоб игрок не мог на них тыкнуть
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
                {
                    // Если не заполнены, сменить текущего игрока
                    StartCoroutine(DelayedUpdatePlayerTurn());
                }
            }
        }
        // запускает смену игрока с задержкой
        IEnumerator DelayedUpdatePlayerTurn()
        {
            yield return new WaitForSeconds(turnTime);
            UpdatePlayerTurn();
        }
        // останавливает игру, выключает алгоритмы
        public override void StopGame()
        {
            Destroy(PCPlayerAI1);
            Destroy(PCPlayerAI2);
            base.StopGame();
        }
    }
}