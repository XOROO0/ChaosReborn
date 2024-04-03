using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int MaxHealth = 100;

    private int currentHealth;

    private void Start()
    {
        currentHealth = MaxHealth;
    }

    public void DamagePlayer(int amt)
    {
        currentHealth -= amt;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        SceneManager.LoadScene(1);
    }
}
