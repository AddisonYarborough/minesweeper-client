using UnityEngine;

namespace Minesweeper.MVC {
    public static class PlayerPrefsUtility {
        private const string _BEST_TIME_KEY = "BEST_TIME";

        /// <summary>
        /// Gets the fastest time (in seconds) that the user has won the game.
        /// </summary>
        /// <returns>The fastest time (in seconds), or 999 if no best score yet</returns>
        public static int GetBestTime() {
            return PlayerPrefs.GetInt(_BEST_TIME_KEY, 999);
        }

        /// <summary>
        /// Sets the fastest time (in seconds) that the user has won the game.
        /// </summary>
        /// <param name="seconds">The number of seconds it took the user to win the game</param>
        public static void SetBestTime(int seconds) {
            PlayerPrefs.SetInt(_BEST_TIME_KEY, seconds);
        }
    }
}
