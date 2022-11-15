using System;
using System.Diagnostics;
using Amnesia.classes.Uno;
using Amnesia.interfaces;

namespace Amnesia.components.Uno
{
    public class Card : IDrawable, ICloneable
    {

        private bool _isDeck = false;
        
        public Card()
        {
            Value = "****";
            Color = ConsoleColor.White;
            IsSelected = false;
            _isDeck = true;
        }
        
        public Card(string value, ConsoleColor color = ConsoleColor.White)
        {
            Value = value;
            Color = color;
            IsSelected = false;
        }

        private Card(Card card)
        {
            Value = card.Value;
            Color = card.Color;
            IsSelected = card.IsSelected;
            _isDeck = card._isDeck;
            X = card.X;
            Y = card.Y;
        }
        
        public ConsoleColor Color { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        public static int Width => CardManager.LongestCard + 4;

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

        public bool IsDrawn()
        {
            Debug.WriteLine($"IsDraw: {X}, {Y}, {Width}, ");
            return !(X < 0 || X + Width >= Console.WindowWidth-1 || Y < 0 || Y + 5 >= Console.WindowHeight-1);
        }
                                                                                                                                                                                                                                            
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

        public void Clear()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            if (!IsDrawn()) return;
            for (var i = X; i < X+Width; i++)
            {
                for (var j = Y; j < Y+5; j++)
                {
                    Debug.WriteLine($"X: {X}, Y: {Y}");
                    Debug.WriteLine($"{Console.WindowWidth}, {Console.WindowHeight}");
                    Console.SetCursorPosition(i,j);
                    Console.Write(" ");
                }
            }
        }

        public object Clone()
        {
            return new Card(this);
        }
    }
}