using System;
using UnityEngine;

public class UIRootView : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Transform _uiSceneContainer;
    public void Awake()
    {
        HideLoadingScreen();
    }
    public void ShowLoadingScreen()
    {
        _loadingScreen.SetActive(true);
    }
    public void HideLoadingScreen()
    {
        _loadingScreen.SetActive(false);
    }
    public void AttachScreenUI(GameObject sceneUI)
    {
        ClearSceneUI();
        sceneUI.transform.SetParent(_uiSceneContainer, false);
    }

    private void ClearSceneUI()
    {
        int childrenCount = _uiSceneContainer.childCount;
        for (int i = 0; i < childrenCount; i++)
        {
            Destroy(_uiSceneContainer.GetChild(i).gameObject);
        }
    }
}
