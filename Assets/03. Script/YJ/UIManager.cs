using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private const string UIPrefabPath = "UI/";
    private Dictionary<string, BaseUI> activeUIs = new Dictionary<string, BaseUI>();

    public T ShowUI<T>() where T : BaseUI
    {
        string uiName = typeof(T).Name;

        if (activeUIs.ContainsKey(uiName))
        {
            activeUIs[uiName].OnShow();
            return activeUIs[uiName] as T;
        }

        GameObject uiPrefab = Resources.Load<GameObject>($"{UIPrefabPath}{uiName}");
        if (uiPrefab == null)
        {
            return null;
        }

        GameObject uiInstance = Instantiate(uiPrefab,transform);
        T uiComponent = uiInstance.GetComponent<T>();
        if (uiComponent == null)
        {
            Destroy(uiInstance);
            return null;
        }

        activeUIs.Add(uiName, uiComponent);
        uiComponent.Initialize();
        uiComponent.OnShow();

        return uiComponent;
    }

    public void HideUI<T>() where T : BaseUI
    {
        string uiName = typeof (T).Name;

        if (activeUIs.TryGetValue(uiName, out BaseUI ui))
        {
            ui.OnHide();
        }
        else
        {
            Debug.LogWarning("UI가 활성화 상태가 아닙니다.");
        }
    }
}
