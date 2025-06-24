using UnityEngine;

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

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Destroy(gameObject);
	}
}