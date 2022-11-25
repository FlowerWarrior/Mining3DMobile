using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PickaxesData", menuName = "PickaxesData", order = 1)]
public class PickaxesData : ScriptableObject
{
    public pickaxe[] pickaxes;
    public premiumPickaxe premium;
}

[Serializable]
public struct pickaxe
{
    public int cost;
    public int power;
    public Sprite sprite;
    public GameObject prefab;
}

[Serializable]
public struct premiumPickaxe
{
    public float costUSD;
    public int power;
    public Sprite sprite;
    public GameObject prefab;
}