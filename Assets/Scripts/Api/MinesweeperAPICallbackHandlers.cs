using System;
using Minesweeper.MVC;
using UnityEngine;
using UnityEngine.Networking;

namespace Minesweeper.API {
    public abstract class CallbackHandler {
        /// <summary>
        /// Whether this handler's web request was successful.
        /// </summary>
        public bool WasSuccessful { get; }

        /// <summary>
        /// The error message (if any) for this handler's web request if it was not successful.
        /// </summary>
        public string ErrorMessage { get; }

        protected CallbackHandler(UnityWebRequest.Result result) {
            switch (result) {
                case UnityWebRequest.Result.Success:
                    WasSuccessful = true;
                    break;
                default:
                    WasSuccessful = false;
                    ErrorMessage = result.ToString();
                    break;
            }
        }
    }

    public class ServerTestCallbackHandler : CallbackHandler {
        public string Message { get; }

        // A class to define the format of the response we get from the server so that we can use
        // JsonUtility to parse
        [Serializable]
        private class ResponseClass {
            public string message = default;
        }

        public ServerTestCallbackHandler(string jsonResponse, UnityWebRequest.Result result) : base(result) {
            if (WasSuccessful) {
                Message = JsonUtility.FromJson<ResponseClass>(jsonResponse).message;
            }
        }
    }

    public class StartGameCallbackHandler : CallbackHandler {
        /// <summary>
        /// A unique identifier used to access further moves for a game instance.
        /// </summary>
        public string GameId { get; }

        // A class to define the format of the response we get from the server so that we can use
        // JsonUtility to parse
        [Serializable]
        private class ResponseClass {
            public string gameId = default;
        }

        public StartGameCallbackHandler(string jsonResponse, UnityWebRequest.Result result) : base(result) {
            if (WasSuccessful) {
                GameId = JsonUtility.FromJson<ResponseClass>(jsonResponse).gameId;
            }
        }
    }

    public class SelectGridPositionCallbackHandler : CallbackHandler {
        public readonly GameBoardModel gameBoardModel = default;
        public readonly GameState gameState;

        // A class to define the format of the response we get from the server so that we can use
        // JsonUtility to parse
        [Serializable]
        private class ResponseClass {
            public GameBoardModel gameInstance = default;
        }

        public SelectGridPositionCallbackHandler(string jsonResponse, int responseCode, UnityWebRequest.Result result) :
            base(result) {
            if (!WasSuccessful) {
                return;
            }

            this.gameBoardModel = JsonUtility.FromJson<ResponseClass>(jsonResponse).gameInstance;

            gameState = responseCode switch {
                200 => GameState.InProgress,
                201 => GameState.Won,
                202 => GameState.Lost,
                _ => gameState
            };
        }
    }
}
