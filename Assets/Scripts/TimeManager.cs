using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SlowMo(float amt, float duration)
    {
        Time.timeScale = amt;

        Invoke(nameof(RevertSlowMo), duration);
    }

    private void RevertSlowMo()
    {
        Time.timeScale = 1f;
    }
}
