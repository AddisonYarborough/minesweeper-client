namespace Minesweeper {
    /// <summary>
    /// A collection of all visual states that <see cref="SmileyButtonDisplay"/> may have.
    /// </summary>
    public enum SmileyButtonDisplayState {
        /// <summary>
        /// Uninitialized state.
        /// </summary>
        None = 0,

        /// <summary>
        /// Button state when not being pressed.
        /// </summary>
        DownDefault = 1,

        /// <summary>
        /// Button state when it is being pressed.
        /// </summary>
        UpDefault = 2,

        /// <summary>
        /// Button state when a square on the board is being pressed (left-clicked).
        /// </summary>
        SquarePress = 3,

        /// <summary>
        /// Button state when the game is won.
        /// </summary>
        GameWon = 4,

        /// <summary>
        /// Button state when the game is lost.
        /// </summary>
        GameLost = 5,
    }
}
