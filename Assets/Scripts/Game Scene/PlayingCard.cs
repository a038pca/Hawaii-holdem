/*
 * Author: Wyatt Tse
 * Description:  The class describe different infomation of a playing card.
 * 
 *               Suit suit: The suit of the card
 *               number: represents different rank respectively
 *               frontSprite: The sprite of the playing card's front
 */
[System.Serializable]
public class PlayingCard
{
    public Suit suit;
    public int number;
    public UnityEngine.Sprite frontSprite;

    public PlayingCard(Suit suit, int number) {
        this.suit = suit;
        this.number = number;
    }
}

// An enumunation refer to suits in playing cards
public enum Suit { Spade, Heart, Club, Diamond }

