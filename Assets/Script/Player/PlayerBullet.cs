﻿using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
	[SerializeField] private float speed = 15f;
	[SerializeField] private float lifetime = 2f;
	[SerializeField] private Rigidbody2D bulletRigidbody;

	void Start()
	{
		Destroy(gameObject, lifetime);
	}

	public void SetDirection(Vector2 direction)
	{
		bulletRigidbody.velocity = direction.normalized * speed;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
			if (enemy != null)
			{
				enemy.TakeDamage(1f);
			}
			Destroy(gameObject);
		}
		else if (collision.CompareTag("Terrain"))
		{
			Destroy(gameObject);
		}
	}
}