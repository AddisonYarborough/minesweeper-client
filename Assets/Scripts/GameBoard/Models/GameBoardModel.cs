using System;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper.MVC {
    /// <summary>
    /// The data model that represents all revealed squares and bomb positions on the game board grid.
    /// </summary>
    /// <remarks>
    /// The private fields in this class do not use an underscore in their name even though they're private because
    /// the API's fields need to match 1:1 in order to properly deserialize from Json using <see cref="JsonUtility"/>.
    /// </remarks>
    [Serializable]
    public class GameBoardModel {
        /// <summary>
        /// A collection of all revealed <see cref="GameBoardSquareModel"/>s on the game board grid.
        /// </summary>
        public List<GameBoardSquareModel> RevealedSquares => revealedSquares;

        /// <summary>
        /// A collection of all X/Y positions of bombs on the game board
        /// </summary>
        public List<Vector2Int> BombIndices => bombIndices;

        [SerializeField]
        private List<GameBoardSquareModel> revealedSquares = default;

        [SerializeField]
        private List<Vector2Int> bombIndices = default;
    }
}
