using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RebirthButton : MonoBehaviour
{
    [SerializeField] GameObject panelRebirth;
    [SerializeField] GameObject panelNotYet;
    [SerializeField] GameObject lockedUI;
    [SerializeField] TextMeshProUGUI textDepthReq;
    
    private void OnEnable()
    {
        textDepthReq.text = "DEPTH " + GetCurrentReqDepth().ToString();

        if (DataMgr.instance.GetBlocksMined() >= GetCurrentReqDepth())
        {
            lockedUI.SetActive(false);
        }
    }

    public void OnClicked()
    {
        if (DataMgr.instance.GetBlocksMined() >= GetCurrentReqDepth())
        {
            panelRebirth.SetActive(true);
        }
        else
        {
            panelNotYet.SetActive(true);
        }
    }

    public int GetCurrentReqDepth()
    {
        int reqDepth = 0;
        int rebirthCounter = DataMgr.instance.GetRebirthCounter();
        switch (rebirthCounter)
        {
            case 0:
                reqDepth = 310; //310
                break;
            case 1:
                reqDepth = 450;
                break;
            case 2:
                reqDepth = 600;
                break;
            case 3:
                reqDepth = 800;
                break;
            case 4:
                reqDepth = 1100;
                break;
            default:
                reqDepth = rebirthCounter * 300;
                if (reqDepth > 9999)
                {
                    reqDepth = 9990; // constant after this level
                }
                break;
        }
        return reqDepth;
    }
}
