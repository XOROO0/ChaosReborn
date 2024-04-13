using DitzeGames.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickAction : MonoBehaviour
{
    private Transform enemy;

    public void SetUpKick(Transform enemy)
    {
        this.enemy = enemy;
    }

    public void PerformKick()
    {
        if (enemy == null)
            return;

        enemy.root.GetComponent<RagdollEnemy>().Push();
        CameraEffects.ShakeOnce(0.5f, 5);
    }
}
