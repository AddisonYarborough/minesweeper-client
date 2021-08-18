namespace Minesweeper.MVC {
    /// <summary>
    /// An enum that represents the state of a <see cref="GameBoardSquareModel"/>.
    /// </summary>
    public enum GameBoardSquareState {
        /// <summary>
        /// Blank state. The initial state.
        /// </summary>
        Blank = -1,

        /// <summary>
        /// The associated grid position has no adjacent mines.
        /// </summary>
        Zero = 0,

        /// <summary>
        /// The associated grid position has 1 adjacent mine.
        /// </summary>
        One = 1,

        /// <summary>
        /// The associated grid position has 2 adjacent mines.
        /// </summary>
        Two = 2,

        /// <summary>
        /// The associated grid position has 3 adjacent mines.
        /// </summary>
        Three = 3,

        /// <summary>
        /// The associated grid position has 4 adjacent mines.
        /// </summary>
        Four = 4,

        /// <summary>
        /// The associated grid position has 5 adjacent mines.
        /// </summary>
        Five = 5,

        /// <summary>
        /// The associated grid position has 6 adjacent mines.
        /// </summary>
        Six = 6,

        /// <summary>
        /// The associated grid position has 7 adjacent mines.
        /// </summary>
        Seven = 7,

        /// <summary>
        /// The associated grid position has 8 adjacent mines.
        /// </summary>
        Eight = 8,
    }
}
