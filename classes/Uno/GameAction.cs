using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Amnesia.components.Uno;

namespace Amnesia.classes.Uno
{
    public static class GameAction
    {
        /// <summary>
        /// When you click on UpArrow, this method is called.
        /// </summary>
        /// <param name="gameManager">Manager of the game</param>
        public static void UpArrow(GameManager gameManager)
        {
            if(gameManager.WaitPlayer) return;
            var currentPlayer = gameManager.PlayerManager.CurrentPlayer();
            if (currentPlayer.SelectedDeck) return;
            if(currentPlayer.ColorSelector) return;
            currentPlayer.SelectedDeck = true;
            currentPlayer.Hand.First(card => card.IsSelected).IsSelected = false;
            currentPlayer.DrawHand();
        }
        
        /// <summary>
        /// When you click on DownArrow, this method is called.
        /// </summary>
        /// <param name="gameManager">Manager of the game</param>
        public static void DownArrow(GameManager gameManager)
        {
            if(gameManager.WaitPlayer) return;
            var currentPlayer = gameManager.PlayerManager.CurrentPlayer();
            if(!currentPlayer.SelectedDeck) return;
            if(currentPlayer.ColorSelector) return;
            currentPlayer.SelectedDeck = false;
            if(currentPlayer.LastCardSelected == -1) currentPlayer.LastCardSelected = 0;
            var fCard  = currentPlayer.Hand[currentPlayer.LastCardSelected];
            if (fCard != null) fCard.IsSelected = true;
            currentPlayer.DrawHand();
        }
        
        /// <summary>
        /// When you click on LeftArrow, this method is called.
        /// </summary>
        /// <param name="gameManager">Manager of the game</param>
        public static void LeftArrow(GameManager gameManager)
        {
            if(gameManager.WaitPlayer) return;
            var currentPlayer = gameManager.PlayerManager.CurrentPlayer();
            if (currentPlayer.ColorSelector)
            {
                ColorSelector.SelectPrevious();
            }
            else
            {
                currentPlayer.SelectPrevious();
                currentPlayer.DrawHand();
            }
        }
        
        /// <summary>
        /// When you click on RightArrow, this method is called.
        /// </summary>
        /// <param name="gameManager">Manager of the game</param>
        public static void RightArrow(GameManager gameManager)
        {
            if(gameManager.WaitPlayer) return;
            var currentPlayer = gameManager.PlayerManager.CurrentPlayer();
            if (currentPlayer.ColorSelector)
            {
                ColorSelector.SelectNext();
                
            }
            else
            {
                currentPlayer.SelectNext();
                currentPlayer.DrawHand();
            }
        }
        
        /// <summary>
        /// When you click on Enter, this method is called.
        /// </summary>
        /// <param name="gameManager">Manager of the game</param>
        public static void Enter(GameManager gameManager)
        {
            var currentPlayer = gameManager.PlayerManager.CurrentPlayer();
            if (gameManager.WaitPlayer) // If we wait a player start this turn
            {
                gameManager.WaitPlayer = false;
                Writer.Clear(gameManager.PlayerManager);
                currentPlayer.Turn(gameManager);
                return;
            }
            if (currentPlayer.SelectedDeck) // If the player select the deck
            {
                currentPlayer.AddCardToHand(gameManager.Picking());
            }
            else if (currentPlayer.ColorSelector) // If the player has played a card or has to choose the colour
            {
                gameManager.CurrentCard.Color = ColorSelector.CurrentColor();
                Writer.ObjForWrite.Add(gameManager.CurrentCard);
                Writer.ObjForClear.Add(gameManager.ColorSelector);
                currentPlayer.ColorSelector = false;
                currentPlayer.SelectedDeck = true;
            }
            else // Other case
            {
                var canPlay = currentPlayer.CanPlayCurrentCard(gameManager);
                if (!canPlay) return;
                var card = currentPlayer.PlayCard(true);
                currentPlayer.LastCardSelected = currentPlayer.Hand.IndexOf(currentPlayer.Hand.FirstOrDefault(tmpCard => tmpCard.IsSelected));
                gameManager.AddDiscardPile(card);
                switch (card.Value)
                {
                    case "Draw Two":
                    {
                        var nextPlayer = gameManager.PlayerManager.NextPlayer(gameManager.ClockWise);
                        nextPlayer.AddCardToHand(gameManager.Picking());
                        nextPlayer.AddCardToHand(gameManager.Picking());
                        break;
                    }
                    case "Skip":
                        gameManager.PlayerManager.NextPlayer(gameManager.ClockWise, true);
                        break;
                    case "Reverse":
                        gameManager.ClockWise = !gameManager.ClockWise;
                        break;
                    default:
                    {
                        if (card.Color == ConsoleColor.White)
                        {
                            gameManager.ColorSelector.Draw();
                            if (card.Value != "Wild Draw Four") return;
                            var nextPlayer = gameManager.PlayerManager.NextPlayer(gameManager.ClockWise);
                            nextPlayer.AddCardToHand(gameManager.Picking());
                            nextPlayer.AddCardToHand(gameManager.Picking());
                            nextPlayer.AddCardToHand(gameManager.Picking());
                            nextPlayer.AddCardToHand(gameManager.Picking());
                            return;
                        }
                        break;
                    }
                }
            }
            currentPlayer.ClearDrawHand(true);
            gameManager.WaitPlayer = true;
            gameManager.PlayerManager.CurrentPlayer().CurrentPlayer = false;
            Writer.Clear(gameManager.PlayerManager.CurrentPlayer());
            gameManager.PlayerManager.NextPlayer(gameManager.ClockWise, true);
            gameManager.PlayerManager.CurrentPlayer().CurrentPlayer = true;
            Writer.Write(gameManager.PlayerManager.CurrentPlayer());
            var nP = gameManager.PlayerManager.NextPlayer(gameManager.ClockWise);
            Writer.Write(nP);
            Writer.Write(gameManager.PlayerManager);
            if (currentPlayer.Hand.Count != 0) return;
            gameManager.EndGame();
            gameManager.Winner = currentPlayer;

        }

    }
}