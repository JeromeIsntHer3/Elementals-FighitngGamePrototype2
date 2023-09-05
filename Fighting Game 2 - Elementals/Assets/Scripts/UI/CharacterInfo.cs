using System;
using UnityEngine;

[Serializable]
public class CharacterInfo
{
    public string Name;
    public GameObject DisplayPrefab;
    public GameObject GamePrefab;
    public Vector2 RelativeSpawn;
    public Sprite CharacterIcon;
}