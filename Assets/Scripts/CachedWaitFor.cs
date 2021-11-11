using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CachedWaitFor
{
    private static readonly Dictionary<float, WaitForSeconds> cachedWaitForSeconds = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWaitForSeconds(float time)
    {
        time = Mathf.Round(time * 1000) / 1000;
        if (cachedWaitForSeconds.ContainsKey(time))
            return cachedWaitForSeconds[time];
        cachedWaitForSeconds.Add(time, new WaitForSeconds(time));
        return cachedWaitForSeconds[time];
    }

    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
}
