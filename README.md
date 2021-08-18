# minesweeper-client

## Installation

- This project was created using Unity 2021.3.15f2. You can install that by downloading the [Unity Hub](https://unity3d.com/get-unity/download)
- Once installed, open the project and navigate to the "Main" scene in the Scenes folder
- Press play! By default, this project will hit a remote server with the hosted server code. To host and use that code:
  - Download the code from the [repository](https://github.com/Addyarb/minesweeper-server)
  - cd to the root folder of the server code in the terminal/command prompt
  - use `npm run dev` to start the server
  
  ## Overview
  This is a [Minesweeper](https://en.wikipedia.org/wiki/Minesweeper_(video_game)) inspired game that uses a [node.js](https://nodejs.org/en/) / [express.js](https://expressjs.com/) server
  to perform all of the game logic.
  
  It's important to note that this is a client that doesn't perform any game logic. All game logic is performed on the server and passed back to the client to present. The general flow is as follows:
  - Call upon the server to check if it's available
  - If not, show a "No Connection" panel with a "Retry" button
  - If so, show a "Play" button
  - When the "Play" button is clicked, the client will send a "start" web request with the width, height, and mine count
  - Each time the user presses a new square, they will send a "select" web request with the X and Y position
  - After the web request is sent, the user will receive a callback with the new squares to reveal, as well as their state (0-8)
  - If during selection the game is won or lost, the server will respond with a 202 or 203 respectively - and the client will respond accordingly
  - Pressing the smiley face button at the top will reset the client
  
  ## Structure
  This client follows a fairly straight-forward MVC/MVP architecture, where there are models for what the server passes back in its response.
  
  The main classes to know about are:
  - `Core` - The entry-point for this application (from a MonoBehaviour lifecycle standpoint)
  - `GameBoardModel` - A model that mirrors the "select" route's response and contains all squares to reveal, as well as all mine positions on the baord
  - `GameBoardController` - A logic-only class that passes model updates to the view
  - `GameBoardView` - A MonoBehaviour that listens to the GameBoardController and performs visual updates to the board, as well as listens to user input
  - `MinesweeperAPI` - The class that allows the app to interface with the server via web requests
