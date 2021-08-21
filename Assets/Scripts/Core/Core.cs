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

        private void OnEnable() {
            _gameBoardView.onPlayButtonClick += HandleOnPlayButtonClick;
            _gameBoardView.onRetryButtonClick += HandleOnRetryButtonClick;
        }

        private void OnDisable() {
            _gameBoardView.onPlayButtonClick -= HandleOnPlayButtonClick;
            _gameBoardView.onRetryButtonClick -= HandleOnRetryButtonClick;
        }

        private async void Start() {
            _gameBoardView.SetPlayButtonDisplayVisible(false);
            _gameBoardView.SetNoConnectionDisplayVisible(false);

            ServerTestCallbackHandler serverTestResponse = await MinesweeperApi.ServerTestAsync();

            bool isConnected = serverTestResponse.WasSuccessful;

            _gameBoardView.SetNoConnectionDisplayVisible(!isConnected);
            _gameBoardView.SetPlayButtonDisplayVisible(isConnected);
        }

        private void HandleOnPlayButtonClick() {
            _ = new GameRunner(_levelConfig, _graphicsConfig, _gameBoardView);
        }

        private void HandleOnRetryButtonClick() {
            Start();
        }
    }
}
