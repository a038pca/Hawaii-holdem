/*
 * Author: Wyatt Tse
 * Description: The script refers to the behaviours of the child of GameObject Prop Buttons
 *              
 *              Button propButtonA: The Button instance of GameObject Prop Button A
 *              Button propButtonB: The Button instance of GameObject Prop Button B
 *              Button propButtonC: The Button instance of GameObject Prop Button C
 */
using UnityEngine;
using UnityEngine.UI;

public class PropButtons : MonoBehaviour
{
    [SerializeField]
    private Button propButtonA;
    [SerializeField]
    private Button propButtonB;
    [SerializeField]
    private Button propButtonC;

    // Set all child to be interactable
    public void Interactable() {
        propButtonA.interactable = true;
        propButtonB.interactable = true;
        propButtonC.interactable = true;
    }

    // Set a child to be not interactable if int gamblingChip, the number of gambling chips, are fewer than the cost
    public void CheckEnoughChip(int gamblingChip) {
        if (gamblingChip < 100)
            propButtonA.interactable = false;
        if (gamblingChip < 150)
            propButtonB.interactable = false;
        if (gamblingChip < 300)
            propButtonC.interactable = false;
    }

    // Set whether Prop Buttons and all child is active according to bool active
    public void SetActive(bool active) {
        propButtonA.gameObject.SetActive(active);
        propButtonB.gameObject.SetActive(active);
        propButtonC.gameObject.SetActive(active);
        gameObject.SetActive(active);
    }
}
