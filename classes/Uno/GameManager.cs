using System;
using System.Collections.Generic;
using System.Linq;
using Amnesia.components.Uno;

namespace Amnesia.classes.Uno
{
    public class GameManager
    {
        private readonly Stack<Card> _shuffledDeck = CardManager.CreateDeck("random"); // Deck of cards
        private readonly Card _deckCard = new Card(); // Card for display deck in console
        
        public readonly ColorSelector ColorSelector = new ColorSelector(); // Color selector for wild cards
        private readonly Stack<Card> _discardPile = new Stack<Card>(); // Discard pile
        public Card CurrentCard { get; set; }
        public readonly PlayerManager PlayerManager = new PlayerManager();
        public bool ClockWise { get; set; } = true;
        public bool WaitPlayer { get; set; }
        public Player Winner { get; set; }

        private const int NumberOfCardsToDraw = 2; // Number of cards at the beginning of the game
        private bool IsGameFinished { get; set; }
        private readonly List<Card> _tmpCards = new List<Card>();
        
        /// <summary>
        /// Start the game
        /// </summary>
        public void StartGame()
        {
            PlayerManager.RandomizeFirstPlayer();
            DistributeCards();
            AddDiscardPile(ValidCardForStart());
            PlayerManager.CurrentPlayer().CurrentPlayer = true;
            Writer.Write(PlayerManager.CurrentPlayer());
            CurrentGame();
        }
        
        /// <summary>
        /// Check if the first card selected is valid
        /// </summary>
        /// <returns>Card</returns>
        private Card ValidCardForStart()
        {
            var tmp = _shuffledDeck.Pop();
            if (tmp.Color == ConsoleColor.White)
            {
                _tmpCards.Add(tmp);
                tmp = ValidCardForStart();
            }
            else
            {
                foreach (var card in _tmpCards)
                {
                    _shuffledDeck.Push(card);
                }
            }
            return tmp;
        }
        
        /// <summary>
        /// Loop for player turn
        /// </summary>
        public void CurrentGame()
        {
            Writer.Write(PlayerManager.CurrentPlayer());
            PlayerManager.CurrentPlayer().DrawHand();
            while (!IsGameFinished || WaitPlayer)
            {
                if (WaitPlayer)
                {
                    DrawDeck(false);
                }
                else
                {
                    DrawDeck();
                }
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.LeftArrow:
                        GameAction.LeftArrow(this);
                        break;
                    case ConsoleKey.RightArrow:
                        GameAction.RightArrow(this);
                        break;
                    case ConsoleKey.DownArrow:
                        GameAction.DownArrow(this);
                        DrawDeck();
                        break;
                    case ConsoleKey.UpArrow:
                        GameAction.UpArrow(this);
                        DrawDeck();
                        break;
                    case ConsoleKey.Enter:
                        GameAction.Enter(this);
                        DrawDeck();
                        break;
                    case ConsoleKey.Escape:
                        IsGameFinished = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Add a card to the discard pile
        /// </summary>
        /// <param name="card">Card for discard pile</param>
        public void AddDiscardPile(Card card)
        {
            CurrentCard = card;
            _discardPile.Push(card);
            card.X = Console.WindowWidth / 2 - Card.Width;
            card.Y = 1;
            Writer.ObjForWrite.Add(card);
        }

        /// <summary>
        /// For draw the deck in console
        /// </summary>
        /// <param name="selectPlayer">Do not select the deck when the user is on hold</param>
        private void DrawDeck(bool selectPlayer = true)
        {
            _deckCard.IsSelected = selectPlayer && PlayerManager.CurrentPlayer().SelectedDeck;
            _deckCard.X = Console.WindowWidth / 2 + 2;
            _deckCard.Y = 1;
            _deckCard.Value = _shuffledDeck.Count.ToString();
            Writer.ObjForWrite.Add(_deckCard);
        }

        /// <summary>
        /// Give cards to players
        /// </summary>
        /// <returns>Card, first card in _shuffledDeck</returns>
        public Card Picking()
        {
            if (_shuffledDeck.Count == 1)
            {
                ResetDeck();
            }
            return _shuffledDeck.Pop();
        }
        
        /// <summary>
        /// Reset the deck when there is only one card left
        /// </summary>
        private void ResetDeck()
        {
            var tempLastCard = _shuffledDeck.Last();
            var tempFirstDiscardCard = _discardPile.Pop();
            _shuffledDeck.Clear();
            var tempList = new List<Card>();
            foreach (var card in _discardPile)
            {
                if(card.WhiteCard) card.Color = ConsoleColor.White;
                tempList.AddRange(_discardPile);
            }
            _discardPile.Clear();
            _discardPile.Push(tempFirstDiscardCard);
            var n = tempList.Count;  
            while (n > 1) {  
                n--;  
                var k = Program.Random.Next(n + 1); 
                //Deconstructing for swap elements 
                (tempList[k], tempList[n]) = (tempList[n], tempList[k]);
            }
            foreach (var card in tempList)
            {
                _shuffledDeck.Push(card);
            }
            _shuffledDeck.Push(tempLastCard);
        }
        
        /// <summary>
        /// Distribute cards to players
        /// </summary>
        private void DistributeCards()
        {
            for (var i = 0; i < NumberOfCardsToDraw; i++)
            {
                foreach (var player in PlayerManager.Players)
                {
                    var card = _shuffledDeck.Pop();
                    player.AddCardToHand(card);
                }
            }
        }
 
        /// <summary>
        /// Display the end screen
        /// </summary>
        public void EndGame()
        {
            IsGameFinished = true;
            Writer.Stop();
            Console.Clear();
            Console.SetCursorPosition(0,0);
            Console.WriteLine("Finished Game");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Winner is: " + PlayerManager.PreviousPlayer(ClockWise).Name);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Thanks to");
            foreach (var player in PlayerManager.Players)
            {
                Console.WriteLine("  - "+player.Name);
            }
            Console.WriteLine("for playing");
            Console.WriteLine("Press any key to exit game");
            Console.ReadKey();
            Environment.Exit(0);
        }

        
        


        // public void Redraw()
        // {
        //     Console.BackgroundColor = ConsoleColor.Black;
        //     for (var i = 0; i < Program.WindowsWidth; i++)
        //     {
        //         for (var j = 0; j < Program.WindowsHeight; j++)
        //         {
        //             Console.SetCursorPosition(i, j);
        //             Console.Write(" ");
        //         }
        //     }
        //     DrawDeck();
        //     var card = _discardPile.Peek();
        //     AddDiscardPile(card);
        //     foreach (var player in Players)
        //     {
        //         player.DrawHand();
        //     }
        // }

    }
}