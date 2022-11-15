using System;
using System.Collections.Generic;
using Amnesia.components.Uno;

namespace Amnesia.classes.Uno
{
    public class GameManager
    {
        public readonly Stack<Card> ShuffledDeck = CardManager.CreateDeck("random");
        private Card _deckCard = new Card();
        

        private Stack<Card> _discardPile = new Stack<Card>();
        public Card CurrenCard { get; set; }
        public readonly List<Player> Players = new List<Player>();
        private int CurrentPlayerIndex { get; set; } = 0;
        private int _direction = 1;
        private int _currentColor = 0;

        public const int NumberOfCardsToDraw = 2;
        public bool IsGameFinished { get; private set; }

        public void StartGame()
        {
            Players.Add(new Player());
            DistributeCards();
            AddDiscardPile(ShuffledDeck.Pop());
            CurrentGame();
        }

        private void CurrentGame()
        {
            DrawDeck();
            while (true)
            {
                Players[CurrentPlayerIndex].DrawHand();
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.LeftArrow:
                        Players[CurrentPlayerIndex].SelectPrevious();
                        break;
                    case ConsoleKey.RightArrow:
                        Players[CurrentPlayerIndex].SelectNext();
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
                    default:
                        break;
                }
                if (!IsGameFinished) continue;
                break;
            }
        }

        public void AddDiscardPile(Card card)
        {
            CurrenCard = card;
            _discardPile.Push(card);
            card.X = Console.WindowWidth / 2 - Card.Width;
            card.Y = 1;
            Writer.ObjForWrite.Add(card);
        }
        private void DrawDeck()
        {
            _deckCard.IsSelected = CurrentPlayer().SelectedDeck;
            _deckCard.X = Console.WindowWidth / 2 + 2;
            _deckCard.Y = 1;
            _deckCard.Value = ShuffledDeck.Count.ToString();
            Writer.ObjForWrite.Add(_deckCard);
        }

        private void DistributeCards()
        {
            for (var i = 0; i < NumberOfCardsToDraw; i++)
            {
                foreach (var player in Players)
                {
                    var card = ShuffledDeck.Pop();
                    player.AddCardToHand(card);
                }
            }
        }

        public Player CurrentPlayer()
        {
            return Players[CurrentPlayerIndex];
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