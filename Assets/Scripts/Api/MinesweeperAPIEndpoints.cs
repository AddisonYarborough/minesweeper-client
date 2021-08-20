namespace Minesweeper.API {
    /// <summary>
    /// A utility class that formats HTTP request URIs for every endpoint in the API.
    /// </summary>
    public static class MinesweeperAPIEndpoints {
        /// <summary>
        /// The root path to query.
        /// </summary>
#if DEVELOPMENT_BUILD
        private const string _ROOT_URL = "http://localhost:3001";
#else
        private const string _ROOT_URL = "https://minesweep-server.herokuapp.com";
#endif
        /// <summary>
        /// Returns an endpoint URI for use with <see cref="MinesweeperApi.ServerTestAsync"/>
        /// </summary>
        /// <returns>A string to be used in a test web request</returns>
        public static string GetServerTestUri() {
            const string serverTestPath = _ROOT_URL + "/serverTest";

            return serverTestPath;
        }

        /// <summary>
        /// Formats the given arguments into a
        /// URI for use with <see cref="MinesweeperApi.StartGameAsync"/>.
        /// </summary>
        /// <param name="width">The width of the game board grid</param>
        /// <param name="height">The height of the game board grid</param>
        /// <param name="bombQuantity">The number of bombs on the game board grid</param>
        /// <returns>A formatted string to be used in a web request</returns>
        public static string GetStartGameUri(int width, int height, int bombQuantity) {
            const string startGameFormat = _ROOT_URL + "/start/{0}/{1}/{2}";

            return string.Format(startGameFormat, width, height, bombQuantity);
        }

        /// <summary>
        /// Formats the given arguments into a
        /// URI for use with <see cref="MinesweeperApi.SelectGridPositionAsync"/>
        /// </summary>
        /// <param name="gameId">The unique ID of the game instance, obtained from
        /// <see cref="MinesweeperApi.StartGameAsync"/></param>
        /// <param name="gridPositionX">The X grid position to select</param>
        /// <param name="gridPositionY">The Y grid position to select</param>
        /// <returns>A formatted string to be used in a web request</returns>
        public static string GetSelectSquareUri(string gameId, int gridPositionX, int gridPositionY) {
            const string selectGameFormat = _ROOT_URL + "/select/{0}/{1}/{2}";

            return string.Format(selectGameFormat, gameId, gridPositionX, gridPositionY);
        }
    }
}
