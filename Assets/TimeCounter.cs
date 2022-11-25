using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SecondInterval());
    }

    IEnumerator SecondInterval()
    {
        yield return new WaitForSeconds(1);
        DataMgr.instance.IncrementSecondsPlayed();
        StartCoroutine(SecondInterval());
    }
}
