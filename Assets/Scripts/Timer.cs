using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;

    bool countDown = true;

    [SerializeField] List<Enemy> enemies;

    void Update()
    {
        CheckEnemies();

        if (enemies.Count == 0)
            return;

        elapsedTime += Time.deltaTime;
        int mins = Mathf.FloorToInt(elapsedTime / 60);
        int secs = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", mins, secs);
    }

    private void CheckEnemies()
    {
        foreach (Enemy enemy in enemies) 
        {
            if (enemy == null)
                enemies.Remove(enemy);
        }
    }
}
