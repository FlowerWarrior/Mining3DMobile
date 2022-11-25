using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BlocksData", menuName = "BlocksData", order = 1)]
public class BlocksData : ScriptableObject
{
    public block startBlock;
    public layer[] blockInfo;
}

[Serializable]
public struct layer
{
    public string name;
    public int depth;
    public Color32 bgColor;
    public block[] blocks;
}

[Serializable]
public struct block
{
    public string name;
    public int value;
    public int chance;
    public string rarity;
    public int hp;
    public Color particleCol;
    public GameObject prefab;
    public AudioClip audioDestroyed;
}