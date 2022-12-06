using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Amnesia.classes.Uno;
using Amnesia.interfaces;

namespace Amnesia.components.Uno
{
    public class Player : IDrawable
    {
        public string Name { get; }
        public static int MaxPlayersNameLength { get; set; }
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public List<Card> Hand { get; }
        public List<Card> OldHand { get; set; }
        public bool CurrentPlayer { get; set; }
        public bool SelectedDeck { get; set; }
        private int WindowsHeight { get; set; }
        public int LastCardSelected { get; set; }
        public bool ColorSelector { get; set; }
        private bool DrawAll { get; set; } = true;
        /// <summary>
        /// Constructor for the player class
        /// </summary>
        /// <param name="name">Name of player</param>
        public Player(string name)
        {
            SelectedDeck = true;
            Name = name;
            Hand = new List<Card>();
        }
        
        /// <summary>
        /// Add a card to the players hand
        /// </summary>
        /// <param name="card"></param>
        public void AddCardToHand(Card card)
        {
            var y = (Console.WindowHeight / 3) - 2;
            if (WindowsHeight != Console.WindowHeight)
            {
                WindowsHeight = Console.WindowHeight;
                foreach (var tCard in Hand)
                {
                    Writer.ObjForClear.Add(tCard.Clone());
                    tCard.Y = y;
                }
            }
            card.Y = y;
            Hand.Add(card);
            ResetPosition();
            LastCardSelected = 1;
        }

        /// <summary>
        /// Reset all position of the cards in the players hand
        /// </summary>
        private void ResetPosition()
        {
            var space = (Console.WindowWidth % Card.Width)/2;
            if (Card.Width * Hand.Count < Console.WindowWidth)
            {
                space = (Console.WindowWidth - Card.Width * Hand.Count) / 2;
            }
            for(var i = 0; i < Hand.Count; i++)
            {
                // Hand[i].X = i * Card.Width;
                Hand[i].X = i * Card.Width + space;
            }
        }

        /// <summary>
        /// Check if the player can play a card
        /// </summary>
        /// <param name="gameManager">Manager of game</param>
        /// <returns>If player can play this card</returns>
        public bool CanPlayCurrentCard(GameManager gameManager)
        {
            var result = false;

            var currentCard = gameManager.CurrentCard;
            Card playerCard = null;
            foreach (var card in Hand.Where(card => card.IsSelected).ToList())
            {
                playerCard = card;
            }
            if (playerCard == null) return result;
            if (playerCard.Color == ConsoleColor.White || playerCard.Color == currentCard.Color || playerCard.Value == currentCard.Value)
            {
                result = true;
            }
            return result;
        }
        
        /// <summary>
        /// Called when player play a card and remove it from his hand
        /// </summary>
        /// <param name="remove">If i need remove this card of player hand</param>
        /// <returns>Card play by the player</returns>
        public Card PlayCard(bool remove = false)
        {
            Card result = null;
            OldHand = new List<Card>();
            foreach (var card in Hand)
            {
                OldHand.Add((Card) card.Clone());
            }
            foreach (var card in Hand.Where(card => card.IsSelected).ToList())
            {
                result = card;
                if (!remove) continue;
                if (result.Color == ConsoleColor.White)
                {
                    ColorSelector = true;
                    Hand.First(hCard => hCard.IsSelected).IsSelected = false;
                }
                else
                {
                    SelectNext();
                }
                Writer.Clear(card);
                Hand.Remove(card);
            }
            ClearDrawHand();
            ResetPosition();
            return result;
        }
        
        /// <summary>
        /// Display the player hand in console
        /// </summary>
        public void DrawHand()
        {
            foreach (var card in Hand)
            {
                Writer.ObjForWrite.Add(card);
            }
        }

        /// <summary>
        /// Select the next card in the hand
        /// </summary>
        public void SelectNext()
        {
            var needMove = false;
            for (var i = 0; i < Hand.Count; i++)
            {
                if (!Hand[i].IsSelected) continue;
                Hand[i].IsSelected = false;
                Card nextCard;
                if (i == Hand.Count - 1)
                {
                    nextCard = Hand[0];
                    nextCard.IsSelected = true;
                    if (!nextCard.IsDrawn())
                    {
                        needMove = true;
                    }
                }else
                {
                    nextCard = Hand[i + 1];
                    nextCard.IsSelected = true;
                    if (!nextCard.IsDrawn())
                    {
                        needMove = true;
                    }
                }
                LastCardSelected = Hand.IndexOf(nextCard);
                if(!needMove) Writer.ObjForWrite.Add(nextCard);
                break;
            }

            if (!needMove) return;
            var min = (Console.WindowWidth % Card.Width)/2;
            if (Card.Width * Hand.Count < Console.WindowWidth)
            {
                min = (Console.WindowWidth - Card.Width * Hand.Count) / 2;
            }
            var max = (Hand.Count-1) * Card.Width + min;
            foreach (var card in Hand)
            {
                card.MoveRight(min,max);
            }
        }

        /// <summary>
        /// Select the previous card in the hand
        /// </summary>
        public void SelectPrevious()
        {
            var needMove = false;
            for (var i = 0; i < Hand.Count; i++)
            {
                if (!Hand[i].IsSelected) continue;
                Hand[i].IsSelected = false;
                Card prevCard;
                if (i == 0)
                {
                    prevCard = Hand[Hand.Count - 1];
                    prevCard.IsSelected = true;
                    if (!prevCard.IsDrawn())
                    {
                        needMove = true;
                    }
                }else
                {
                    prevCard = Hand[i - 1];
                    prevCard.IsSelected = true;
                    if (!prevCard.IsDrawn())
                    {
                        needMove = true;
                    }
                }
                LastCardSelected = Hand.IndexOf(prevCard);
                if(!needMove) Writer.ObjForWrite.Add(prevCard);
                break;
            }

            if (needMove)
            {
                var min = (Console.WindowWidth % Card.Width)/2;
                if (Card.Width * Hand.Count < Console.WindowWidth)
                {
                    min = (Console.WindowWidth - Card.Width * Hand.Count) / 2;
                }
                var max = (Hand.Count-1) * Card.Width + min;
                foreach (var card in Hand)
                {
                    card.MoveLeft(min,max);
                }
            }
        }

        /// <summary>
        /// Clear the hand of the player in console
        /// </summary>
        /// <param name="current">If i need clear the old hand or current hand</param>
        public void ClearDrawHand(bool current = false)
        {
            var hand = OldHand;
            if (current)
            {
                hand = Hand;
            }
            if(hand == null) return;
            foreach (var card in hand)
            {
                Debug.WriteLine(card.Value);
                Writer.ObjForClear.Add(card);
            }
        }

        /// <summary>
        /// Called when it's the turn of the player
        /// </summary>
        /// <param name="gameManager">Manager of game</param>
        public void Turn(GameManager gameManager)
        {
            DrawHand();
            gameManager.CurrentGame();
        }
        
        /// <summary>
        /// Display the player name in console
        /// </summary>
        public void Draw()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (CurrentPlayer)
            {
                Console.SetCursorPosition(0,0);
                Console.WriteLine("Current Player: "+Name);
            }
            else
            {
                var x = Console.WindowWidth - (13 + Name.Length);
                Console.SetCursorPosition(x,0);
                Console.Write("Next Player: "+Name);   
            }
            if (!DrawAll) return;
            
        }

        /// <summary>
        /// Clear the player name in console
        /// </summary>
        /// <param name="full">Not use</param>
        public void Clear(bool full)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            if (CurrentPlayer)
            {
                Console.SetCursorPosition(0,0);
                for (var i = 0; i < MaxPlayersNameLength+16; i++)
                {
                    Console.Write(' ');
                }  
            }
            else
            {
                var x = Console.WindowWidth - (13 + MaxPlayersNameLength);
                Console.SetCursorPosition(x,0);
                for (var i = 0; i < MaxPlayersNameLength+13; i++)
                {
                    Console.Write(' ');
                }  
            }
            if (!DrawAll) return;
        }
    }
}