using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Minesweeper.MVC {
    /// <summary>
    /// A view that is responsible for simple visual updates for each square on the game board grid.
    /// </summary>
    public class GameBoardSquareView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler {
        public event Action<GameBoardSquareView, PointerEventData> onPointerDown = default;
        public event Action<GameBoardSquareView, PointerEventData> onPointerUp = default;
        public event Action<GameBoardSquareView, PointerEventData> onPointerClick = default;
        public event Action<GameBoardSquareView, PointerEventData> onPointerEnter = default;
        public event Action<GameBoardSquareView, PointerEventData> onPointerExit = default;

        public event Action<GameBoardSquareView, bool> onFlagged = default;

        /// <summary>
        /// The current position of this square on the game board grid.
        /// </summary>
        public Vector2Int GridPosition { get; private set; }

        /// <summary>
        /// Whether this square is in a "pointer down" visual state.
        /// </summary>
        public bool IsPointerDown { get; private set; }

        /// <summary>
        /// The state given to this square by the API.
        /// </summary>
        public GameBoardSquareState State { get; private set; } = GameBoardSquareState.Blank;

        /// <summary>
        /// Whether this square is currently flagged as a bomb.
        /// </summary>
        public bool IsFlagged { get; private set; }

        [SerializeField]
        private Image _image = default;

        // The default square with pointer down
        private Sprite _blankDownStateSprite = default;

        // The default square with pointer up
        private Sprite _blankUpStateSprite = default;

        // The bomb state (one that wasn't just clicked)
        private Sprite _bombStateSprite = default;

        // The bomb state (one that was just clicked)
        private Sprite _bombClickedStateSprite = default;

        // The flag state
        private Sprite _flagStateSprite = default;

        // The state when the user misplaced a flag and lost the game
        private Sprite _misplacedFlagStateSprite = default;

        // A collection of all number states (0-8)
        private Sprite[] _numberStateSprites = new Sprite[9];

        public void Init(int xPosition, int yPosition, GraphicsConfig graphicsConfig) {
            this.GridPosition = new Vector2Int(xPosition, yPosition);

            transform.position = new Vector3(xPosition, yPosition, 0);

            SetGraphics(graphicsConfig);

            SetBlank(isPointerDown: false);
        }

        private void SetGraphics(GraphicsConfig graphicsConfig) {
            _blankDownStateSprite = graphicsConfig.squareBlankDown;
            _blankUpStateSprite = graphicsConfig.squareBlankUp;
            _bombStateSprite = graphicsConfig.squareMineDefault;
            _bombClickedStateSprite = graphicsConfig.squareMineClicked;
            _flagStateSprite = graphicsConfig.squareFlag;
            _numberStateSprites = graphicsConfig.squareAdjacentNumbers;
        }

        /// <summary>
        /// Called when the user marks this square as flagged or un-flagged.
        /// </summary>
        /// <param name="state">The flagged state to set this square to</param>
        public void Flag(bool state) {
            IsFlagged = state;

            // Update to our flag sprite
            _image.sprite = IsFlagged ? _flagStateSprite : _blankUpStateSprite;

            onFlagged?.Invoke(this, state);
        }

        /// <summary>
        /// Called when this square was incorrectly marked as a flag after the game is lost
        /// </summary>
        public void SetMisplacedFlag() {
            _image.sprite = _misplacedFlagStateSprite;
        }

        /// <summary>
        /// Called when this square is un-clicked (in a blank state), and the user clicks or moves over it.
        /// </summary>
        /// <param name="isPointerDown">Whether the user is clicking up or down</param>
        public void SetBlank(bool isPointerDown) {
            if (State != GameBoardSquareState.Blank) {
                Debug.LogError($"Cannot set blank state to a square with state: {State}");
                return;
            }

            IsPointerDown = isPointerDown;

            // Update to our blank sprite
            _image.sprite = IsPointerDown ? _blankDownStateSprite : _blankUpStateSprite;
        }

        /// <summary>
        /// Called when this square was a bomb after the game is lost.
        /// </summary>
        /// <param name="wasClicked">Whether this was the bomb the user clicked on to lose the game</param>
        public void SetBomb(bool wasClicked) {
            // Update to our blank sprite
            _image.sprite = wasClicked ? _bombClickedStateSprite : _bombStateSprite;
        }

        /// <summary>
        /// Called when this square is revealed, and not a bomb.
        /// </summary>
        /// <param name="viewState">The number state to set this square to</param>
        public void SetNearbyBombs(GameBoardSquareState viewState) {
            // Ignore if this is a bomb or otherwise not a numbered square
            if ((int)viewState < 0) {
                return;
            }

            // Un-flag this square since it's now visible
            if (IsFlagged) {
                Flag(false);
            }

            // Set our visual display to a 0-9 sprite
            _image.sprite = _numberStateSprites[(int)viewState];
        }

        #region IPointer Implementations

        public void OnPointerDown(PointerEventData eventData) => onPointerDown?.Invoke(this, eventData);

        public void OnPointerUp(PointerEventData eventData) => onPointerUp?.Invoke(this, eventData);

        public void OnPointerClick(PointerEventData eventData) => onPointerClick?.Invoke(this, eventData);

        public void OnPointerEnter(PointerEventData eventData) => onPointerEnter?.Invoke(this, eventData);

        public void OnPointerExit(PointerEventData eventData) => onPointerExit?.Invoke(this, eventData);

        #endregion
    }
}
