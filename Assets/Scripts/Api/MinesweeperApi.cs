using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Minesweeper.Core;
using UnityEngine.Networking;
using Endpoints = Minesweeper.API.MinesweeperAPIEndpoints;

namespace Minesweeper.API {
    /// <summary>
    /// The class that interfaces with the back-end endpoints via <see cref="GameRunner"/>
    /// </summary>
    public class MinesweeperApi {
        public static async Task<ServerTestCallbackHandler> ServerTestAsync() {
            // Create the URI for the GET request
            string uri = Endpoints.GetServerTestUri();

            // Create the request
            using UnityWebRequest request = UnityWebRequest.Get(uri);

            // Wait for the request to hit the server and call back
            await request.SendWebRequest();

            // Create and return a new handler that will process the result
            return new ServerTestCallbackHandler(request.downloadHandler?.text, request.result);
        }

        public static async Task<StartGameCallbackHandler> StartGameAsync(LevelConfig levelConfig) {
            // Create the URI for the GET request
            string uri = Endpoints.GetStartGameUri(levelConfig.Width, levelConfig.Height, levelConfig.BombQuantity);

            // Create the request
            using UnityWebRequest request = UnityWebRequest.Get(uri);

            // Wait for the request to hit the server and call back
            await request.SendWebRequest();

            // Create and return a new handler that will process the result
            return new StartGameCallbackHandler(request.downloadHandler?.text, request.result);
        }

        public static async Task<SelectGridPositionCallbackHandler> SelectGridPositionAsync(string gameId,
            int positionX,
            int positionY) {
            // Create the URI for the GET request
            string uri = Endpoints.GetSelectSquareUri(gameId, positionX, positionY);

            // Create the request
            using UnityWebRequest request = UnityWebRequest.Get(uri);

            // Wait for the request to hit the server and call back
            await request.SendWebRequest();

            // Create and return a new handler that will process the result
            return new SelectGridPositionCallbackHandler(request.downloadHandler?.text, (int)request.responseCode,
                request.result);
        }
    }

    /// <summary>
    /// A class for extending <see cref="UnityWebRequest"/>s to support async/await pattern.
    /// </summary>
    /// <remarks>Thank you to Develax from Unity Answers
    /// (https://answers.unity.com/questions/1655690/weird-problem-with-async-task-and-unitywebrequest.html)</remarks>
    public static class UnityWebRequestExtension {
        public static TaskAwaiter<UnityWebRequest.Result> GetAwaiter(this UnityWebRequestAsyncOperation reqOp) {
            TaskCompletionSource<UnityWebRequest.Result> tsc = new TaskCompletionSource<UnityWebRequest.Result>();
            reqOp.completed += asyncOp => tsc.TrySetResult(reqOp.webRequest.result);

            if (reqOp.isDone) {
                tsc.TrySetResult(reqOp.webRequest.result);
            }

            return tsc.Task.GetAwaiter();
        }
    }
}
