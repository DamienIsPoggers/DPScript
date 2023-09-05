using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;

public class BattleLoad : MonoBehaviour
{
    [SerializeField]
    Animator cam;
    byte[] sceneLoad = new byte[255];

    public IEnumerator battleLoad(string sceneToLoad, string p1, string p2)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
        scene.allowSceneActivation = false;

        while (scene.progress <= 0.9)
            yield return null;

        cam.Play("CameraDone");
        while (cam.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
            yield return null;

        scene.allowSceneActivation = true;
    }
}
