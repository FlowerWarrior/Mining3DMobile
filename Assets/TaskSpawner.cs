using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TaskSpawner : MonoBehaviour
{
    [SerializeField] GameObject taskPrefab;
    [SerializeField] bool buttonGenerate = false;

    void Update()
    {
        if (buttonGenerate)
        {
            buttonGenerate = false;

            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < DataMgr.instance.GetTasksCount(); i++)
            {
                GameObject newTask = Instantiate(taskPrefab, Vector3.zero, Quaternion.identity, transform);
                newTask.GetComponent<Task>().index = i;
            }
        }

        if (Application.isPlaying)
        {
            this.enabled = false;
        }
    }
}
