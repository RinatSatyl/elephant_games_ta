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
        float turnTimeTimer = 0;
        bool activateTimer = false;

        // Заставляет пк думать (делать ход)
        void PC1MakeAMove()
        {
            // вычислить куда ходить
            PCPlayerAI1.MakeMove(firstToStart == 2, playingFieldConfigurator.PlayingFiledReference, out int moveX, out int moveY);
            // сделать ход на вычисленой ячейке
            PlayerPressedOnCell(moveX, moveY);
        }
        void PC2MakeAMove()
        {
            // вычислить куда ходить
            PCPlayerAI2.MakeMove(firstToStart == 2, playingFieldConfigurator.PlayingFiledReference, out int moveX, out int moveY);
            // сделать ход на вычисленой ячейке
            PlayerPressedOnCell(moveX, moveY);
        }
        // переключает ход с одного игрока на другого
        public override void UpdatePlayerTurn()
        {
            if (currentPlayerTurn.name == PCPlayer1.name)
            {
                currentPlayerTurn = PCPlayer2;
                PC1MakeAMove();
            }
            else
            {
                currentPlayerTurn = PCPlayer1;
                PC2MakeAMove();
            }
        }
        // метод для инициации игрового режима
        public override void StartGame(int whoIsFirst, int size)
        {
            base.StartGame(whoIsFirst, size);

            PCPlayerAI1 = new PCAI();
            PCPlayerAI2 = new PCAI();
            PCPlayerAI1.GiveSymbols(PCPlayer1.playingSymbol, PCPlayer2.playingSymbol);
            PCPlayerAI2.GiveSymbols(PCPlayer2.playingSymbol, PCPlayer1.playingSymbol);

            switch (whoIsFirst)
            {
                case 1:
                    currentPlayerTurn = PCPlayer1;
                    break;
                case 2:
                    currentPlayerTurn = PCPlayer2;
                    break;
                default:
                    currentPlayerTurn = PCPlayer1;
                    break;
            }

            for(int x = 0; x < playingFieldConfigurator.PlayingFiledReference.GetLength(0); x++)
                for (int y = 0; y < playingFieldConfigurator.PlayingFiledReference.GetLength(0); y++)
                {
                    playingFieldConfigurator.PlayingFiledReference[x, y].cellObject.gameObject.GetComponent<Button>().interactable = false;
                }

            activateTimer = true;
            turnTimeTimer = turnTime;
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
                    activateTimer = false;
                    UpdatePlayerTurn();
                }
        }

        public override void StopGame()
        {
            Destroy(PCPlayerAI1);
            Destroy(PCPlayerAI2);
            base.StopGame();
        }
    }
}