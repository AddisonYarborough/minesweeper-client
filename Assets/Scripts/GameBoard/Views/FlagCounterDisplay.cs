using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper.MVC {
    public class FlagCounterDisplay : MonoBehaviour {
        // The 0-9 sprites to display
        private Sprite[] _digitSprites = default;

        [SerializeField]
        private List<Image> _digitImages = new List<Image>(3);

        public void Init(GraphicsConfig graphicsConfig) {
            // Set all digits to zero
            _digitSprites = graphicsConfig.counterNumbers;
            _digitImages.ForEach(image => image.sprite = _digitSprites[0]);
        }

        /// <summary>
        /// Sets the counter to the given number.
        /// </summary>
        /// <param name="flagCount">The number of flags used</param>
        public void SetCounter(int flagCount) {
            // Pad the number with zeroes
            string flagsString = flagCount.ToString();
            while (flagsString.Length < _digitImages.Count) {
                flagsString = flagsString.Insert(0, "0");
            }

            for (int i = 0; i < _digitImages.Count; i++) {
                // We subtract '0' to get the decimal value of the number from the char's ASCII value
                _digitImages[i].sprite = _digitSprites[flagsString[i] - '0'];
            }
        }
    }
}
