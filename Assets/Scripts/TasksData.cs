using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tasks", menuName = "Tasks", order = 1)]
public class TasksData : ScriptableObject
{
    [SerializeField] public task[] tasks; 
}


[System.Serializable]
public struct task
{
    public int customId;
    public string text;
    public int value;
}