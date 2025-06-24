using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health Settings")]
    [SerializeField] private float startingHealth = 5f;

    public float CurrentHealth { get; private set; }
    private Animator anim;
    private bool isDead = false;

    protected virtual void Awake()
    {
		CurrentHealth = startingHealth;
		anim = GetComponent<Animator>();
	}

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0f, startingHealth);
        Debug.Log(CurrentHealth);
        if (CurrentHealth > 0)
        {
            anim.SetTrigger("IsHit"); //Tất cả quái đều để trigger IsHit
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

        if (anim != null)
        {
            anim.SetTrigger("IsDead");
		}

        foreach (var component in GetComponents<MonoBehaviour>())
        {
            if (component != this)
            {
                component.enabled = false;
            }
		}

        Collider2D[] colliders =GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
		}
        Destroy(gameObject, 1f);
	}
}
