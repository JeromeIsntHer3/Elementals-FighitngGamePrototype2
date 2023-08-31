using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] CinemachineVirtualCamera menuCam;
    [SerializeField] CinemachineVirtualCamera characterSelectCam;
    [SerializeField] CinemachineVirtualCamera gameCam;
    [SerializeField] CinemachineTargetGroup targetGroup;

    private void Awake()
    {
        Instance = this;
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

    public void SetGamCams()
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
}