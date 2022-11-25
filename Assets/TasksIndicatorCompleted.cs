using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TasksIndicatorCompleted : MonoBehaviour
{
    [SerializeField] Image _tickImage;
    [SerializeField] GameObject _taskTutorial;

    void Start()
    {
        UpdateIndicator();
    }

    void OnEnable()
    {
        Task.ClaimedTask += UpdateIndicator;
        StartCoroutine(CheckLoop());
    }

    void OnDisable()
    {
        Task.ClaimedTask -= UpdateIndicator;
        StopCoroutine(CheckLoop());
    }

    void OnBlockDestroyed(int block, int layer)
    {
        UpdateIndicator();
    }

    void UpdateIndicator()
    {
        if (DataMgr.instance.areAnyTasksReadyToClaim())
        {
            _tickImage.enabled = true;

            if (PlayerPrefs.GetInt("taskTutorialCompleted", 0) == 0)
            {
                _taskTutorial.SetActive(true);
            }
        }
        else
        {
            _tickImage.enabled = false;
        }
    }

    IEnumerator CheckLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            UpdateIndicator();
        }
    }
}
