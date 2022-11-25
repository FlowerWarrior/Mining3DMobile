using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class ScreenshotTaker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    IEnumerator screenshot()
    {
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot("Screenshots/" + DateTime.Now.Millisecond.ToString() + ".png");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(screenshot());
        }
    }
}