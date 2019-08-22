/*
 * Author: Wyatt Tse
 * Description: The script refers to the behaviour of GameObject Summery.
 * 
 *              Text messageText, bonusText: The text showing the message and the bonus.
 */
using UnityEngine;
using UnityEngine.UI;

public class Summery : MonoBehaviour
{
    private Text messageText;
    private Text bonusText;

    // Initialize all data members
    public void Awake() {
        var woodBoard = transform.GetChild(0);
        
        messageText = woodBoard.GetChild(0).GetComponent<Text>();
        bonusText = woodBoard.GetChild(1).GetComponent<Text>();
    }

    // Show Summery after 1 second
    public System.Collections.IEnumerator Show(string message, int bonus) {
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(true);
        messageText.text = message;
        bonusText.text = "Bonus: " + bonus;
    }
}
