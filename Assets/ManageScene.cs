using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageScene : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public void GoToNextScene(int sceneIndex)
    {
        StartCoroutine(GoToNextSceneRoutine(sceneIndex));
    }
    IEnumerator GoToNextSceneRoutine(int sceneIndex)
    {
        // fadeScreen.FadeOut();
        // yield return new WaitForSeconds(fadeScreen.fadeDuration);
        yield return new WaitForSeconds(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);

    }


}
