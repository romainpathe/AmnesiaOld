using System;
using System.Collections.Generic;
using System.Linq;
using Amnesia.classes.Uno;

namespace Amnesia.components.Uno
{
    public class Player
    {
        public string Name { get; }

        public List<Card> Hand { get; }
        public static List<Card> OldHand { get; set; }
        
        public bool SelectedDeck { get; set; }

        public Player()
        {
            SelectedDeck = true;
            // Console.WriteLine("Enter your name: ");
            // Name = Console.ReadLine();
            Hand = new List<Card>();
        }
        
        public void AddCardToHand(Card card)
        {
            card.Y = (Console.WindowHeight / 2) - 2;
            Hand.Add(card);
            ResetPosition();
        }

        private void ResetPosition()
        {
            // var space = (Console.WindowWidth % Card.Width)/2;
            // if (Card.Width * Hand.Count < Console.WindowWidth)
            // {
                // space = (Console.WindowWidth - Card.Width * Hand.Count) / 2;
            // }
            for(var i = 0; i < Hand.Count; i++)
            {
                Hand[i].X = i * Card.Width;
                // Hand[i].X = i * Card.Width + space;
            }
        }

        public bool CanPlayCurrentCard(GameManager gameManager)
        {
            var result = false;

            var currentCard = gameManager.CurrenCard;
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
                SelectNext();
                Hand.Remove(card);
                ResetPosition();
                DrawHand();
            }
            return result;
        }
        
        public void DrawHand()
        {
            ClearDrawHand();
            foreach (var card in Hand)
            {
                Writer.ObjForWrite.Add(card);
                // card.Draw();
            }
        }

        public void SelectNext()
        {
            var needMove = false;
            for (var i = 0; i < Hand.Count; i++)
            {
                if (!Hand[i].IsSelected) continue;
                Hand[i].IsSelected = false;
                if (i == Hand.Count - 1)
                {
                    var card = Hand[0];
                    card.IsSelected = true;
                    if (!card.IsDrawn())
                    {
                        needMove = true;
                    }
                }else
                {
                    var card = Hand[i + 1];
                    card.IsSelected = true;
                    if (!card.IsDrawn())
                    {
                        needMove = true;
                    }
                }
                break;
            }

            if (needMove)
            {
                foreach (var card in Hand)
                {
                    card.MoveRight(0,(Hand.Count-1) * Card.Width);
                }
            }
            DrawHand();
        }

        public void SelectPrevious()
        {
            var needMove = false;
            for (var i = 0; i < Hand.Count; i++)
            {
                if (!Hand[i].IsSelected) continue;
                Hand[i].IsSelected = false;
                if (i == 0)
                {
                    var card = Hand[Hand.Count - 1];
                    card.IsSelected = true;
                    if (!card.IsDrawn())
                    {
                        needMove = true;
                    }
                }else
                {
                    var card = Hand[i - 1];
                    card.IsSelected = true;
                    if (!card.IsDrawn())
                    {
                        needMove = true;
                    }
                }
                break;
            }

            if (needMove)
            {
                foreach (var card in Hand)
                {
                    card.MoveLeft(0,(Hand.Count) * Card.Width);
                }
                DrawHand();
            }
        }
        
        private static void ClearDrawHand()
        {
            if(OldHand == null) return;
            foreach (var card in OldHand)
            {
                Writer.ObjForClear.Add(card);
            }
        }
        
        

    }
}