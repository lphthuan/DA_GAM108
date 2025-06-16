using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Setting")]
    [SerializeField] private float startingHealth = 3f;

    public float currentHealth { get; private set; }
    private Animator anim;
    private bool isDead;

    protected virtual void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0f, startingHealth);
        Debug.Log(currentHealth);
        if (currentHealth > 0)
        {
            //if (anim != null) anim.SetTrigger("IsHit");
        }
        else
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;

        isDead = true;

        //if (anim != null) anim.SetTrigger("IsDead");

        foreach (var component in GetComponents<MonoBehaviour>())
        {
            if (component != this) component.enabled = false;
        }
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
        Destroy(gameObject, 1f);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "PlayerBullet(Clone)")
        {
            TakeDamage(1);
        }
    }
}
