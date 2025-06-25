using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] private float startHealth;
	public float currentHeath { get; private set; }

	[SerializeField] private GameManager gameManager;
	[SerializeField] private PlayerClimb playerClimb;

	[SerializeField] private float deathAnimationDuration = 0.5f;
	private bool isDead = false;

	private void Awake()
	{
		currentHeath = startHealth;
	}

	public void TakeDamage(float _damage)
	{
		if (isDead) return;

		currentHeath = Mathf.Clamp(currentHeath - _damage, 0, startHealth);

		if (playerClimb != null)
			playerClimb.ExitClimb();

		if (currentHeath <= 0 && !isDead)
		{
			StartCoroutine(HandleDeathSequence());
		}
		else
		{
			TeleportToCheckpoint();
		}
	}

	private IEnumerator HandleDeathSequence()
	{
		isDead = true;
		yield return new WaitForSeconds(deathAnimationDuration);
		gameManager.GameOver();
	}

	private void TeleportToCheckpoint()
	{
		if (gameManager == null) return;

		Transform checkpoint = gameManager.GetCheckpointOrSpawn();

		if (checkpoint != null)
		{
			transform.position = checkpoint.position;
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