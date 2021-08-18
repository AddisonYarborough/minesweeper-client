using Minesweeper.API;
using Minesweeper.MVC;
using UnityEngine;

namespace Minesweeper.Core {
    /// <summary>
    /// The entry point for this application.
    /// </summary>
    public class Core : MonoBehaviour {
        private bool _IsConnected { get; set; } = false;

        private readonly MinesweeperApi _api = new MinesweeperApi();

        [SerializeField]
        private GameBoardView _gameBoardView = default;

        [SerializeField]
        private LevelConfig _levelConfig = default;

        [SerializeField]
        private GraphicsConfig _graphicsConfig = default;

        private void OnEnable() {
            _gameBoardView.onPlayButtonClick += HandleOnPlayButtonClick;
            _gameBoardView.onRestartButtonClick += HandleOnRestartButtonClick;
        }

        private void OnDisable() {
            _gameBoardView.onPlayButtonClick -= HandleOnPlayButtonClick;
            _gameBoardView.onRestartButtonClick -= HandleOnRestartButtonClick;
        }

        private async void Start() {
            _gameBoardView.SetPlayButtonDisplayVisible(false);
            _gameBoardView.SetNoConnectionDisplayVisible(false);

            ServerTestCallbackHandler serverTestResponse = await _api.ServerTestAsync();

            _IsConnected = serverTestResponse.WasSuccessful;

            _gameBoardView.SetNoConnectionDisplayVisible(!_IsConnected);
            _gameBoardView.SetPlayButtonDisplayVisible(_IsConnected);
        }

        private void HandleOnPlayButtonClick() {
            _ = new GameRunner(_api, _levelConfig, _graphicsConfig, _gameBoardView);
        }

        private void HandleOnRestartButtonClick() {
            Start();
        }
    }
}
