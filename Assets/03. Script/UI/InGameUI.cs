using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using TMPro;

public class InGameUI : BaseUI
{
    [Header("InGameUI")]
    [SerializeField]private RectTransform backGroundRect;
    public Button menuButton;
    public Button homeButton;
    public Button restartButton;
    public GameObject SettingUI;

    public Vector3 backGroundPosition;

    private bool isMove = false;

    private void Start()
    {
        menuButton.onClick.AddListener(OnMenuButton);
        homeButton.onClick.AddListener(OnMainMenuButton);
        restartButton.onClick.AddListener(OnRestartButton);
        
    }

    public override void OnShow()
    {
        base.OnShow();
        Initialize();
    }
    private bool isInit;
    /// <summary>
    /// 초기화
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        backGroundRect.gameObject.SetActive(false);
        SettingUI.SetActive(false);
        if(!isInit)
        {
            backGroundPosition = backGroundRect.anchoredPosition;
            isInit = true;
        }
        backGroundRect.anchoredPosition = backGroundPosition;
    }

    /// <summary>
    /// 인게임 메뉴버튼 눌렀을때
    /// </summary>
    private void OnMenuButton()
    {
        if (!isMove)
        {
            isMove = true;
            if (!backGroundRect.gameObject.activeSelf)
            {
                backGroundRect.gameObject.SetActive(true);
                backGroundRect.DOAnchorPosY(backGroundRect.anchoredPosition.y - 120f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    isMove = false;
                });
            }
            else
            {
                backGroundRect.DOAnchorPosY(backGroundRect.anchoredPosition.y + 120f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    backGroundRect.gameObject.SetActive(false);
                    isMove = false;
                });
            }
        }
    }

    /// <summary>
    /// 재시작버튼 눌렀을때
    /// </summary>
    private void OnRestartButton()
    {
        GameManager.Instance.stageChanger.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// 메인메뉴 눌렀을때
    /// </summary>
    private void OnMainMenuButton()
    {
        GameManager.Instance.stageChanger.LoadScene(6);
    }
}
