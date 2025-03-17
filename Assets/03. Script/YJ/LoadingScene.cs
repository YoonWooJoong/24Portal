using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] Slider loadingBar;
    [SerializeField] float time;
    [SerializeField] int SceneIndex;
    [SerializeField] GameObject loadingBarText;
    // Start is called before the first frame update
    void Start()
    {
        SceneIndex = GameManager.Instance.sceneLoadIndex;
        StartCoroutine(LoadScene(SceneIndex));
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        loadingBar.value = time;
        loadingBarText.transform.GetComponent<TextMeshProUGUI>().text = $"Loading...{(time * 100f).ToString("F2")}%";
    }

    IEnumerator LoadScene(int index)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(index);
        op.allowSceneActivation = false;
        yield return new WaitForSeconds(1.0f);
        op.allowSceneActivation = true;
    }
}
