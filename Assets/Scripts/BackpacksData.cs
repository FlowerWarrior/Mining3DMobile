using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BackpacksData", menuName = "BackpacksData", order = 1)]
public class BackpacksData : ScriptableObject
{
    public backpack[] backpacksInfo;
    public infBackpack infiniteBackpack;
}

[Serializable]
public struct backpack
{
    public int cost;
    public int capacity;
    public Sprite sprite;
}

[Serializable]
public struct infBackpack
{
    public float costUSD;
    public string capacity;
    public Sprite sprite;
}