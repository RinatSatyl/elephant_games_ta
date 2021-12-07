// Компонент с логикой определения победителя

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public class PlayingFieldChecker : MonoBehaviour
    {
        // возвращает true если все ячейки заполены
        public bool CheckForFieldBeingFilled()
        {
            PlayingFieldConfigurator.GameCell[,] gameCells = TheGameManager.Instance.CurrentGameModeRunningReference.playingFieldConfigurator.PlayingFiledReference; // получить игровое поле

            // проверить заполены ли все ячейки
            for (int x = 0; x < gameCells.GetLength(0); x++)
                for (int y = 0; y < gameCells.GetLength(1); y++)
                    if (gameCells[x, y].cellState == string.Empty)
                        return false; // если нет, вернуть false

            return true; // если да, вернуть true
        }
        // возвращяет true если символ в указаной ячейке подходит под критерии победы
        public bool CheckForWin(int lastX, int lastY)
        {
            PlayingFieldConfigurator.GameCell[,] gameCells = TheGameManager.Instance.CurrentGameModeRunningReference.playingFieldConfigurator.PlayingFiledReference; // получить игровое поле
            // получить позицию с коротой надо проверить на победителя
            int startingPointX = lastX; 
            int startingPointY = lastY;
            int cellsDetected = 0; // сколько ячеек с данной символом было замечено
            string symbolToCheckWith = gameCells[startingPointX, startingPointY].cellState; // получить символ на который надо проверять на победителя

            // проверить победу по горизонтали
            //  012
            // 0OOO
            // 1XXX
            // 2OOO
            for (int x = 0; x < gameCells.GetLength(0); x++)
                if (gameCells[x, startingPointY].cellState == symbolToCheckWith)
                    cellsDetected++;

            // проверить если количество найденых ячеек с данным символом равняеться горизонтальному размеру поля (победному количеству символов)
            if (cellsDetected == gameCells.GetLength(0))
                return true; // если да, вернуть победу

            // если нет, начать проверку по диагонали

            // сбросить счётчик
            cellsDetected = 0;

            // начать проверку по диагонали с верхне-левого края до нижне-правого
            //  012
            // 0XOO
            // 1OXO
            // 2OOX
            for (int x = 0; x < gameCells.GetLength(0); x++)
                if (gameCells[x, x].cellState == symbolToCheckWith)
                    cellsDetected++;

            // проверить если количество найденых ячеек с данным символом равняеться горизонтальному размеру поля (победному количеству символов)
            if (cellsDetected == gameCells.GetLength(0))
                return true; // если да, вернуть победу

            // если нет, начать проверку с другой стороны диагонали

            // сбросить счётчик
            cellsDetected = 0;

            // начать проверку по диагонали с верхне-правого до нижне-левого
            //  012
            // 0OOX
            // 1OXO
            // 2XOO
            for (int x = 0; x < gameCells.GetLength(0); x++)
                if (gameCells[((gameCells.GetLength(0) - 1) - x), x].cellState == symbolToCheckWith)
                    cellsDetected++;

            // проверить если количество найденых ячеек с данным символом равняеться горизонтальному размеру поля (победному количеству символов)
            if (cellsDetected == gameCells.GetLength(0))
                return true; // если да, вернуть победу

            // Если не одна проверка не вернула победу, вернуть нет
            return false;
        }
    }
}
