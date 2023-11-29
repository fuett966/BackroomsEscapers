using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EvolveGames;
using Mirror;
public class Health : NetworkBehaviour, IDamageable
{
    [SerializeField]
    private int maxHealth = 100;

    [SyncVar,SerializeField]
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
