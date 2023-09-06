using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Range(0f, 1f)] public float masterVolume;
    [Range(0f, 1f)] public float sfxVolume;
    [Range(0f, 1f)] public float musicVolume;
    [Range(0f, 1f)] public float ambienceVolume;

    Bus masterBus;
    Bus sfxBus;
    Bus ambienceBus;
    Bus musicBus;

    List<EventInstance> eventInstances = new ();

    EventInstance ambienceEventInstance;
    EventInstance musicEventInstance;

    void Awake()
    {
        Instance = this;

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
    }

    void Start()
    {
        InitAmbience(FModEvents.Instance.ForestAmbience);
        InitBGM(FModEvents.Instance.MusicTrack);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference sound)
    {
        EventInstance evtInstance = RuntimeManager.CreateInstance(sound);
        eventInstances.Add(evtInstance);
        return evtInstance;
    }

    public void InitAmbience(EventReference ambienceSound)
    {
        ambienceEventInstance = CreateEventInstance(ambienceSound);
        ambienceEventInstance.start();
    }

    public void InitBGM(EventReference musicSound)
    {
        musicEventInstance = CreateEventInstance(musicSound);
        musicEventInstance.start();
    }

    public void SetBGM(LevelBGM bgm)
    {
        musicEventInstance.setParameterByName("GameState", (float) bgm);
    }

    public void CleanUp()
    {
        foreach(EventInstance evtInstance in eventInstances)
        {
            evtInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            evtInstance.release();
        }
    }

    void OnDestroy()
    {
        CleanUp();
    }
}