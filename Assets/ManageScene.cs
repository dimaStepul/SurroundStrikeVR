using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageScene : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public void GoToNextScene(int sceneIndex)
    {
        GoToNextSceneRoutine(sceneIndex);
    }
    public void GoToNextSceneRoutine(int sceneIndex)
    {
        // fadeScreen.FadeOut();
        // yield return new WaitForSeconds(fadeScreen.fadeDuration);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);

    }


}
