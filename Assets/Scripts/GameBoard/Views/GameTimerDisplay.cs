using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper.MVC {
    public class GameTimerDisplay : MonoBehaviour {
        // The 0-9 sprites to display
        private Sprite[] _digitSprites = default;

        [SerializeField]
        private List<Image> _digitImages = new List<Image>(3);

        private bool _isPaused = false;

        private int _secondsElapsed = default;

        public void Init(GraphicsConfig graphicsConfig) {
            _digitSprites = graphicsConfig.counterNumbers;
            _digitImages.ForEach(image => image.sprite = _digitSprites[0]);
            TimerLoop();
        }

        public void GameOver() {
            _isPaused = true;
        }

        private async void TimerLoop() {
            while (true) {
                // Wait 1 sec
                await Task.Delay(1000);

                if (_isPaused) {
                    return;
                }

                // Add one to our counter, clamping at 999
                _secondsElapsed = Mathf.Clamp(_secondsElapsed + 1, 0, 999);

                // Stop this loop if we've hit max time
                if (_secondsElapsed >= 999) {
                    return;
                }

                // Pad the number with zeroes
                string secondsString = _secondsElapsed.ToString();
                while (secondsString.Length < _digitImages.Count) {
                    secondsString = secondsString.Insert(0, "0");
                }

                for (int i = 0; i < _digitImages.Count; i++) {
                    Image targetImage = _digitImages[i];

                    // There's a chance that this un-canceled async function may run once after the game has reset
                    // so let's check to make sure the image exists for this instance
                    if (!targetImage) {
                        continue;
                    }

                    // We subtract '0' to get the decimal value of the number from the char's ASCII value
                    targetImage.sprite = _digitSprites[secondsString[i] - '0'];
                }
            }
        }
    }
}
