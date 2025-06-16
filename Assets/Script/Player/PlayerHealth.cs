using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float startHealth;
    public float currentHeath { get; private set; }

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private float deathAnimationDuration = 1.5f;
    private bool isDead = false;

    private void Awake()
    {
        currentHeath = startHealth;
    }

    public void TakeDamage(float _damage)
    {
        if (isDead) return;
        currentHeath = Mathf.Clamp(currentHeath - _damage, 0, startHealth);// chỉ xảy ra số trong kh
        if (currentHeath <= 0 && !isDead)
        {
            StartCoroutine(HandleDeathSequence());
        }

    }

    private IEnumerator HandleDeathSequence()
    {
        isDead = true;

        yield return new WaitForSeconds(deathAnimationDuration);
        gameManager.GameOver();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(1);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap") || other.CompareTag("Enemy"))
        {
            TakeDamage(1);            
        }
    }
}
