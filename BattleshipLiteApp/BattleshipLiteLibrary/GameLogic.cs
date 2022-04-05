using BattleshipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary
{
    public class GameLogic
    {
        public static void InitializeGrid(PlayerInfoModel player)
        {
            List<string> letters = new List<string>()
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> numbers = new List<int>()
            {
                1,
                2,
                3,
                4,
                5
            };

            foreach (string letter in letters)
            {
                foreach (int number in numbers)
                {
                    AddShotGrid(player, letter, number);
                }
            }
        }
        public static bool PlayerStillActive(PlayerInfoModel player)
        {
            bool isActive = false;

            foreach (var ship in player.ShipLocations)
            {
                if (ship.Status != GridSpotStatus.Sunk)
                {
                    isActive = true;
                }
            }

            return isActive;
        }
        private static void AddShotGrid( PlayerInfoModel player, string letter , int number)
        {
            GridSpotModel spot = new GridSpotModel()
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.Empty
           };
            player.ShotGrid.Add(spot);
        }
        public static bool Placeship(PlayerInfoModel model, string location)
        {
            bool output = false;

            (string row, int column) = SplitShotIntoRowAndColumn(location);

            bool isValidLocation = ValidateGridLocation(model, row, column);

            bool isSpotOpen = ValidateShipLocation(model, row, column);

            if (isSpotOpen && isValidLocation)
            {
                model.ShipLocations.Add(new GridSpotModel {SpotLetter = row, SpotNumber = column, Status = GridSpotStatus.Ship });
            }

            return output;
        }
        private static bool ValidateShipLocation(PlayerInfoModel model, string row, int column)
        {
            bool isValidLocation = true;

            foreach (var ship in model.ShipLocations)
            {
                if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    isValidLocation = false;
                }
            }

            return isValidLocation;
        }
        private static bool ValidateGridLocation(PlayerInfoModel model, string row, int column)
        {
            bool isValidLocation = false;

            foreach (var shot in model.ShotGrid)
            {
                if (shot.SpotLetter == row.ToUpper() && shot.SpotNumber == column)
                {
                    isValidLocation = true;
                }
            }

            return isValidLocation;
        }
        public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
        {
            string row = "";
            int column = 0;

            if (shot.Length != 2)
            {
                throw new ArgumentException(" This was an invalid shot type.", " shot"); 
            }

            char[] shotArray = shot.ToCharArray();
            row = shotArray[0].ToString();
            column = int.Parse(shotArray[1].ToString());

            return (row, column);
        }
        public static int GetShotCount(PlayerInfoModel player)
        {
            int ShotCount = 0;

            foreach (var shot in player.ShotGrid)
            {
                if (shot.Status != GridSpotStatus.Empty)
                {
                    ShotCount += 1;
                }
            }

            return ShotCount;
        }
        public static bool ValidateShot(PlayerInfoModel player, string row, int column)
        {
            bool isValidShot = false;

            foreach (var gridSpot in player.ShotGrid)
            {
                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    isValidShot = true;
                }
            }

            return isValidShot;
        }
        public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
        {
            bool isAHit = false;

            foreach (var shot in opponent.ShotGrid)
            {
                if (shot.SpotLetter == row.ToUpper() && shot.SpotNumber == column)
                {
                    isAHit = true;
                }
            }

            return isAHit;
        }
        public static void MarkShotResult(PlayerInfoModel activePlayer, string row, int column, bool isAHit)
        {

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter == row.ToUpper() && gridSpot.SpotNumber == column)
                {
                    if (isAHit)
                    {
                        gridSpot.Status = GridSpotStatus.Hit;
                    }
                    else
                    {
                        gridSpot.Status = GridSpotStatus.Miss;
                    }
                }
            }

        }
    }
}
