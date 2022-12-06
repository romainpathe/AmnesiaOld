using System;
using Amnesia.classes.Uno;
using Amnesia.interfaces;

namespace Amnesia.components.Uno
{
    public class Card : IDrawable, ICloneable
    {

        private readonly bool _isDeck;
        public ConsoleColor Color { get; set; }
        public bool WhiteCard { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public static int Width => CardManager.LongestCard + 4;
        
        /// <summary>
        /// Constructor for a card without value
        /// </summary>
        public Card()
        {
            Value = "****";
            Color = ConsoleColor.White;
            IsSelected = false;
            _isDeck = true;
        }
        
        /// <summary>
        /// Constructor for a card
        /// </summary>
        /// <param name="value">Value of card</param>
        /// <param name="color">Color of card</param>
        public Card(string value, ConsoleColor color = ConsoleColor.White)
        {
            Value = value;
            Color = color;
            IsSelected = false;
        }

        /// <summary>
        /// Constructor for a card
        /// </summary>
        /// <param name="card">OldCard (Use for clone item)</param>
        private Card(Card card)
        {
            Value = card.Value;
            Color = card.Color;
            IsSelected = card.IsSelected;
            _isDeck = card._isDeck;
            X = card.X;
            Y = card.Y;
        }

        /// <summary>
        /// Move card to left
        /// </summary>
        /// <param name="min">Minimum position</param>
        /// <param name="max">Maximum position</param>
        public void MoveLeft(int min, int max)
        {
            X += Width;
            if (X > max)
            {
                X = min;
            }else if (X < min)
            {
                X = max;
            }
        }
        
        /// <summary>
        /// Move card to right
        /// </summary>
        /// <param name="min">Minimum position</param>
        /// <param name="max">Maximum position</param>
        public void MoveRight(int min, int max)
        {
            X -= Width;
            if (X > max)
            {
                X = min;
            }else if (X < min)
            {
                X = max;
            }
        }

        /// <summary>
        /// Check if card is drawable
        /// </summary>
        /// <returns>Boolean</returns>
        public bool IsDrawn()
        {
            return !(X < 0 || X + Width >= Console.WindowWidth-1 || Y < 0 || Y + 5 >= Console.WindowHeight-1);
        }
               
        /// <summary>
        /// Display card in console
        /// </summary>
        public void Draw()
        {
            if (!IsDrawn()) return;
            var x = X;
            var y = Y;
            Console.SetCursorPosition(x,y);
            Console.BackgroundColor = IsSelected ? ConsoleColor.White : ConsoleColor.Black;
            Console.ForegroundColor = Console.BackgroundColor == ConsoleColor.White && Color == ConsoleColor.White ? ConsoleColor.Black : Color;
            if (_isDeck)
            {
                const string deck = " Deck ";
                for (var i = 0; i < Width/2-deck.Length/2; i++)
                {
                    Console.Write("*");
                }   
                Console.Write(deck);
                for (var i = 0; i < Width/2-deck.Length/2; i++)
                {
                    Console.Write("*");
                }
            }
            else
            {
                for (var i = 0; i < Width; i++)
                {
                    Console.Write("*");
                }  
            }
            y++;
            Console.SetCursorPosition(x,y);
            Console.Write("*");
            Console.SetCursorPosition(x+Width-1,y);
            Console.Write("*");
            y++;
            Console.SetCursorPosition(x,y);
            Console.Write("*");
            Console.SetCursorPosition(x+Width/2-(Value.Length/2),y);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = Color;
            Console.Write(Value);
            Console.BackgroundColor = IsSelected ? ConsoleColor.White : ConsoleColor.Black;
            Console.ForegroundColor = Console.BackgroundColor == ConsoleColor.White && Color == ConsoleColor.White ? ConsoleColor.Black : Color;
            Console.SetCursorPosition(x+Width-1,y);
            Console.Write("*");
            y++;
            Console.SetCursorPosition(x,y);
            Console.Write("*");
            Console.SetCursorPosition(x+Width-1,y);
            Console.Write("*");
            y++;
            Console.SetCursorPosition(x,y);
            for (var i = 0; i < Width; i++)
            {
                Console.Write("*");
            }
        }

        /// <summary>
        /// Clear card in console
        /// </summary>
        /// <param name="full">If we clear only value or all element</param>
        public void Clear(bool full = false)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            if (!IsDrawn()) return;
            var windowWidth = Console.WindowWidth;
            var windowHeight = Console.WindowHeight;
            if (full)
            {
                for (var i = X; i < X+Width && X+Width < windowWidth; i++)
                {
                    for (var j = Y; j < Y+5 && Y+5 < windowHeight; j++)
                    {
                        Console.SetCursorPosition(i,j);
                        Console.Write(" ");
                    }
                }
            }
            else
            {
                for (var i = X+1; i < X+Width-1 && X+Width-1 < windowWidth; i++)
                {
                    for (var j = Y+1; j < Y+4 && Y+4 < windowHeight; j++)
                    {
                        Console.SetCursorPosition(i,j);
                        Console.Write(" ");
                    }
                }
            }
        }

        /// <summary>
        /// Clone this card
        /// </summary>
        /// <returns>Card</returns>
        public object Clone()
        {
            return new Card(this);
        }
    }
}