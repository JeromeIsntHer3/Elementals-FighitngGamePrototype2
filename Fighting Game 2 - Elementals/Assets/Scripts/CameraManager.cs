using Cinemachine;
using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] Camera MainCamera;
    [SerializeField] float gameInitialOrthoSize = 4.5f;
    [SerializeField] CinemachineVirtualCamera menuCam;
    [SerializeField] CinemachineVirtualCamera characterSelectCam;
    [SerializeField] CinemachineVirtualCamera gameCam;
    [SerializeField] CinemachineTargetGroup targetGroup;

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        GameManager.OnToMenu += SetCameraOrtho;
        GameManager.OnToCharacterSelect += SetCameraOrtho;
        GameManager.OnEnterGame += SetGameOrthoSize;
    }

    void OnDisable()
    {
        GameManager.OnToMenu -= SetCameraOrtho;
        GameManager.OnToCharacterSelect -= SetCameraOrtho;
        GameManager.OnEnterGame -= SetGameOrthoSize;
    }

    void SetCameraOrtho(object sender, EventArgs args)
    {
        gameCam.m_Lens.OrthographicSize = MainCamera.orthographicSize;
    }

    void SetGameOrthoSize(object sender, EventArgs args)
    {
        gameCam.m_Lens.OrthographicSize = gameInitialOrthoSize;
    }

    public void SetMenuCams()
    {
        menuCam.gameObject.SetActive(true);
        characterSelectCam.gameObject.SetActive(false);
        gameCam.gameObject.SetActive(false);
    }

    public void SetCharacterSelectCams()
    {
        menuCam.gameObject.SetActive(false);
        characterSelectCam.gameObject.SetActive(true);
        gameCam.gameObject.SetActive(false);
    }

    public void SetGameCams()
    {
        menuCam.gameObject.SetActive(false);
        characterSelectCam.gameObject.SetActive(false);
        gameCam.gameObject.SetActive(true);

        var framingTransposer = gameCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        framingTransposer.m_MaximumOrthoSize = 5.5f;
        framingTransposer.m_MinimumOrthoSize = 2.6f;
        framingTransposer.m_GroupFramingMode = CinemachineFramingTransposer.FramingMode.Horizontal;
    }

    public void SetTargetGroup(params Transform[] targets)
    {
        foreach (Transform t in targets)
        {
            targetGroup.AddMember(t, 1, .2F);
        }
    }

    public void ClearTargetGroup(params Transform[] targets)
    {
        Debug.Log("Clear Targets");
        foreach (Transform t in targets)
        {
            if (t == null) return;
            targetGroup.RemoveMember(t);
        }
    }

    public bool NoTargets()
    {
        return targetGroup.m_Targets == null;
    }
}