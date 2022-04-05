using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;
using System;

namespace BattleshipLite
{
    class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();

            PlayerInfoModel activePlayer = CreatePlayer(" Player 1");
            PlayerInfoModel opponent = CreatePlayer(" Player 2");
            PlayerInfoModel winner = null;

            do
            {
                // Display the grid for activePlayer on where they have fired
                DisplayShotGrid(activePlayer);

                // Ask activePlayer for a shot "B5"
                // Determine if its a valid shot
                // Determine the shot results
                RecordPlayerResult(activePlayer, opponent);

                //Determine if the game is over
                bool isGameOver = GameLogic.PlayerStillActive(opponent);

                // If over, set activePlayer as winner
                // else, swap positions (opponent to activePlayer

                if (isGameOver == true)
                {
                    // I used a Tuple
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {
                    winner = activePlayer;
                }

            } while (winner == null);

            IdentifyWinner(winner);

            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($" Congrats to {winner.UsersName} for winning!");
            Console.WriteLine($" {winner.UsersName} took {GameLogic.GetShotCount(winner) } shots.");
        }
        private static void RecordPlayerResult(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            // Ask for a shot "B4"
            // Determine what row and column that is- split it apart
            // Determine if that is a valid shot
            // Go back to beginning if not a valid shot
            // determine shot results
            // Record results

            bool isValidShot = false;
            string row = "";
            int column = 0;

            do
            {
                string shot = AskForShot();
                (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                isValidShot = GameLogic.ValidateShot(activePlayer, row, column);

                if (isValidShot == false)
                {
                    Console.WriteLine(" Invalid shot location. Please try again later.");
                }

            } while (isValidShot == false);

            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            GameLogic.MarkShotResult(activePlayer, row, column, isAHit); 
        }
        private static string AskForShot()
        {
            string output = "";

            Console.Write(" Please enter your shot selection: ");
            output = Console.ReadLine();

            return output;
        }
        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].SpotLetter;

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }
                else if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($" {gridSpot.SpotNumber}{gridSpot.SpotNumber} ");
                }
                else if(gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X ");
                }
                else if(gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O ");
                }
                else
                {
                    Console.Write(" ? ");
                }
            }
        }
        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            PlayerInfoModel output = new PlayerInfoModel();

            Console.Write($" Information for {playerTitle}");

            // Ask for users name
            output.UsersName = AskForUsersName();

            // Load up the shot grid
            GameLogic.InitializeGrid(output);

            // Ask user for their five ship placement
            PlaceShips(output);

            // Clear
            Console.Clear();

            return output;

        }
        private static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.Write($" Where do you want to place ship number {model.ShipLocations.Count + 1}: ");
                string location = Console.ReadLine();

                bool isValidLocation = GameLogic.Placeship(model, location);

                if (isValidLocation == false)
                {
                    Console.Write(" That was not a valid location. Please try again later.");
                }

            } while (model.ShipLocations.Count < 5);
        }
        private static string AskForUsersName()
        {
            string output = "";

            Console.Write(" What is your name: ");
            output = Console.ReadLine();

            return output;
        }
        private static void WelcomeMessage()
        {
            Console.WriteLine(" Welcome to our Battleship Lite App.");
            Console.WriteLine(" Created and Built by David");
        }
    }
}
