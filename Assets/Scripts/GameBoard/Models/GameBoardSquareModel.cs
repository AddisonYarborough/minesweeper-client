using System;
using UnityEngine;

namespace Minesweeper.MVC {
    /// <summary>
    /// The data model for a single square on the game board grid.
    /// </summary>
    /// <remarks>
    /// The private fields in this class do not use an underscore even though they're private because
    /// the API's fields need to match 1:1 in order to properly deserialize from Json using <see cref="JsonUtility"/>.
    /// </remarks>
    [Serializable]
    public class GameBoardSquareModel {
        /// <summary>
        /// The position of this game board square within the game board grid.
        /// </summary>
        public Vector2Int GridPosition => gridPosition;

        /// <summary>
        /// The state of this <see cref="GameBoardSquareModel"/> within the game board grid.
        /// </summary>
        public GameBoardSquareState State => state;

        [SerializeField]
        private Vector2Int gridPosition;

        [SerializeField]
        private GameBoardSquareState state = default;
    }
}
