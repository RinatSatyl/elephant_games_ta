// Компонент с логикой AI противника

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public class PCAI : MonoBehaviour
    {
        private string pcSymbol = string.Empty; // символ за который играет AI
        private string playerSymbol = string.Empty; // символ за который играет игрок

        int playerMoveScore = -10;
        int pcMoveScore = 10;
        int tieScore = 0;

        public void GiveSymbols(string pcSymbol, string playerSymbol)
        {
            this.pcSymbol = pcSymbol;
            this.playerSymbol = playerSymbol;
        }

        public void MakeMove(bool goingFirst, PlayingFieldConfigurator.GameCell[,] playingField, out int moveX, out int moveY)
        {
            // Скопировать текущее состояние доски 
            PlayingFieldConfigurator.GameCell[,] playingFieldCopy = playingField.Clone() as PlayingFieldConfigurator.GameCell[,];

            // размер массива игрового поля (вычисляем один раз чтоб при вычислениях не лагало)
            int playingFieldSize = playingField.GetLength(0);

            // первоначальное лучшее очко для хода
            int bestScore = int.MinValue;

            moveX = 0;
            moveY = 0;

            for (int x = 0; x < playingFieldSize; x++)
                for (int y = 0; y < playingFieldSize; y++)
                    if (playingFieldCopy[x, y].cellState == string.Empty)
                    {
                        // поставить символ
                        playingFieldCopy[x, y].cellState = pcSymbol;
                        // вызвать minimax рекурсивно выбрав максимальное значание
                        int currentScore = MiniMax(playingFieldCopy, 3, int.MinValue, int.MaxValue, goingFirst, playingFieldSize);
                        // отменить действие
                        playingFieldCopy[x, y].cellState = string.Empty;
                        // если вычисленный ход имеет очко больше(лучше) чем у лучшего очка (bestScore)
                        if (currentScore > bestScore)
                        {
                            // тогда обновив значение лучшего очка
                            bestScore = currentScore;
                            // сохраним координаты данного движения
                            moveX = x;
                            moveY = y;
                        }
                    }
        }

        // minimax алгоритм (т.к размер игрового поля может быть больше 3 на 3)
        int MiniMax(PlayingFieldConfigurator.GameCell[,] playingField, int depth, int alpha, int beta, bool isMaximizing, int playingFieldSize)
        {
            // проверить остановлена ли игра из-за ничьи или победы
            if (TheGameManager.Instance.CurrentGameModeRunningReference.playingFieldChecker.CheckForWin(pcSymbol, playingField))
                // Если победил пк, вернуть score пк
                return pcMoveScore;
            if (TheGameManager.Instance.CurrentGameModeRunningReference.playingFieldChecker.CheckForWin(playerSymbol, playingField))
                // Если победил игрок, вернуть score игрка
                return playerMoveScore;
            if (TheGameManager.Instance.CurrentGameModeRunningReference.playingFieldChecker.CheckForFieldBeingFilled(playingField))
                // Если победил игрок, вернуть score ничьи
                return tieScore;
            
            if (depth == 0) // Достиг максимум рекурсии, выходим
                return 0;

            if (isMaximizing)
            {
                int bestScore = int.MinValue; // первоначальное лучшее очко

                // Найти пустую ячейку
                for (int x = 0; x < playingFieldSize; x++)
                    for (int y = 0; y < playingFieldSize; y++)
                        if (playingField[x, y].cellState == string.Empty)
                        {
                            // поставить символ
                            playingField[x, y].cellState = pcSymbol;
                            // вызвать minimax рекурсивно 
                            int score = MiniMax(playingField, depth - 1, alpha, beta, false, playingFieldSize);
                            // убрать символ
                            playingField[x, y].cellState = string.Empty;
                            // обновить максимальный score
                            bestScore = Math.Max(bestScore, score);
                            // обновить alpha 
                            alpha = Math.Max(alpha, score);
                            if (beta <= alpha)
                                break;
                        }

                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue; // первоначальное лучшее очко

                // Найти пустую ячейку
                for (int x = 0; x < playingFieldSize; x++)
                    for (int y = 0; y < playingFieldSize; y++)
                        if (playingField[x, y].cellState == string.Empty)
                        {
                            // поставить символ
                            playingField[x, y].cellState = playerSymbol;
                            // вызвать minimax рекурсивно
                            int score = MiniMax(playingField, depth - 1, alpha, beta, true, playingFieldSize);
                            // убрать символ
                            playingField[x, y].cellState = string.Empty;
                            // обновить максимальный score
                            bestScore = Math.Min(bestScore, score);
                            // обновить beta 
                            beta = Math.Min(beta, score);
                            if (beta <= alpha)
                                break;
                        }

                return bestScore;
            }

        }
    }
}
