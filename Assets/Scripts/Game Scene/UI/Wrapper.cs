/*
 * Author: Wyatt Tse
 * Description: The scripts refers to the behaviour of GameObject Wrapper.
 *              
 *              Canvas Canvas: The canvas of the game.
 */
using UnityEngine;

public class Wrapper : MonoBehaviour
{
    [SerializeField]
    private Canvas Canvas;

    // Wrap all child of Wrapper into Canvas
    void Start()
    {
        var canvasRect = Canvas.GetComponent<RectTransform>();
        var rectTransform = GetComponent<RectTransform>();

        if (rectTransform.sizeDelta.x > canvasRect.sizeDelta.x)
            rectTransform.sizeDelta = new Vector2(canvasRect.sizeDelta.x, rectTransform.sizeDelta.y);
    }
}