/*
 * Author: Wyatt Tse
 * Description: The class store a whole deck of playing cards. ScriptableObject is inherited for more convenient setup and cleaner code
 */
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Deck")]
public class Deck : ScriptableObject {
    public List<PlayingCard> playingCards = new List<PlayingCard>();
}
