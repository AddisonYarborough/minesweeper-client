using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Minesweeper.MVC {
    public class GameBoardView : MonoBehaviour {
        /// <summary>
        ///  A collection of all square views on the game board grid.
        /// </summary>
        public List<GameBoardSquareView> AllSquareViews { get; } = new List<GameBoardSquareView>();

        [Header("Game Board Squares"), SerializeField]
        private Transform _gameBoardSquaresParent = default;

        [SerializeField]
        private GameObject _gameBoardSquarePrefab = default;

        [Header("Displays"), SerializeField]
        private ButtonTextDisplay _noConnectionDisplay = default;

        [SerializeField]
        private ButtonTextDisplay _playButtonDisplay = default;

        [SerializeField]
        private SmileyButtonDisplay _smileyButtonDisplay = default;

        [SerializeField]
        private GameTimerDisplay _gameTimerDisplay = default;

        [SerializeField]
        private FlagCounterDisplay _flagCounterDisplay = default;

        private GameBoardController _gameBoardController = default;

        private GraphicsConfig _graphicsConfig = default;

        public void Init(GameBoardController controller, GraphicsConfig graphicsConfig) {
            this._gameBoardController = controller;
            this._graphicsConfig = graphicsConfig;

            // Start listening to smiley button
            _smileyButtonDisplay.onClicked += HandleOnSmileyButtonDisplayClick;

            // The flag counter will start with the total number of bombs on the board, decrement
            // as flags are placed, and increment when they are retrieved
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

        public void ShowPlayButtonDisplay(Action onClick) =>
            _playButtonDisplay.Show($"Best Score: {PlayerPrefsUtility.GetBestTime()}", onClick);

        public void HidePlayButtonDisplay() => _playButtonDisplay.Hide();

        public void ShowNoConnectionDisplay(Action onClick) => _noConnectionDisplay.Show("No Connection", onClick);

        public void HideNoConnectionDisplay() => _noConnectionDisplay.Hide();

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
