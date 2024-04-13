using DitzeGames.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : MonoBehaviour
{
    [SerializeField] GameObject kickPrefab;

    public void DoKick(Transform enemy)
    {
        kickPrefab.SetActive(true);
        kickPrefab.GetComponent<KickAction>().SetUpKick(enemy);
        kickPrefab.GetComponent<Animator>().Play("Kick");

    }
}
