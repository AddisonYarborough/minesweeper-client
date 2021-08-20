using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Minesweeper.MVC {
    /// <summary>
    /// Manages logic processing / event handling for the board and its squares
    /// and sends visual state updates to views.
    /// </summary>
    public class GameBoardController {
        /// <summary>
        /// Called any time a <see cref="GameBoardSquareView"/> is left-clicked.
        /// </summary>
        public event Action<GameBoardSquareView> onSquareViewLeftClick = default;

        /// <summary>
        /// Whether the game has ended (won or lost).
        /// </summary>
        public bool IsGameOver { get; private set; } = false;

        private readonly GameBoardView _view = default;
        private readonly LevelConfig _levelConfig = default;
        private readonly GraphicsConfig _graphicsConfig = default;
        private int _flagCount = default;

        public GameBoardController(LevelConfig levelConfig, GraphicsConfig graphicsConfig, GameBoardView view) {
            this._view = view;
            this._graphicsConfig = graphicsConfig;
            this._levelConfig = levelConfig;
        }

        public void CreateBoard() {
            _view.Init(this, _graphicsConfig);
            _view.SetFlagCounter(_flagCount = _levelConfig.BombQuantity);
            _view.CreateGameBoardGrid(_levelConfig.Width, _levelConfig.Height);
        }

        public void UpdateBoard(IEnumerable<GameBoardSquareModel> squareModels) {
            _view.UpdateGameBoardGrid(squareModels);
        }

        public void GameLost(GameBoardModel gameBoardModel, Vector2Int clickedPosition) {
            IsGameOver = true;
            _view.GameLost(gameBoardModel.BombIndices, clickedPosition);
            _view.AllSquareViews.ForEach(UnlistenToPointerEventsForGameBoardSquare);
        }

        public void GameWon() {
            IsGameOver = true;
            _view.GameWon();
        }

        public void RestartGame() {
            // HACK: Since we don't store state statically, we can just reload the scene
            // Ideally, we'd want to tear-down our game board and pool the squares - but this is fine for the current
            // scope and complexity
            SceneManager.LoadScene(0);
        }

        public void ListenToPointerEventsForGameBoardSquare(GameBoardSquareView squareView) {
            squareView.onPointerDown += HandleOnGameBoardSquarePointerDown;
            squareView.onPointerUp += HandleOnGameBoardSquarePointerUp;
            squareView.onPointerClick += HandleOnGameBoardSquarePointerClick;
            squareView.onPointerEnter += HandleOnGameBoardSquarePointerEnter;
            squareView.onPointerExit += HandleOnGameBoardSquarePointerExit;
            squareView.onFlagged += HandleOnGameBoardSquareFlagged;
        }

        public void UnlistenToPointerEventsForGameBoardSquare(GameBoardSquareView squareView) {
            squareView.onPointerDown -= HandleOnGameBoardSquarePointerDown;
            squareView.onPointerUp -= HandleOnGameBoardSquarePointerUp;
            squareView.onPointerClick -= HandleOnGameBoardSquarePointerClick;
            squareView.onPointerEnter -= HandleOnGameBoardSquarePointerEnter;
            squareView.onPointerExit -= HandleOnGameBoardSquarePointerExit;
            squareView.onFlagged -= HandleOnGameBoardSquareFlagged;
        }

        private void HandleOnGameBoardSquarePointerDown(GameBoardSquareView squareView, PointerEventData eventData) {
            bool isRightClick = eventData.button == PointerEventData.InputButton.Right;

            if (isRightClick || squareView.IsPointerDown || squareView.IsFlagged) {
                return;
            }

            squareView.SetBlank(isPointerDown: true);
            _view.SetSmileyButtonDisplayState(SmileyButtonDisplayState.SquarePress);
        }

        private void HandleOnGameBoardSquarePointerUp(GameBoardSquareView squareView, PointerEventData eventData) {
            _view.SetSmileyButtonDisplayState(SmileyButtonDisplayState.UpDefault);
            _view.SetAllBlankSquaresToPointerUpState();
        }

        private void HandleOnGameBoardSquarePointerClick(GameBoardSquareView squareView, PointerEventData eventData) {
            _view.SetSmileyButtonDisplayState(SmileyButtonDisplayState.UpDefault);

            bool isRightClick = eventData.button == PointerEventData.InputButton.Right;

            if (!isRightClick) {
                // Don't allow clicks on flagged squares
                if (squareView.IsFlagged) {
                    return;
                }

                onSquareViewLeftClick?.Invoke(squareView);
                squareView.SetBlank(isPointerDown: false);
            }
            else {
                // Ignore if we have no more flags to place if the user is trying to place one
                if (!squareView.IsFlagged && _flagCount < 1) {
                    return;
                }

                squareView.Flag(!squareView.IsFlagged);
            }
        }

        private void HandleOnGameBoardSquarePointerEnter(GameBoardSquareView squareView, PointerEventData eventData) {
            // If the left mouse button isn't down, ignore
            if (!eventData.eligibleForClick) {
                return;
            }

            _view.SetSmileyButtonDisplayState(SmileyButtonDisplayState.SquarePress);

            if (!squareView.IsPointerDown) {
                squareView.SetBlank(isPointerDown: true);
            }
        }

        private void HandleOnGameBoardSquarePointerExit(GameBoardSquareView squareView, PointerEventData eventData) {
            _view.SetSmileyButtonDisplayState(SmileyButtonDisplayState.UpDefault);

            if (squareView.IsPointerDown) {
                squareView.SetBlank(isPointerDown: false);
            }
        }

        private void HandleOnGameBoardSquareFlagged(GameBoardSquareView squareView, bool isFlagged) {
            _flagCount += (isFlagged) ? -1 : 1;
            _view.SetFlagCounter(_flagCount);
        }
    }
}
