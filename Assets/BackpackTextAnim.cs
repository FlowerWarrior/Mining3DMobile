using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackTextAnim : MonoBehaviour
{
    float animSpeed = 4f;
    Vector3 initialSize, targetSize;
    bool isInitialSize = true;

    private void Awake()
    {
        MiningMgr.BlockDestroyed += Animate;
    }

    private void Start()
    {
        initialSize = transform.localScale;
        targetSize = initialSize * 1.2f;
    }

    private void Update()
    {
        if (!isInitialSize)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, initialSize, Time.deltaTime * animSpeed);
            if (transform.localScale.x > initialSize.x - 0.01f && transform.localScale.x < initialSize.x + 0.01f)
            {
                isInitialSize = true;
            }
        }
    }

    void Animate(int a, int b)
    {
        transform.localScale = targetSize;
        isInitialSize = false;
    }
}
