using System;
using Minesweeper.API;
using Minesweeper.MVC;
using UnityEngine;

namespace Minesweeper.Core {
    /// <summary>
    /// The class that is responsible for controlling the game itself.
    /// Starts the game and handles click events by communicating with the API.
    /// </summary>
    public class GameRunner {
        private GameBoardController _controller = default;
        private readonly GameBoardView _view = default;
        private readonly LevelConfig _levelConfig = default;
        private readonly GraphicsConfig _graphicsConfig = default;
        private readonly MinesweeperApi _api = default;
        private string _gameId = default;

        public GameRunner(MinesweeperApi api, LevelConfig levelConfig, GraphicsConfig graphicsConfig,
            GameBoardView view) {
            this._api = api;
            this._levelConfig = levelConfig;
            this._graphicsConfig = graphicsConfig;
            this._view = view;

            StartGame();
        }

        private async void StartGame() {
            // Attempt to start the game with the server and wait for a response
            StartGameCallbackHandler startGameResponse = await _api.StartGameAsync(_levelConfig);

            // If the request was successful, cache the game ID
            if (startGameResponse.WasSuccessful) {
                _gameId = startGameResponse.GameId;
            }
            else {
                _controller.RestartGame();
                return;
            }

            // Create a new game board controller and generate a new board
            _controller = new GameBoardController(_levelConfig, _graphicsConfig, _view);
            _controller.CreateBoard();

            // Listen to the controller's events
            _controller.onSquareViewLeftClick += HandleOnSquareViewLeftClick;
        }

        private async void SelectGridPosition(Vector2Int gridPosition) {
            SelectGridPositionCallbackHandler selectGridPositionResponse =
                await _api.SelectGridPositionAsync(_gameId, gridPosition.x, gridPosition.y);

            if (!selectGridPositionResponse.WasSuccessful) {
                _controller.RestartGame();
                return;
            }

            switch (selectGridPositionResponse.gameState) {
                case GameState.InProgress:
                    _controller.UpdateBoard(selectGridPositionResponse.gameBoardModel.RevealedSquares);
                    break;
                case GameState.Won:
                    _controller.UpdateBoard(selectGridPositionResponse.gameBoardModel.RevealedSquares);
                    _controller.GameWon();
                    break;
                case GameState.Lost:
                    _controller.GameLost(selectGridPositionResponse.gameBoardModel, gridPosition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleOnSquareViewLeftClick(GameBoardSquareView squareView) {
            if (_controller.IsGameOver) {
                return;
            }

            SelectGridPosition(squareView.GridPosition);
        }
    }
}
