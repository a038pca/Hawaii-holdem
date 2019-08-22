/*
 * Author: Wyatt Tse
 * Description: The script refers to the behaviour of GameObject Bet Button.
 * 
 *              AudioClip betSFX: The sound effect of clicking Bet Button.
 */

using UnityEngine;

public class BetButton : MonoBehaviour
{
    [SerializeField]
    private AudioClip betSFX;

    /*
     *  As AudioManager PlaySFX(AudioClip, bool) cannot be added to OnClick() in the editor directly,
     *  This function is used to handle the event of Bet Button when clicked.
     */
    public void Bet() {
        GameManager.instance.Bet();
        AudioManager.instance.PlaySFX(betSFX, false);
    }
}
