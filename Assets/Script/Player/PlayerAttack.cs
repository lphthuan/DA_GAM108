using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	[Header("Attack Settings")]
	[SerializeField] private GameObject playerBulletPrefab;
	[SerializeField] private Transform firePoint;
	[SerializeField] private SpriteRenderer playerSpriteRenderer;
	[SerializeField] private Animator playerAnimator;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
			if (playerAnimator != null)
			{
				playerAnimator.SetTrigger("IsShoot");
			}
		}
	}

	public void PerformAttack()
	{
		GameObject bullet = Instantiate(playerBulletPrefab, firePoint.position, firePoint.rotation);
		PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();

		if (bulletScript != null)
		{
			Vector2 fireDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
			bulletScript.SetDirection(fireDirection);
		}
	}
}