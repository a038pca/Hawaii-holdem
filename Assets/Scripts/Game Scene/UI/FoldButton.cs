/*
 * Author: Wyatt Tse
 * Description: The script refers to the behaviour of GameObject Fold Button.
 * 
 *              AudioClip flodSFX: The sound effect of clicking Fold Button.
 */
using UnityEngine;

public class FoldButton : MonoBehaviour
{
    [SerializeField]
    private AudioClip foldSFX;

    /*
     *  As void PlaySFX(AudioClip, bool) in AudioManager cannot be added to OnClick() in the editor directly.
     *  This function is used to play the sound effect and do the remains in GameManager instance when Fold Button is clicked.
     */
    public void Fold() {
        AudioManager.instance.PlaySFX(foldSFX, false);
        GameManager.instance.Fold();
    }
}
