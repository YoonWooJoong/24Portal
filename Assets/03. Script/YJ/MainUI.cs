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
    public Slider bgmSoundSlider;
    public Slider sfxSoundSlider;
    public Slider mouseSlider;
    public Button closeSettingButton;

    public Camera cameraObject;
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

        bgmSoundSlider.onValueChanged.AddListener(BGMSoundControl);
        sfxSoundSlider.onValueChanged.AddListener(SFXSoundControl);
        mouseSlider.onValueChanged.AddListener(MouseControl);
        closeSettingButton.onClick.AddListener(OnCloseSettingButton);
        
    }


    public override void Initialize()
    {
        base.Initialize();
        mainButtons.SetActive(true);
        selectLevelUI.SetActive(false);
        settingUI.SetActive(false);
        ClearLevel = PlayerPrefs.GetInt("Level");
        MouseControl(0.2f);
        if (ClearLevel <= 0)
        {
            Debug.LogWarning("저장된레벨이 없습니다..");
            ClearLevel = 1;
        }
        if (mapSprites.Count > 0)
        {
            mapImage.sprite = mapSprites[0];
            currentMapLevel = mapSprites.IndexOf(mapSprites[0])+1;
        }
        else
            Debug.Log("맵이미지를 불러오지못했습니다;;");
        cameraObject.gameObject.SetActive(true);
    }

    /// <summary>
    /// 시작버튼
    /// </summary>
    private void OnClickStartButton()
    {
        mainButtons.SetActive(false);
        selectLevelUI.SetActive(true);
    }

    /// <summary>
    /// 세팅버튼
    /// </summary>
    private void OnClickSettingButton()
    {
        mainButtons.SetActive(false);
        settingUI.SetActive(true);
    }

    /// <summary>
    /// 나가기버튼
    /// </summary>
    private void OnClickExitButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// 맵선택 오른쪽버튼
    /// </summary>
    private void OnClickRightButton()
    {
        if (mapSprites == null)
        { Debug.LogWarning("맵이없습니다."); }
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
    /// 맵선택 왼쪽버튼
    /// </summary>
    private void OnClickLeftButton()
    {
        if (mapSprites == null)
        { Debug.LogWarning("맵이없습니다."); }
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
    /// 맵레벨 텍스트변경
    /// </summary>
    /// <param name="i">현재맵레벨</param>
    private void UpdateTextUI(int i)
    {
        mapLevelText.text = i.ToString();
    }

    /// <summary>
    /// 선택창 나가기버튼
    /// </summary>
    private void OnCloseSelectButton()
    {
        selectLevelUI.gameObject.SetActive(false);
        mainButtons.gameObject.SetActive(true);
    }

    /// <summary>
    /// 맵선택확인버튼
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
            Debug.Log("현재선택할수 없는 맵입니다.");
        }
    }

    private void BGMSoundControl(float value)
    {
        GameManager.Instance.soundManager.BGMVolumeControll(value);
    }

    private void SFXSoundControl(float value)
    {
        GameManager.Instance.soundManager.SFXVolumeControll(value);
    }

    private void MouseControl(float value)
    {
        GameManager.Instance.MouseSensitivty = value;
    }

    private void OnCloseSettingButton()
    {
        settingUI.gameObject.SetActive(false);
        mainButtons.gameObject.SetActive(true);
    }



}
