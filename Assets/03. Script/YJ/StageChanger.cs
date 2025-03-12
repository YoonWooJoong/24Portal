using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChanger : MonoBehaviour
{
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextScene()
    {
        if (Application.CanStreamedLevelBeLoaded(SceneManager.GetActiveScene().buildIndex + 1))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else { Debug.LogWarning("다음씬이 없습니다."); }
    }

    public void GoFirstScene()
    {
        SceneManager.LoadScene(0);
    }
}
