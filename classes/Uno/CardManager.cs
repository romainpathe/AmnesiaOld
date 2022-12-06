using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Amnesia.components.Uno;

namespace Amnesia.classes.Uno
{
    public static class CardManager
    {
        /// <summary>
        /// Variable for create the deck of cards
        /// </summary>
        public static readonly List<ConsoleColor> Color = new List<ConsoleColor>{ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Green, ConsoleColor.DarkYellow};
        private static readonly Dictionary<string, int> CardWithoutColor = new Dictionary<string, int>{ { "Wild", 4 }, { "Wild Draw Four", 4 }};
        // private static readonly Dictionary<string, int> CardWithoutColor = new Dictionary<string, int>{ { "Wild", 1 }};
        private static readonly Dictionary<string, int> CardWithColor = new Dictionary<string, int>{ { "Zero", 1 }, { "One", 2 }, { "Two", 2 }, { "Three", 2 }, { "Four", 2 }, { "Five", 2 }, { "Six", 2 }, { "Seven", 2 }, { "Eight", 2 }, { "Nine", 2 }, { "Skip", 2 }, { "Reverse", 2 }, { "Draw Two", 2 } };
        // private static readonly Dictionary<string, int> CardWithColor = new Dictionary<string, int>{ { "Zero", 1 }, { "One", 2 }, { "Two", 2 } };
        public static int LongestCard { get; private set; }

        /// <summary>
        /// Function for create the deck of cards and order it if you want
        /// </summary>
        /// <param name="type">Ordered deck or random deck</param>
        /// <returns>Deck of card</returns>
        public static Stack<Card> CreateDeck(string type = "ordered")
        {
            var deck = new List<Card>();
            foreach (var color in Color)
            {
                foreach (var card in CardWithColor)
                {
                    if(card.Key.Length > LongestCard)
                    {
                        LongestCard = card.Key.Length;
                    }
                    for (var i = 0; i < card.Value; i++)
                    {
                        deck.Add(new Card(card.Key, color));
                    }
                }
            }
            foreach (var card in CardWithoutColor)
            {
                if(card.Key.Length > LongestCard)
                {
                    LongestCard = card.Key.Length;
                }
                for (var i = 0; i < card.Value; i++)
                {
                    var c = new Card(card.Key)
                    {
                        WhiteCard = true
                    };
                    deck.Add(c);
                }
            }
            var finalDeck = new Stack<Card>();
            if (type == "random")
            {
                finalDeck = ShuffleDeck(deck);
            }
            else
            {
                foreach (var card in deck)
                {
                    finalDeck.Push(card);
                }
            }

            return finalDeck;

        }
        
        /// <summary>
        /// Function for shuffle the ordered deck of cards
        /// </summary>
        /// <param name="deck">Ordered deck</param>
        /// <returns>Random deck</returns>
        private static Stack<Card> ShuffleDeck(IEnumerable<Card> deck)
        {
            var shuffledDeck = new Stack<Card>(deck.OrderBy(x => Program.Random.Next()));
            return shuffledDeck;
        }
        
    }
}