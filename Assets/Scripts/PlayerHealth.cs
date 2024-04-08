using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int MaxHealth = 100;

    [SerializeField] private Image healthBar;

    private int currentHealth;
    

    private void Start()
    {
        currentHealth = MaxHealth;
    }

    public void DamagePlayer(int amt)
    {
        currentHealth -= amt;

        healthBar.fillAmount = (float) currentHealth / MaxHealth;

        if (currentHealth <= 0)
            Die();
    }

    public void AddHealth(int amt)
    {
        currentHealth += amt;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        healthBar.fillAmount = (float)currentHealth / MaxHealth;
    }

    private void Update()
    {

    }

    private void Die()
    {
        SceneManager.LoadScene(1);
    }
}
