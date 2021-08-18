namespace Minesweeper.API {
    public enum GameState {
        /// <summary>
        /// The game hasn't been started yet.
        /// </summary>
        None = 0,

        /// <summary>
        /// There is a game in progress.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// The game is over and the user won.
        /// </summary>
        Won = 2,

        /// <summary>
        /// The game is over and user lost.
        /// </summary>
        Lost = 3,
    }
}
