using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ImGuiNET;
using System.Text;

public class BattleLoad : MonoBehaviour
{
    [SerializeField]
    Animator camera;
    byte[] sceneLoad = new byte[255];

    void OnEnable()
    {
        ImGuiUn.Layout += Draw;
    }

    void OnDisable()
    {
        ImGuiUn.Layout -= Draw;
    }

    void Draw()
    {
        if(ImGui.Begin("LoadMenu"))
        {
            ImGui.InputText("Scene to load", sceneLoad, 255);
            if (ImGui.Button("Load"))
                StartCoroutine(battleLoad(Encoding.ASCII.GetString(sceneLoad).Replace("\0", string.Empty), "", ""));
            ImGui.End();
        }
    }

    public IEnumerator battleLoad(string sceneToLoad, string p1, string p2)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
        scene.allowSceneActivation = false;

        while (scene.progress <= 0.9)
            yield return null;

        camera.Play("CameraDone");
        while (camera.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
            yield return null;

        scene.allowSceneActivation = true;
    }
}
