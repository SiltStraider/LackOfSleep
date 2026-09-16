using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VictoryChecker : MonoBehaviour
{
    public AudioClip winAudio;
    public AudioClip endTimeAudio;

    public PlaySoundEffects playSoundEffects;
    public PanelsController panelsController;

    [SerializeField] private PlayerSanity playerSanity;
    [SerializeField] private float menuLoadDelay = 2f;
    
    private bool isWinHandled;

    public void Win()
    {
        // Победа невозможна, если рассудок закончился
        if (playerSanity.CurrentSanity <= 0f)
        {
            return;
        }
        
        if (isWinHandled) return;
        isWinHandled = true;
        
        panelsController.SetActivePanelWin(true);
        playSoundEffects.PlayEffect(winAudio);
        playSoundEffects.PlayEffect(endTimeAudio);

        StartCoroutine(LoadMainMenuAfterDelay());
    }

    private IEnumerator LoadMainMenuAfterDelay()
    {
        yield return new WaitForSeconds(menuLoadDelay);
        SceneManager.LoadScene("Menu");
    }
}