// Компонент с логикой AI противника

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public class PCAI : MonoBehaviour
    {
        private string myPlayingSymbol = string.Empty; // символ за который играет AI
        private string opponentPlayingSymbol = string.Empty; // символ за который играет игрок

        int opponentWinScore = -10; // призваемое алгоритму значение если вычисленый результат привёл к победе противника
        int myWinScore = 10; // призваемое алгоритму значение если вычисленый результат привёл к победе алгоритма
        int tieScore = 0; // призваемое алгоритму значение если вычисленый результат привёл к ничьей
        int foresightDepth = 0; // На сколько ходов вперёд ПК может смотреть. Чем больше игровое поле, чем меньше значение

        public void Configure(string myPlayingSymbol, string opponentPlayingSymbol, int opponentWinScore, int myWinScore, int tieScore)
        {
            // Перенять за какой символ против какого играет алгоритм
            this.myPlayingSymbol = myPlayingSymbol;
            this.opponentPlayingSymbol = opponentPlayingSymbol;

            // Перенять значения настроект алгоритма, какие исходы ставить в приоретет
            this.opponentWinScore = opponentWinScore;
            this.myWinScore = myWinScore;
            this.tieScore = tieScore;

            // назначить оптимальную грубину алгоритма в зависимости от размера игрового поля
            // p.s больше 4х сильно увиличивает время расчёта хода на поле 5х5
            switch (TheGameManager.Instance.CurrentGameModeRunningReference.playingFieldConfigurator.PlayingFieldSizeReference)
            {
                case 3:
                    foresightDepth = 6;
                    break;
                case 4:
                    foresightDepth = 6;
                    break;
                case 5:
                    foresightDepth = 4;
                    break;
                default:
                    goto case 3;
            }
        }

        public void MakeMove(PlayingFieldConfigurator.GameCell[,] playingField, out int moveX, out int moveY)
        {
            // Скопировать текущее состояние доски 
            PlayingFieldConfigurator.GameCell[,] playingFieldCopy = playingField.Clone() as PlayingFieldConfigurator.GameCell[,];

            // размер массива игрового поля (вычисляем один раз чтоб при вычислениях не лагало)
            int playingFieldSize = playingField.GetLength(0);

            moveX = 0;
            moveY = 0;

            // первоначальное лучшее очко для хода
            int bestScore = int.MinValue;

            for (int x = 0; x < playingFieldSize; x++)
                for (int y = 0; y < playingFieldSize; y++)
                    if (playingFieldCopy[x, y].cellState == string.Empty)
                    {
                        // поставить символ
                        playingFieldCopy[x, y].cellState = myPlayingSymbol;
                        // вызвать minimax рекурсивно выбрав максимальное значение рекурский     
                        int currentScore = MiniMax(playingFieldCopy, foresightDepth, int.MinValue, int.MaxValue, false, playingFieldSize);
                        // отменить сделаный ход
                        playingFieldCopy[x, y].cellState = string.Empty;
                        // если вычисленный ход лучше чем у bestScore
                        if (currentScore > bestScore)
                        {
                            // тогда обновив значение лучшего значения очка
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
            if (TheGameManager.Instance.CurrentGameModeRunningReference.playingFieldChecker.CheckForWin(myPlayingSymbol, playingField))
                // Если победил "я", вернуть успешное очко
                return myWinScore;
            if (TheGameManager.Instance.CurrentGameModeRunningReference.playingFieldChecker.CheckForWin(opponentPlayingSymbol, playingField))
                // Если победил "противник", вернуть не успешное очко
                return opponentWinScore;
            if (TheGameManager.Instance.CurrentGameModeRunningReference.playingFieldChecker.CheckForFieldBeingFilled(playingField))
                // Если произошла ничья, вернуть очко за ничью
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
                            playingField[x, y].cellState = myPlayingSymbol;
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
                            playingField[x, y].cellState = opponentPlayingSymbol;
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
