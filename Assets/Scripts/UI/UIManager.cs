using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private IntValue   scoreValue;
    [SerializeField] private FloatValue timeValue;
    [SerializeField] private FloatValue maxTimeValue;

    public void Play()
    {
        SceneMng sceneManager = FindObjectOfType<SceneMng>();
        sceneManager.StartScene("GameScene");

        scoreValue.SetValue(0);
        timeValue.SetValue(maxTimeValue.GetValue());
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
