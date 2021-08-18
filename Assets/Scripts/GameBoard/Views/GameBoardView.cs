using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper.MVC {
    public class GameBoardView : MonoBehaviour {
        public event Action onPlayButtonClick = default;
        public event Action onRestartButtonClick = default;

        /// <summary>
        ///  A collection of all square views on the game board grid.
        /// </summary>
        public List<GameBoardSquareView> AllSquareViews { get; } = new List<GameBoardSquareView>();

        [Header("Game Board Squares"), SerializeField]
        private Transform _gameBoardSquaresParent = default;

        [SerializeField]
        private GameObject _gameBoardSquarePrefab = default;

        [Header("Buttons"), SerializeField]
        private Button _playButton = default;

        // If we're not connected to the server, this will allow the user to retry a connection
        [SerializeField]
        private Button _retryButton = default;

        [Header("Displays"), SerializeField]
        private GameObject _noConnectionDisplay = default;

        [SerializeField]
        private GameObject _playButtonDisplay = default;

        [SerializeField]
        private SmileyButtonDisplay _smileyButtonDisplay = default;

        [SerializeField]
        private GameTimerDisplay _gameTimerDisplay = default;

        [SerializeField]
        private FlagCounterDisplay _flagCounterDisplay = default;

        private GameBoardController _gameBoardController = default;

        private GraphicsConfig _graphicsConfig = default;

        private void OnEnable() {
            // Register all difficulty level buttons
            _playButton.onClick.AddListener(HandleOnPlayButtonClick);
            _retryButton.onClick.AddListener(HandleOnRestartButtonClick);
        }

        private void OnDisable() {
            // Unregister buttons
            _playButton.onClick.RemoveAllListeners();
            _retryButton.onClick.RemoveAllListeners();

            // Stop listening to smiley button
            _smileyButtonDisplay.onClicked -= HandleOnSmileyButtonDisplayClick;
        }

        public void Init(GameBoardController controller, GraphicsConfig graphicsConfig) {
            this._gameBoardController = controller;
            this._graphicsConfig = graphicsConfig;

            // Start listening to smiley button
            _smileyButtonDisplay.onClicked += HandleOnSmileyButtonDisplayClick;

            _flagCounterDisplay.Init(_graphicsConfig);
            _gameTimerDisplay.Init(_graphicsConfig);
            _smileyButtonDisplay.Init(_graphicsConfig);
        }

        public void CreateGameBoardGrid(int width, int height) {
            for (int column = 0; column < width; column++) {
                for (int row = 0; row < height; row++) {
                    GameBoardSquareView newSquareView = Instantiate(_gameBoardSquarePrefab, _gameBoardSquaresParent)
                        .GetComponent<GameBoardSquareView>();

                    newSquareView.Init(row, column, _graphicsConfig);
                    _gameBoardController.ListenToPointerEventsForGameBoardSquare(newSquareView);
                    AllSquareViews.Add(newSquareView);
                }
            }
        }

        public void UpdateGameBoardGrid(IEnumerable<GameBoardSquareModel> revealedSquares) {
            foreach (GameBoardSquareModel revealedSquare in revealedSquares) {
                GameBoardSquareView squareView = AllSquareViews.FirstOrDefault(view =>
                    view.GridPosition == revealedSquare.GridPosition);

                squareView.SetNearbyBombs(revealedSquare.State);

                // We no longer need to listen to this square view since it's not clickable
                _gameBoardController.UnlistenToPointerEventsForGameBoardSquare(squareView);
            }
        }

        public void SetAllBlankSquaresToPointerUpState() {
            AllSquareViews.ForEach(square => {
                if (square.State == GameBoardSquareState.Blank && square.IsPointerDown) {
                    square.SetBlank(isPointerDown: false);
                }
            });
        }

        public void GameLost(List<Vector2Int> bombIndices, Vector2Int clickedPosition) {
            foreach (Vector2Int bombIndex in bombIndices) {
                GameBoardSquareView squareView =
                    AllSquareViews.FirstOrDefault(view => view.GridPosition == bombIndex);

                squareView.SetBomb(wasClicked: squareView.GridPosition == clickedPosition);
            }

            _gameTimerDisplay.GameOver();
            _smileyButtonDisplay.SetState(SmileyButtonDisplayState.GameLost);
        }

        public void GameWon() {
            _gameTimerDisplay.GameOver();
            _smileyButtonDisplay.SetState(SmileyButtonDisplayState.GameWon);
        }

        public void SetFlagCounter(int flagCount) {
            _flagCounterDisplay.SetCounter(flagCount);
        }

        private void HandleOnPlayButtonClick() {
            SetPlayButtonDisplayVisible(false);

            onPlayButtonClick?.Invoke();
        }

        private void HandleOnRestartButtonClick() {
            onRestartButtonClick?.Invoke();
        }

        public void SetPlayButtonDisplayVisible(bool state) {
            _playButtonDisplay.SetActive(state);
        }

        public void SetNoConnectionDisplayVisible(bool state) {
            _noConnectionDisplay.SetActive(state);
        }

        public void SetSmileyButtonDisplayState(SmileyButtonDisplayState displayState) {
            if (_gameBoardController.IsGameOver) {
                return;
            }

            _smileyButtonDisplay.SetState(displayState);
        }

        private void HandleOnSmileyButtonDisplayClick() {
            _gameBoardController.RestartGame();
        }
    }
}
