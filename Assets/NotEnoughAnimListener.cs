using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotEnoughAnimListener : MonoBehaviour
{
    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        DataMgr.NotEnoughMoney += PlayAnim;
    }

    void OnDisable()
    {
        DataMgr.NotEnoughMoney -= PlayAnim;
    }

    void PlayAnim()
    {
        _animator.Play("A_NotEnoughMoney", 0);
        AudioMgr.instance.PlayAudioFullBackpack();
    }
}
