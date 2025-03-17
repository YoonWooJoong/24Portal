using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    private Coroutine nowCo;

    public Elevator elevator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            elevator.IsIn = true;
            nowCo = StartCoroutine(DelayNextScene());

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (nowCo != null)
            {
                StopCoroutine(nowCo);
            }
        }
    }

    IEnumerator DelayNextScene()
    {
        GameManager.Instance.achieveManager.UnLockAchievement("Teleport");
        yield return new WaitForSeconds(2);
        GameManager.Instance.stageChanger.NextScene();
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
    }
}
