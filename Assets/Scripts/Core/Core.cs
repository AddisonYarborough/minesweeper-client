using Minesweeper.API;
using Minesweeper.MVC;
using UnityEngine;

namespace Minesweeper.Core {
    /// <summary>
    /// The entry point for this application.
    /// </summary>
    public class Core : MonoBehaviour {
        [SerializeField]
        private GameBoardView _gameBoardView = default;

        [SerializeField]
        private LevelConfig _levelConfig = default;

        [SerializeField]
        private GraphicsConfig _graphicsConfig = default;

        private async void Start() {
            _gameBoardView.HidePlayButtonDisplay();
            _gameBoardView.HideNoConnectionDisplay();

            // Call to the server to make sure we can play the game
            ServerTestCallbackHandler serverTestResponse = await MinesweeperApi.ServerTestAsync();

            // Determine whether we're connected to the server
            bool isConnected = serverTestResponse.WasSuccessful;

            // If we're connected, hide the no connection display and show the play button
            if (isConnected) {
                _gameBoardView.ShowPlayButtonDisplay(HandleOnPlayButtonClick);
            }
            else {
                // If we're not, show the no connection display and let the user retry connection
                _gameBoardView.ShowNoConnectionDisplay(HandleOnRetryButtonClick);
            }
        }

        private void HandleOnRetryButtonClick() {
            Start();
        }

        private void HandleOnPlayButtonClick() {
            _gameBoardView.HidePlayButtonDisplay();
            _ = new GameRunner(_levelConfig, _graphicsConfig, _gameBoardView);
        }
    }
}
