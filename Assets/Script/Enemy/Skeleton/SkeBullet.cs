using UnityEngine;

public class SkeBullet : MonoBehaviour
{
	[SerializeField] private float speed = 10f;
	[SerializeField] private float lifetime = 3f;

	private Rigidbody2D rb;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;

		if (player != null)
		{
			// Tính hướng bay đến player
			Vector2 direction = (player.position - transform.position).normalized;

			// Gán vận tốc bay
			rb.velocity = direction * speed;

			// Tính góc xoay (vì sprite mũi tên mặc định quay sang phải → không cần +90)
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

			// Xoay mũi tên theo hướng bay
			transform.rotation = Quaternion.Euler(0f, 0f, angle);
		}
		else
		{
			// Nếu không tìm thấy player, bay thẳng theo hướng phải
			rb.velocity = transform.right * speed;
		}

		Destroy(gameObject, lifetime);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
			if (playerHealth != null)
			{
				playerHealth.TakeDamage(1);
			}
			Destroy(gameObject);
		}
		else if (!collision.CompareTag("Enemy") && !collision.isTrigger)
		{
			Destroy(gameObject);
		}
	}
}
