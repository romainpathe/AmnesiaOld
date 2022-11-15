using System;
using System.Linq;

namespace Amnesia.classes.Uno
{
    public static class GameAction
    {
        
        public static void UpArrow(GameManager gameManager)
        {
            if (gameManager.CurrentPlayer().SelectedDeck) return;
            gameManager.CurrentPlayer().SelectedDeck = true;
            gameManager.CurrentPlayer().Hand.First(card => card.IsSelected).IsSelected = false;
        }
        
        public static void DownArrow(GameManager gameManager)
        {
            if(!gameManager.CurrentPlayer().SelectedDeck) return;
            gameManager.CurrentPlayer().SelectedDeck = false;
            gameManager.CurrentPlayer().Hand.First().IsSelected = true;
        }
        
        public static void LeftArrow(GameManager gameManager)
        {
            
        }
        
        public static void RightArrow(GameManager gameManager)
        {
            
        }
        
        public static void Space(GameManager gameManager)
        {
            
        }
        
        public static void Enter(GameManager gameManager)
        {
            if (gameManager.CurrentPlayer().SelectedDeck)
            {
                gameManager.CurrentPlayer().AddCardToHand(gameManager.ShuffledDeck.Pop());
            }
            else
            {
                var canPlay =gameManager.CurrentPlayer().CanPlayCurrentCard(gameManager);
                if (!canPlay) return;
                var card = gameManager.CurrentPlayer().PlayCard(true);
                gameManager.AddDiscardPile(card);
            }
        }

    }
}