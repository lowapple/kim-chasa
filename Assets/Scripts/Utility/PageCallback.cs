using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageCallback : MonoBehaviour {
    public delegate void PageTurnCallback();
    public PageTurnCallback pageturncallback = null;

    public void Callback()
    {
        if (pageturncallback != null)
            pageturncallback();
    }
}