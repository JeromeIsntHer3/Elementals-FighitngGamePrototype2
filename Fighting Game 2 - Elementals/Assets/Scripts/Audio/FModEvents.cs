using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FModEvents : MonoBehaviour
{
    [field: Header("Level")]
    [field: SerializeField] public EventReference ForestAmbience { get; private set; }
    [field: SerializeField] public EventReference MusicTrack { get; private set; }


    [field: Header("Fire Knight Sound Events")]
    [field: SerializeField] public EventReference FKSwordSlash { get; private set; }

    [field: Header("Leaf Ranger Sound Events")]
    [field: SerializeField] public EventReference LFShootArrow { get; private set; }

    [field: Header("Metal Bladekeeper Sound Events")]
    [field: SerializeField] public EventReference MBDaggerSlice { get; private set; }
    [field: SerializeField] public EventReference MBDaggerThrow { get; private set; }

    [field: Header("Crystal Mauler Sound Events")]
    [field: SerializeField] public EventReference CRHammerSwing { get; private set; }


    [field: Header("General SFX")]
    [field: SerializeField] public EventReference BlockHit { get; private set; }
    [field: SerializeField] public EventReference Hit { get; private set; }

    public static FModEvents Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
}
