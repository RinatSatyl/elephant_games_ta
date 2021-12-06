// Компонент для отображения содежимого ячейки

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTT
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] Text displayText; // Текстовый элемент отображаемый на кнопке. Используеться для отображения "О","Х" или " "

        private PlayingFieldConfigurator.CellState myCellState; // Ссылка на состояние ячейки к которой привязан этот объект

        // Задаёт символ по умолчанию, пустой
        private void Start()
        {
            displayText.text = "";
        }

        // Передает ссылку на ячейку этому объекту
        public void LinkCellState(PlayingFieldConfigurator.CellState cellState)
        {
            myCellState = cellState;
        }

        // Обновляет отображаемый символ на объекте в указаным в состоянии привязанной ячейки 
        public void UpdateState()
        {
            displayText.text = myCellState.ToString(); // Обновить отображаемый символ
        }
    }
}
