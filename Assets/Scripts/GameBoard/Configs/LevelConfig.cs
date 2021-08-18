using UnityEngine;

namespace Minesweeper {
    [CreateAssetMenu(menuName = "New Level Config")]
    public class LevelConfig : ScriptableObject {
        /// <summary>
        /// The width of the game board grid.
        /// </summary>
        public int Width => _width;

        /// <summary>
        /// The height of the game board grid.
        /// </summary>
        public int Height => _height;

        /// <summary>
        /// The number of bombs on the game board.
        /// </summary>
        public int BombQuantity => _bombQuantity;

        [SerializeField]
        private int _width = 10;

        [SerializeField]
        private int _height = 10;

        [SerializeField]
        private int _bombQuantity = 5;
    }
}
