// Компонент с методами для конфигурацию игрового поля

using UnityEngine;
using UnityEngine.UI;

namespace TTT
{
    public class PlayingFieldConfigurator : MonoBehaviour
    {
        public struct GameCell // Объект ячейки
        {
            public string cellState;
            public Cell cellObject;
        }

        // Размер игрового поля 
        private int playingFieldSize = 0;

        [SerializeField] GridLayoutGroup playingFieldObject; // обьект игрового поля в сцене
        [SerializeField] GameObject cellPrefab; // префаб объекта ячейки для сцены

        private GameCell[,] playingField; // массив игровога поля

        public GameCell[,] PlayingFiledReference { get { return playingField; } } // ссылка на игровое поле
        // ссылки на свойства игрового поля
        public int PlayingFieldSizeReference { get { return playingFieldSize; } }

        // Меняет состояние указанной ячейки на то что указано в методе
        public void SetCellState(string newState, int posX, int posY)
        {
            playingField[posX, posY].cellState = newState; // Задать состояние ячейки
            playingField[posX, posY].cellObject.UpdateState(newState);
        }

        // Генерирует новое игровое поле с указаными размерами
        public void GeneratePlayingField(int size)
        {
            // запомнить размер игрового поля, на всякий
            playingFieldSize = size;

            // Проверить пустое ли игровое поле, если нет, зачистить
            if (playingField != null)
                WipePlayingField();

            // Создать массив с игровым полем с указаными методом размерами
            playingField = new GameCell[size, size];

            // Заполить ячейки массива данными по умолчанию
            for (int y = 0; y < playingField.GetLength(1); y++)
                for (int x = 0; x < playingField.GetLength(0); x++)
                {
                    playingField[x, y].cellState = string.Empty; // Задать состояние ячейки "пустое"
                    playingField[x, y].cellObject = Instantiate(cellPrefab).GetComponent<Cell>(); // Создать Cell объект, добавить на него него ссылку в ячейку
                    playingField[x, y].cellObject.gameObject.transform.SetParent(playingFieldObject.gameObject.transform); // Перемещает созданный объект ячейки на игровое поле
                    playingField[x, y].cellObject.LinkCellState(x, y);  // Передать объекту ссылку на состояние этой ячейки 
                }
        }
        // Сносит игровое поле, удалая все её данные
        public void WipePlayingField()
        {
            // Удалить объект в ячейках
            for (int y = 0; y < playingField.GetLength(1); y++)
                for (int x = 0; x < playingField.GetLength(0); x++)
                    Destroy(playingField[x, y].cellObject);

            // Сделать playingField null
            playingField = null;
            // Сбросить запомненый размер игрового поля
            playingFieldSize = 0;
        }
    }
}