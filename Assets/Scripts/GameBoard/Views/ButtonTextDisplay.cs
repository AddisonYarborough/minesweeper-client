using System;
using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper.MVC {
    /// <summary>
    /// A simple component that displays a title along with a button.
    /// </summary>
    public class ButtonTextDisplay : MonoBehaviour {
        [SerializeField]
        private GameObject _rootGameObject = default;

        [SerializeField]
        private Text _titleText = default;

        [SerializeField]
        private Button _button = default;

        /// <summary>
        /// Shows this display with the given text and onClick action.
        /// </summary>
        /// <param name="titleText">The text to display alongside the button</param>
        /// <param name="onClick">The action to perform when this displays' button is clicked</param>
        public void Show(string titleText, Action onClick) {
            _titleText.text = titleText;
            _button.onClick.AddListener(() => onClick?.Invoke());
            _rootGameObject.SetActive(true);
        }

        /// <summary>
        /// Hides this display, resets its text and removes any onClick actions from the button.
        /// </summary>
        public void Hide() {
            _rootGameObject.SetActive(false);
            _titleText.text = string.Empty;
            _button.onClick.RemoveAllListeners();
        }
    }
}
