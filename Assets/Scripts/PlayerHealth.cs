using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int MaxHealth = 100;
    [SerializeField] private Volume vol;

    [SerializeField] private Image healthBar;

    private float currentHealth;

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;


    private void Start()
    {
        currentHealth = MaxHealth;
    }

    public void DamagePlayer(float amt)
    {
        currentHealth -= amt;

        blinkTimer = blinkDuration;

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
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = lerp * blinkIntensity;

        Vignette vg;
        if(vol.profile.TryGet<Vignette>(out vg))
        {
            vg.intensity.value = intensity / 0.5f;
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(1);
    }
}
