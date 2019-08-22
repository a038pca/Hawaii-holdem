/*
 * Author: Wyatt Tse
 * Description: The script refers to the behaviour of the GameObject Gambling Chip Board. 
 * 
 *              Text chipText : The text showing the number of gambling chips.
 */
using UnityEngine;
using UnityEngine.UI;


public class GamblingChipBoard : MonoBehaviour
{
    private Text chipText;
    
    void Awake() {
        chipText = transform.GetChild(0).GetComponent<Text>();
    }

    // Chnage the display of chipText according to int number representing the number of gambling chips.
    public void ChangeChipText(int number) {
        chipText.text = number.ToString();
    }
}
