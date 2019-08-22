/*
 * Author: Wyatt Tse
 * Description: The script refers to the data members and the behaviours of GameObject Playing Card.
 * 
 *              PlayingCard playingCard: The card represented
 *              ParticleSystem VFX: The particle effect
 *              Image image: Show the sprite of the card
 *              bool isFacedUp: State whether the card is revealled. GamingTable gamingTable allow convenient access of shared resource
 */
using UnityEngine;
using UnityEngine.UI;

public class PlayingCardImage : MonoBehaviour
{
    public PlayingCard playingCard { private get; set; }
    [SerializeField]
    private ParticleSystem VFX;
    private Image image;
    private bool isFacedUp;
    private static GamingTable gamingTable;

    // Initialize the data members
    public void Start() {
        gamingTable = transform.parent.parent.GetComponent<GamingTable>();
        image = GetComponent<Image>();
    }


    // Allow a player to pick the card corresponded depends on bool isAllowed and isFacedUp
    public void AllowPicking(bool isAllowed) {
        var button = GetComponent<Button>();
        button.interactable = isAllowed && !isFacedUp;
    }

    //Watershed of calling void FaceUp() or void FaceDown() depend on bool faceUp
    public void Flip(bool faceUp) {
        if (faceUp)
            FaceUp();
        else
            FaceDown();
    }

    // Reveal the playing card, play the sound effect and particle effect and add the playing card to hand.
    private void FaceUp() {
        isFacedUp = true;

        image.sprite = playingCard.frontSprite;

        AudioManager.instance.PlaySFX(gamingTable.flipSFX, false);
        if (VFX) VFX.Stop();

        var gameManager = GameManager.instance;
        if (!gameManager.isProp) {
            gameManager.hands.Add(playingCard);
            // Count suits of the hand
            ++gameManager.suitsOnHands[(int)playingCard.suit];
        }
    }

    // Cover the playing card and play the sound effect
    private void FaceDown() {
        if (!isFacedUp) return;

        var playingCardBack = gamingTable.backSprite;
        image.sprite = playingCardBack;

        AudioManager.instance.PlaySFX(gamingTable.flipSFX, false);
        if (VFX) VFX.Play();

        isFacedUp = false;
    }
}
