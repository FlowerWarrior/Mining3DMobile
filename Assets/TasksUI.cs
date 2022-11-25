using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TasksUI : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] RectTransform content;

    void OnEnable()
    {
        PlayerPrefs.SetInt("taskTutorialCompleted", 1);

        // Move to available to claim
        for (int i = 0; i < DataMgr.instance.GetTasksCount(); i++)
        {
            Task taskScript = transform.GetChild(i).gameObject.GetComponent<Task>();

            if (DataMgr.instance.isTaskActionCompleted(i) && DataMgr.instance.IsTaskClaimed(i) == false)
            {
                int childIndex = i - 1;
                if (childIndex < 0)
                {
                    scrollRect.verticalScrollbar.value = 1f;
                    return;
                }
                print(childIndex);
                ScrollTo(transform.GetChild(childIndex).gameObject);
                print(childIndex);
                return;
            }
        }

        

        // If claimable task not found move to in progress task
        for (int i = 0; i < DataMgr.instance.GetTasksCount(); i++)
        {
            Task taskScript = transform.GetChild(i).gameObject.GetComponent<Task>();

            if (DataMgr.instance.IsTaskClaimed(i) == false)
            {
                int childIndex = i - 1;
                if (childIndex < 0)
                {
                    return;
                }

                ScrollTo(transform.GetChild(childIndex).gameObject);
                //VisualElement element = transform.GetChild(childIndex).gameObject.GetComponent<RectTransform>();
                //_scrollView.ScrollTo(element);
                return;
            }
        }
    }

    void ScrollTo(GameObject target)
    {
        //yield return new WaitForSeconds(0.1f);
        float height = content.rect.height;
        float pos = target.GetComponent<RectTransform>().anchoredPosition.y;
        scrollRect.verticalScrollbar.value = 1f + pos*1.1f / height;
    }
}
