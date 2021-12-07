// Компонент с логикой определения победителя

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public class PlayingFieldChecker : MonoBehaviour
    {
        // возвращает true если все ячейки заполены
        public bool CheckForFieldBeingFilled(PlayingFieldConfigurator.GameCell[,] playingField)
        {
            // проверить заполены ли все ячейки
            for (int x = 0; x < playingField.GetLength(0); x++)
                for (int y = 0; y < playingField.GetLength(1); y++)
                    if (playingField[x, y].cellState == string.Empty)
                        return false; // если нет, вернуть false

            return true; // если да, вернуть true
        }
        // возвращяет true если символ в указаной ячейке подходит под критерии победы
        public bool CheckForWin(string symbolToCheckWith, PlayingFieldConfigurator.GameCell[,] playingField)
        {
            int playingFiledLength = playingField.GetLength(0);
            int cellsDetected = 0; // сколько ячеек с данной символом было замечено
            // string symbolToCheckWith - получить символ на который надо проверять на победителя

            // проверить победу по горизонтали
            //  012
            // 0OOO
            // 1XXX
            // 2OOO
            for (int y = 0; y < playingFiledLength; y++)
            {
                // сбросить счётчик
                cellsDetected = 0;

                // проверить каждую горизонтальную полосу
                for (int x = 0; x < playingFiledLength; x++)
                    if (playingField[x, y].cellState == symbolToCheckWith)
                        cellsDetected++;

                // проверить если количество найденых ячеек с данным символом равняеться горизонтальному размеру поля (победному количеству символов)
                if (cellsDetected == playingFiledLength)
                    return true; // если да, вернуть победу
            }


            // проверить победу по вертикали
            //  012
            // 0OXO
            // 1OXO
            // 2OXO
            for (int x = 0; x < playingFiledLength; x++)
            {
                // сбросить счётчик
                cellsDetected = 0;

                // проверить каждую вертикальную полосу
                for (int y = 0; y < playingFiledLength; y++)
                    if (playingField[x, y].cellState == symbolToCheckWith)
                        cellsDetected++;

                // проверить если количество найденых ячеек с данным символом равняеться горизонтальному размеру поля (победному количеству символов)
                if (cellsDetected == playingFiledLength)
                    return true; // если да, вернуть победу
            }

            // если нет, начать проверку по диагонали

            // сбросить счётчик
            cellsDetected = 0;

            // начать проверку по диагонали с верхне-левого края до нижне-правого
            //  012
            // 0XOO
            // 1OXO
            // 2OOX
            for (int x = 0; x < playingFiledLength; x++)
                if (playingField[x, x].cellState == symbolToCheckWith)
                    cellsDetected++;

            // проверить если количество найденых ячеек с данным символом равняеться горизонтальному размеру поля (победному количеству символов)
            if (cellsDetected == playingFiledLength)
                return true; // если да, вернуть победу

            // если нет, начать проверку с другой стороны диагонали

            // сбросить счётчик
            cellsDetected = 0;

            // начать проверку по диагонали с верхне-правого до нижне-левого
            //  012
            // 0OOX
            // 1OXO
            // 2XOO
            for (int x = 0; x < playingFiledLength; x++)
                if (playingField[((playingFiledLength - 1) - x), x].cellState == symbolToCheckWith)
                    cellsDetected++;

            // проверить если количество найденых ячеек с данным символом равняеться горизонтальному размеру поля (победному количеству символов)
            if (cellsDetected == playingFiledLength)
                return true; // если да, вернуть победу

            // Если не одна проверка не вернула победу, вернуть нет
            return false;
        }
    }
}
