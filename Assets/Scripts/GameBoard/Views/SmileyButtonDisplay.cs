using System;
using Minesweeper;
using UnityEngine;
using UnityEngine.UI;

public class SmileyButtonDisplay : MonoBehaviour {
    public event Action onClicked = default;

    [SerializeField]
    private Button _button = default;

    private Sprite _downDefaultSprite = default;

    private Sprite _upDefaultSprite = default;

    private Sprite _squarePressSprite = default;

    private Sprite _gameWinSprite = default;

    private Sprite _gameOverSprite = default;

    private void OnEnable() {
        _button.onClick.AddListener(() => onClicked?.Invoke());
    }

    private void OnDisable() {
        _button.onClick.RemoveAllListeners();
    }

    public void Init(GraphicsConfig graphicsConfig) {
        SpriteState spriteState = _button.spriteState;
        spriteState.pressedSprite = graphicsConfig.smileyButtonDefaultDown;
        _button.spriteState = spriteState;
        _downDefaultSprite = graphicsConfig.smileyButtonDefaultDown;
        _upDefaultSprite = graphicsConfig.smileyButtonDefaultUp;
        _squarePressSprite = graphicsConfig.smileyButtonSquareClick;
        _gameWinSprite = graphicsConfig.smileyButtonWin;
        _gameOverSprite = graphicsConfig.smileyButtonLose;
    }

    /// <summary>
    /// Sets the visual state of this view.
    /// </summary>
    /// <param name="displayState">The target visual state</param>
    public void SetState(SmileyButtonDisplayState displayState) {
        _button.image.sprite = displayState switch {
            SmileyButtonDisplayState.None => null,
            SmileyButtonDisplayState.DownDefault => _downDefaultSprite,
            SmileyButtonDisplayState.UpDefault => _upDefaultSprite,
            SmileyButtonDisplayState.SquarePress => _squarePressSprite,
            SmileyButtonDisplayState.GameWon => _gameWinSprite,
            SmileyButtonDisplayState.GameLost => _gameOverSprite,
            _ => throw new ArgumentOutOfRangeException(nameof(displayState), displayState, null)
        };
    }
}
