using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : BaseUI
{
    [Header("MainUIButton")]
    public GameObject mainButtons;
    public Button gameStartButton;
    public Button settingButton;
    public Button gameExitButton;

    [Header("SelectLevelUI")]
    public GameObject selectLevelUI;
    public Image mapImage;
    public List<Sprite> mapSprites;
    public TextMeshProUGUI mapLevelText;
    private int currentMapLevel;
    public Button rightButton;
    public Button leftButton;
    public Button okButton;
    public Button closeSelectButton;

    [Header("SettingUI")]
    public GameObject settingUI;
    public Slider soundSlider;
    public Slider mouseSlider;
    public Button closeSettingButton;

    private int ClearLevel;

    void Start()
    {
        gameStartButton.onClick.AddListener(OnClickStartButton);
        settingButton.onClick.AddListener(OnClickSettingButton);
        gameExitButton.onClick.AddListener(OnClickExitButton);

        rightButton.onClick.AddListener(OnClickRightButton);
        leftButton.onClick.AddListener(OnClickLeftButton);
        okButton.onClick.AddListener(OnOKButton);
        closeSelectButton.onClick.AddListener(OnCloseSelectButton);
        
    }

    /// <summary>
    /// Ȱ��ȭ �ɶ� �ʱ�ȭ
    /// </summary>
    private void OnEnable()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        mainButtons.SetActive(true);
        selectLevelUI.SetActive(false);
        settingUI.SetActive(false);
        ClearLevel = PlayerPrefs.GetInt("Level");
        if (ClearLevel <= 0)
        {
            Debug.LogWarning("Ŭ����� ������ �����ϴ�.");
            ClearLevel = 1;
        }
        if (mapSprites.Count > 0)
        {
            mapImage.sprite = mapSprites[0];
            currentMapLevel = mapSprites.IndexOf(mapSprites[0])+1;
        }
        else
            Debug.Log("��������Ʈ�� ���̹����� ���� �ʾҽ��ϴ�. �ʱ�ȭ ����;;");
    }

    /// <summary>
    /// ���۹�ư������
    /// </summary>
    private void OnClickStartButton()
    {
        mainButtons.SetActive(false);
        selectLevelUI.SetActive(true);
    }

    /// <summary>
    /// ���ù�ư ������
    /// </summary>
    private void OnClickSettingButton()
    {
        mainButtons.SetActive(false);
        settingUI.SetActive(true);
    }

    /// <summary>
    /// ������ ��ư ������
    /// </summary>
    private void OnClickExitButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// ���ӹ�ư������ �ʹٲٴ� ������ ��ư������
    /// </summary>
    private void OnClickRightButton()
    {
        if (mapSprites == null)
        { Debug.LogWarning("�̹����� �����ϴ�."); }
        else
        {
            for (int i = 0; i < mapSprites.Count; i++)
            {
                if (mapImage.sprite == mapSprites[i])
                {
                    if (mapSprites.IndexOf(mapSprites[i]) + 1 == mapSprites.Count)
                    {
                        mapImage.sprite = mapSprites[0];
                        currentMapLevel = mapSprites.IndexOf(mapSprites[0]) + 1;
                        break;
                    }
                    else 
                    {
                        mapImage.sprite = mapSprites[i+1];
                        currentMapLevel = mapSprites.IndexOf(mapSprites[i+1]) + 1;
                        break;
                    }
                }
            }
            UpdateTextUI(currentMapLevel);
        }
    }


    /// <summary>
    /// ���ʹ�ư ������
    /// </summary>
    private void OnClickLeftButton()
    {
        if (mapSprites == null)
        { Debug.LogWarning("�̹����� �����ϴ�."); }
        else
        {
            for (int i = 0; i < mapSprites.Count; i++)
            {
                if (mapImage.sprite == mapSprites[i])
                {
                    if (mapSprites.IndexOf(mapSprites[i]) - 1 < 0)
                    {
                        mapImage.sprite = mapSprites[mapSprites.Count-1];
                        currentMapLevel = mapSprites.Count;
                        break;
                    }
                    else
                    {
                        mapImage.sprite = mapSprites[i - 1];
                        currentMapLevel = mapSprites.IndexOf(mapSprites[i - 1]) + 1;
                        break;
                    }
                }
            }
            UpdateTextUI(currentMapLevel);
        }
    }

    /// <summary>
    /// �ʷ��� �ؽ�Ʈ����
    /// </summary>
    /// <param name="i">�ʷ���</param>
    private void UpdateTextUI(int i)
    {
        mapLevelText.text = i.ToString();
    }

    /// <summary>
    /// �ʼ��������� �ݱ��ư
    /// </summary>
    private void OnCloseSelectButton()
    {
        selectLevelUI.gameObject.SetActive(false);
        mainButtons.gameObject.SetActive(true);
    }

    /// <summary>
    /// �� ���� ��ư-
    /// </summary>
    private void OnOKButton()
    {
        if (ClearLevel >= currentMapLevel)
        {
            GameManager.Instance.uiManager.HideUI<MainUI>();
            GameManager.Instance.stageChanger.LoadScene(currentMapLevel - 1);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Debug.Log("�����Ҽ� ���� ���Դϴ�. �������� Ŭ�����ϼ���");
        }
    }



}
