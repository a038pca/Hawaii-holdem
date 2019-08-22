/*
 * Author: Wyatt Tse
 * Description: The script is used to handle playing of different sounds.
 *              As void PlaySFX(AudioClip, bool) which has 2 arguements cannot be added to OnClick() in a button with the editor directly and the function is called by many scripts,
 *              static AudioManager instance is set for convenient access. 
 * 
 *              AudioClip BGM: The background music
 *              AudioSource BGMAudioSource, SFXAudioSource: Controls the background music and a sound effect
 */
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private AudioClip BGM;
    private AudioSource BGMAudioSource;
    private AudioSource SFXAudioSource;

    /* 
     * Instantiate instance, BGMAudioSource and SFXAudioSource.
     * Initialize BGMAudioSource with BGM and play background music.
     */  
    void Start()
    { 
        instance = this;
        BGMAudioSource = gameObject.AddComponent<AudioSource>();
        SFXAudioSource = gameObject.AddComponent<AudioSource>();

        BGMAudioSource.clip = BGM;
        BGMAudioSource.loop = true;
        BGMAudioSource.volume = 0.4f;
        BGMAudioSource.Play();
    }


    /* 
     * Play the sound effect a time. AudioClip SFX refers to the sound effect.
     * If decreaseBGMVolume is true, the volume of BGM decrease and change back after the sound effect is played.
     */
    public void PlaySFX(AudioClip SFX, bool decreaseBGMVolume) {
        SFXAudioSource.PlayOneShot(SFX);
        if (decreaseBGMVolume)
            StartCoroutine(AmplifyBGMCoroutine());

        System.Collections.IEnumerator AmplifyBGMCoroutine() {
            BGMAudioSource.volume = 0.2f;
            yield return new WaitForSeconds(SFX.length);
            BGMAudioSource.volume = 0.4f;
        }
    }
}
