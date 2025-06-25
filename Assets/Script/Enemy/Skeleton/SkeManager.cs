using System.Collections;
using UnityEngine;

public class SkeManager : MonoBehaviour
{
	[Header("Attack Settings")]
	[SerializeField] private float attackRange = 10f;
	[SerializeField] private float attackCooldown = 2f;
	[SerializeField] private Transform firePoint;
	[SerializeField] private GameObject skeBulletPrefab;

	private Transform player;
	private Animator anim;
	private float cooldownTimer = Mathf.Infinity;
	private EnemyHealth enemyHealth;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		enemyHealth = GetComponent<EnemyHealth>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update()
	{
		if (enemyHealth.CurrentHealth <= 0 || player == null) return;

		cooldownTimer += Time.deltaTime;
		float distanceToPlayer = Vector2.Distance(transform.position, player.position);

		if (distanceToPlayer < attackRange && cooldownTimer >= attackCooldown)
		{
			cooldownTimer = 0f;
			anim.SetTrigger("IsAtk"); // Gọi animation tấn công
		}
	}

	// Gọi hàm này từ Animation Event (trong animation "IsAtk")
	public void ShootBullet()
	{
		if (skeBulletPrefab != null && firePoint != null)
		{
			Instantiate(skeBulletPrefab, firePoint.position, Quaternion.identity);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
}
