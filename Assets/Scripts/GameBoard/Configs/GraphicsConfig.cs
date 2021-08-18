using UnityEngine;

namespace Minesweeper {
    [CreateAssetMenu(menuName = "New Graphics Config")]
    public class GraphicsConfig : ScriptableObject {
        [Header("Numbers")]
        public Sprite[] counterNumbers = default;

        [Header("Game Board")]
        public Sprite squareBlankUp = default;

        public Sprite squareBlankDown = default;

        public Sprite squareFlag = default;

        public Sprite squareMineDefault = default;

        public Sprite squareMineClicked = default;

        public Sprite[] squareAdjacentNumbers = default;


        [Header("Smiley Button")]
        public Sprite smileyButtonDefaultUp = default;

        public Sprite smileyButtonDefaultDown = default;

        public Sprite smileyButtonSquareClick = default;

        public Sprite smileyButtonWin = default;

        public Sprite smileyButtonLose = default;
    }
}
