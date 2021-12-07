﻿// Компонент для отображения содежимого ячейки и взаимодействия с ней 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTT
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] Text displayText; // Текстовый элемент отображаемый на кнопке. Используеться для отображения "О","Х" или " "
        // позиция данной ячейки в игровом поле
        private int posX;
        private int posY;
        private Button button; // UI кнопка для взаимодействия

        // Задаёт символ по умолчанию, пустой
        private void Start()
        {
            displayText.text = "";
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }
        // Чистка если объект уничтожен
        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

        private void OnClick()
        {
            TheGameManager.Instance.CurrentGameModeRunningReference.PlayerPressedOnCell(posX, posY);
        }

        // Передает этому объекту позицию ячейки на поле
        public void LinkCellState(int x, int y)
        {
            posX = x;
            posY = y;
        }

        // Обновляет отображаемый символ на объекте в указаным в состоянии привязанной ячейки 
        public void UpdateState(string state)
        {
            displayText.text = state; // Обновить отображаемый символ
        }
    }
}
