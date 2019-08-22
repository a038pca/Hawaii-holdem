/*
 * Author: Wyatt Tse
 * Description: The script is the main controller of the game.
 *              As many other objects need to communicate with GameManager, static GameManager instance is set for more convenient calls of functions.
 * 
 *              bool isFlop: States whether the game has been the flop
 *              bool isProp: States whether a flipping of cards is due to Property A
 *              bool canBet: States whether a player has enough money to bet
 *              List<PlayingCard> hands: Stores hands of a player
 *              int[] suitsOnHands: Counts the number of each suit on hands
 *              int multiplier: Multiplies the bonus when Property B is used
 *              numOfHands: The maximum number of cards on hands.
 *              Deck deck: The deck of playing cards
 *              Transform flopsTransform, pickersTransform: Transform instance of GameObject Flops and Pickers
 *              PropButtons propButtons: PropButtons instance of GameObject propButtons
 *              Button betButton, foldButton: Button instance of GameObject Bet Button and Fold Button
 *              GameObject WinVFX, AudioClip winSFX: The visual and sound effect when a player win
 *              Summery summery: Summery instance in GameObject Summery
 *              AudioClip againSFX: The sound effect when a player plays the game again
 *              int gamblingChip: The number of gambling chips owned by a player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager: MonoBehaviour {
    public static GameManager instance { get; private set; }
    public bool isProp { get; private set; }
    public List<PlayingCard> hands { get; private set; }
    public int[] suitsOnHands { get; private set; }
    public int multiplier { private get; set; }
    public int numOfHands { private get; set; }
    public int gamblingChip { get; private set; }

    [SerializeField]
    private Deck deck;
    [SerializeField]
    private Transform flopsTransform;
    [SerializeField]
    private Transform pickersTransform;
    [SerializeField]
    private Button betButton;
    [SerializeField]
    private Button foldButton;
    [SerializeField]
    private PropButtons propButtons;
    [SerializeField]
    private GamblingChipBoard gamblingChipBoard;
    [SerializeField]
    private GameObject WinVFX;
    [SerializeField]
    private AudioClip winSFX;
    [SerializeField]
    private Summery summery;
    [SerializeField]
    private AudioClip againSFX;
    private bool isFlop;
    private bool canBet;

    // Initialize the data members
    public void Start() {
        instance = this;

        var savedChip = PlayerPrefs.HasKey("Gambling Chip") ? PlayerPrefs.GetInt("Gambling Chip") : 20000;
        ChangeChip(savedChip);
        betButton.interactable = canBet;

        Initialize();
    }


    // Initialize the game
    public void Initialize() {
        hands = new List<PlayingCard>();
        suitsOnHands = new int[4];
        multiplier = 1;
        numOfHands = 5;
        isFlop = false;

        var playingCards = new List<PlayingCard>(deck.playingCards);

        DealCard(flopsTransform);
        DealCard(pickersTransform);

        // Deal playing cards to the flop and pickers
        void DealCard(Transform transform) {
            foreach (Transform child in transform) {
                var playingCard = playingCards[Random.Range(0, playingCards.Count)];
                playingCards.Remove(playingCard);

                var playerCardImage = child.GetComponent<PlayingCardImage>();
                playerCardImage.playingCard = playingCard;
            }
        }
    }

    // Change the number of the gambling chip with int number
    public void ChangeChip(int number) {
        gamblingChip += number;
        PlayerPrefs.SetInt("Gambling Chip", gamblingChip);
        gamblingChipBoard.ChangeChipText(gamblingChip);

        canBet = gamblingChip >= 300;

        if (isFlop && propButtons.isActiveAndEnabled)
            propButtons.CheckEnoughChip(gamblingChip);
    }

    public void Bet() {
        if (!isFlop)
            StartCoroutine(Flop(true));
        else
            AllowPicking(true);

        ChangeChip(-300);
    }

    /*
     * The tasks in the flop is similar to one when resetting the game.
     * If bool flop is true, this function refers to the former; otherwise, it refers to the latter   
     */
    public IEnumerator Flop(bool flop) {
        betButton.interactable = false;
        yield return flop ? new WaitForSeconds(0.5f) : null;
        betButton.interactable = canBet;

        propButtons.SetActive(flop);
        foldButton.gameObject.SetActive(flop);

        FlipCards(flopsTransform, flop);

        if (flop)
            isFlop = true;
    }

    // The pickers can be picked depends on bool isAllowed
    public void AllowPicking(bool isAllowed) {
        foreach (Transform child in pickersTransform) {
            var playingCardImage = child.GetComponent<PlayingCardImage>();
            playingCardImage.AllowPicking(isAllowed);
        }

        betButton.interactable = !isAllowed && canBet;
        foldButton.interactable = !isAllowed;

        propButtons.SetActive(false);
    }

    // Check the rank of a player's hands, calculate the bonus and show the result
    public void CheckEndGame() {
        if (hands.Count != numOfHands) return;

        int i, j;
        for (i = 0; i < hands.Count; ++i) {
            var key = hands[i];
            for (j = i - 1; j >= 0 && hands[j].number > key.number; --j)
                hands[j + 1] = hands[j];
            hands[j + 1] = key;
        }

        string message = "Congrad! You get a ";
        var bonus = 0;
        var win = true;

        bool isFlush = false, isStraight = false;
        Suit maxSuit = Suit.Spade;
        for (i = 0; i < 4 && !isFlush; ++i)
            if (suitsOnHands[i] >= 5) {
                isFlush = true;
                maxSuit = (Suit)i;
            }

        List<PlayingCard> flushHands = new List<PlayingCard>();
        if (isFlush) {
            for (i = 0; i < numOfHands; ++i)
                if (hands[i].suit == maxSuit)
                    flushHands.Add(hands[i]);

            for (i = 0; i < numOfHands - 5 + 2 && !isStraight; ++i) {
                isStraight = true;
                for (j = i; j < 5 + i - 1 && isStraight; ++j) {
                    if (j == flushHands.Count - 1) {
                        if (flushHands[j].number != 13 && flushHands[0].number != 1)
                            isStraight = false;
                    }
                    else if (flushHands[j + 1].number - flushHands[j].number != 1)
                        isStraight = false;
                }
            }
        }

        if (isFlush && isStraight) {
            message += "Straight Flush!";
            bonus = 25500;
        }
        else {
            var count = 1;
            var max = new int[2];

            for (i = 0; i < numOfHands; ++i) {
                if (i != numOfHands - 1 && hands[i].number == hands[i + 1].number)
                    ++count;
                else {
                    if (count > max[0])
                        max[0] = count;
                    else if (count > max[1])
                        max[1] = count;

                    if (max[0] > max[1]) {
                        max[1] = max[0] - max[1];
                        max[0] = max[0] - max[1];
                        max[1] = max[0] + max[1];
                    }
                    count = 1;
                }
            }

            if (max[1] == 4) {
                message += "Four of a Kind!";
                bonus = 1627;
            }
            else if (max[1] == 3 && max[0] >= 2) {
                message += "Full house!";
                bonus = 264;
            }
            else {
                for (i = 0; i < numOfHands - 5 + 2 && !isStraight; ++i) {
                    isStraight = true;
                    count = 0;
                    for (j = i; j < numOfHands && isStraight && count < 4; ++j) {
                        if (j == numOfHands - 1) {
                            if (hands[j].number != 13 || hands[0].number != 1)
                                isStraight = false;
                        }
                        else if (hands[j + 1].number - hands[j].number != 1) {
                            if (hands[j + 1].number == hands[j].number)
                                continue;
                            isStraight = false;
                        }
                        else
                            ++count;
                    }
                }

                if (isFlush) {
                    message += "Flush!";
                    bonus = 198;
                }
                else if (isStraight) {
                    message += "Straight!";
                    bonus = 99;
                }
                else if (max[1] == 3) {
                    message += "Three of a kind";
                    bonus = 16;
                }
                else if (max[0] == 2 && max[1] == 2) {
                    message += "Two Pairs!";
                    bonus = 8;
                }
                else {
                    message = "Never mind, Lucky Fairy will bless you in next round!";
                    win = false;
                }
            }
        }



        if (win) {
            WinVFX.SetActive(true);
            AudioManager.instance.PlaySFX(winSFX, true);
        }

        betButton.interactable = false;
        foldButton.interactable = false;

        var total = bonus * multiplier;
        ChangeChip(total);
        StartCoroutine(summery.Show(message, total));
    }

    // Fold this round of the game
    public void Fold() {
        StartCoroutine(Flop(false));

        FlipCards(pickersTransform, false);

        propButtons.Interactable();

        Initialize();
    }

    // Play the game again
    public void Again() {
        Fold();

        summery.gameObject.SetActive(false);
        betButton.interactable = canBet;
        foldButton.interactable = true;

        AudioManager.instance.PlaySFX(againSFX, true);
    }

    // Reveal all pickers 1 second long and cover when Property A is purchased.
    public void OpenAllPickers() {
        isProp = true;
        FlipCards(pickersTransform, true);
        StartCoroutine(CloseAllPickers());

        IEnumerator CloseAllPickers() {
            yield return new WaitForSeconds(1f);
            FlipCards(pickersTransform, false);
            isProp = false;
        }
    }


    // Reveal or cover all cards under Transform cardsTransform according to bool faceUp
    private void FlipCards(Transform cardsTransform, bool faceUp) {
        PlayingCardImage playerCardImage;
        foreach (Transform child in cardsTransform) {
            playerCardImage = child.GetComponent<PlayingCardImage>();
            playerCardImage.Flip(faceUp);
        }
    }
}

