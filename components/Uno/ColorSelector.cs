using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Amnesia.classes.Uno;
using Amnesia.interfaces;

namespace Amnesia.components.Uno
{
    public class ColorSelector: IDrawable
    {
        private static readonly List<Card> ColorCards = new List<Card>();

        /// <summary>
        /// Init list of color cards
        /// </summary>
        private static void InitColorCard()
        {
            var colorList = CardManager.Color;
            var space = (Console.WindowWidth % Card.Width)/2;
            if (Card.Width * colorList.Count < Console.WindowWidth)
            {
                space = (Console.WindowWidth - Card.Width * colorList.Count) / 2;
            }
            foreach (var card in colorList.Select(color => new Card(color.ToString(), color)))
            {
                if (ColorCards.Count == 0)
                {
                    card.IsSelected = true;
                }
                card.X = colorList.IndexOf(card.Color) * Card.Width + space;
                card.Y = (Console.WindowHeight / 3) + 10;
                ColorCards.Add(card);
            }
        }

        /// <summary>
        /// Select next color cards
        /// </summary>
        public static void SelectNext()
        {
            var card = ColorCards.First(c => c.IsSelected);
            card.IsSelected = false;
            Card nextCard;
            if (ColorCards.IndexOf(card) < ColorCards.Count - 1)
            {
                nextCard = ColorCards[ColorCards.IndexOf(card) + 1];
                nextCard.IsSelected = true;
            }
            else
            {
                nextCard = ColorCards[0];
                nextCard.IsSelected = true;
            }
            Writer.ObjForWrite.Add(card);
            Writer.ObjForWrite.Add(nextCard);
        }

        /// <summary>
        /// Select previous color card 
        /// </summary>
        public static void SelectPrevious()
        {
            var card = ColorCards.First(c => c.IsSelected);
            card.IsSelected = false;
            Card prevCard;
            if (ColorCards.IndexOf(card) > 0)
            {
                prevCard = ColorCards[ColorCards.IndexOf(card) - 1];
                prevCard.IsSelected = true;
            }
            else
            {
                prevCard = ColorCards[ColorCards.Count-1];
                prevCard.IsSelected = true;
            }
            Writer.ObjForWrite.Add(card);
            Writer.ObjForWrite.Add(prevCard);
        }
        
        /// <summary>
        /// Get selected color
        /// </summary>
        /// <returns>Selected color</returns>
        public static ConsoleColor CurrentColor()
        {
            var card = ColorCards.First(c => c.IsSelected);
            return card.Color;
        }
        
        /// <summary>
        /// Display color card
        /// </summary>
        public void Draw()
        {
            if (ColorCards.Count == 0) InitColorCard();
            Debug.WriteLine("ColorSelector.Draw()");
            foreach (var card in ColorCards)
            {
                // Debug.WriteLine(card.Value);
                Writer.ObjForWrite.Add(card);
            }
        }

        /// <summary>
        /// Clear color card
        /// </summary>
        /// <param name="full"></param>
        public void Clear(bool full)
        {
            foreach (var card in ColorCards)
            {
                Writer.ObjForClear.Add(card);
            }
        }
    }
}