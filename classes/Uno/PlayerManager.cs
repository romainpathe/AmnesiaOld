using System;
using System.Collections.Generic;
using Amnesia.components.Uno;
using Amnesia.interfaces;

namespace Amnesia.classes.Uno
{
    public class PlayerManager: IDrawable
    {
        public readonly List<Player> Players = new List<Player>();
        private int CurrentPlayerIndex { get; set; }
        
        /// <summary>
        /// Returns the current player
        /// </summary>
        /// <returns>Player, CurrentPlayer</returns>
        public Player CurrentPlayer()
        {
            return Players[CurrentPlayerIndex];
        }
        
        /// <summary>
        /// Select random player to start
        /// </summary>
        public void RandomizeFirstPlayer()
        {
            CurrentPlayerIndex = Program.Random.Next(0, Players.Count);
        }

        /// <summary>
        /// Select next player
        /// </summary>
        /// <param name="clockwise">Direction of play</param>
        /// <param name="changeCurrentPlayer">If i need change the current player or not</param>
        /// <returns></returns>
        public Player NextPlayer(bool clockwise, bool changeCurrentPlayer = false)
        {
            var tempPlayerIndex = CurrentPlayerIndex;
            if (clockwise)
            {
                if (CurrentPlayerIndex == Players.Count - 1)
                {
                    tempPlayerIndex = 0;
                }
                else
                {
                    tempPlayerIndex++;
                }
            }
            else
            {
                if (CurrentPlayerIndex == 0)
                {
                    tempPlayerIndex = Players.Count - 1;
                }
                else
                {
                    tempPlayerIndex--;
                }
            }
            if(changeCurrentPlayer) CurrentPlayerIndex = tempPlayerIndex;
            return Players[tempPlayerIndex];
        }
        
        /// <summary>
        /// Select previous player
        /// </summary>
        /// <param name="clockwise">Direction of game</param>
        /// <returns>Player, previous player in game</returns>
        public Player PreviousPlayer(bool clockwise)
        {
            return NextPlayer(!clockwise);
        }
        
        /// <summary>
        /// Add player to game
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayer(Player player)
        {
            Players.Add(player);
            if (player.Name.Length > Player.MaxPlayersNameLength)
            {
                Player.MaxPlayersNameLength = player.Name.Length;
            }
        }
        
        /// <summary>
        /// Check if player name is already taken
        /// </summary>
        /// <param name="name">Nome of player</param>
        /// <returns>boolean</returns>
        public bool PlayerNameExists(string name)
        {
            return Players.Exists(player => player.Name == name);
        }

        /// <summary>
        /// Display waiting player turn in console
        /// </summary>
        public void Draw()
        {
            var text = CurrentPlayer().Name + "'s turn";
            const string text2 = "Press 'Enter' for start your turn";
            var x = Console.WindowWidth / 2 - (text.Length / 2);
            var x2 = Console.WindowWidth / 2 - (text2.Length / 2);
            Console.SetCursorPosition(x, 11);
            Console.Write(text);
            Console.SetCursorPosition(x2, 12);
            Console.Write(text2);
        }

        /// <summary>
        /// Clear waiting player turn in console
        /// </summary>
        /// <param name="full"></param>
        public void Clear(bool full)
        {
            for (var i = 11; i < 13; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }
    }
}