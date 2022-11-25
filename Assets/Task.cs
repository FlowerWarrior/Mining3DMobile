    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Task : MonoBehaviour
{
    [HideInInspector] public int index;

    [SerializeField] Image buttonImage;
    [SerializeField] TextMeshProUGUI textTodo;
    [SerializeField] TextMeshProUGUI textValue;
    [SerializeField] TextMeshProUGUI textStatus;

    bool canClaim = false;
    task myTask;

    public static System.Action ClaimedTask;

    void OnEnable()
    {
        myTask = DataMgr.instance.GetTaskWithIndex(index);
        textTodo.text = myTask.text;
        textValue.text = myTask.value.ToString() + "$";

        if (DataMgr.instance.IsTaskClaimed(index))
        {
            // already claimed
            SetLookToClaimed();
        }
        else if (DataMgr.instance.isTaskActionCompleted(index))
        {
            // ready to claim
            SetLookReadyToClaim();
        }
        else
        {
            // default in progress
            buttonImage.color = new Color32(161, 161, 161, 255);
            textStatus.text = "IN PROGRESS";

            if (myTask.customId == 221)
            {
                textStatus.text = "CLICK HERE";
            }
        }
    }

    void SetLookReadyToClaim()
    {
        buttonImage.color = new Color32(81, 224, 88, 255);
        canClaim = true;
        textStatus.text = "CLAIM";
    }

    void SetLookToClaimed()
    {
        buttonImage.color = new Color32(62, 125, 67, 230);
        textStatus.text = "CLAIMED";

        textTodo.color = new Color32(255, 255, 255, 150);
        textValue.color = new Color32(255, 255, 255, 150);
        textStatus.color = new Color32(255, 255, 255, 150);
    }

    public void OnClicked()
    {
        if (canClaim)
        {
            DataMgr.instance.AddToMoney(myTask.value);
            DataMgr.instance.SetTaskClaimed(index);
            SetLookToClaimed();
            canClaim = false;

            AudioMgr.instance.PlayAudioSell();
            ClaimedTask?.Invoke();
        }
        if (DataMgr.instance.HasUserRatedApp() == false && myTask.customId == 221)
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.TomaszPiasecki.MiningGame3D.Clicker");
            DataMgr.instance.SetUserRatedApp();
            SetLookReadyToClaim();
        }
    }
}
