using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private TextMeshProUGUI healthValueText;
    [SerializeField] private Animator anim;
    public int CurrentHealth {get;}

    void Start()
    {
        currentHealth = maxHealth;
        healthValueText.text = currentHealth.ToString();

    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Челу нанесли урон, теперь у него: " + currentHealth + " хп");
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        healthValueText.text = currentHealth.ToString();
    }

    public void Die()
    {
        Debug.Log("Чел умер");
        anim.SetBool("Dead", true);
       // gameObject.SetActive(false);
    }

}
