using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EvolveGames;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private int currentHealth;

    [SerializeField]
    private TextMeshProUGUI healthValueText;

    [SerializeField]
    private Animator anim;
    public int CurrentHealth { get; }

    void Start()
    {
        currentHealth = maxHealth;
        healthValueText.text = currentHealth.ToString();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        healthValueText.text = currentHealth.ToString();
    }

    public void Die()
    {
        anim.SetBool("Dead", true);
        gameObject.GetComponent<PlayerController>().enabled = false;
    }
}
