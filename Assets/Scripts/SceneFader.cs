/*
 * Author: Wyatt Tse
 * Description: The script refers to the behaviour of GameObject SceneFader
 */
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    /*
     * Fade out by changing the colour to black gradually before loading to the scene with the name string sceneName.
     */
    public void LoadScene(string sceneName) {
        var image = GetComponent<UnityEngine.UI.Image>();
        image.raycastTarget = true;

        var anim = GetComponent<Animator>();
        anim.Play("Fade Out");

        StartCoroutine(LoadSceneCoroutine());

        System.Collections.IEnumerator LoadSceneCoroutine() {
            yield return new WaitForSeconds(1f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
