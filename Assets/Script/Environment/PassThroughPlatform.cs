using System.Collections;
using UnityEngine;

public class PassThroughPlatform : MonoBehaviour
{
	private Collider2D _collider;
	private bool _playerOnPlatform;

	void Start()
	{
		_collider = GetComponent<Collider2D>();
	}

	void Update()
	{
		if (_playerOnPlatform && Input.GetAxisRaw("Vertical") < 0)
		{
			_collider.enabled = false;
			StartCoroutine(EnableCollider());
		}
	}

	private IEnumerator EnableCollider()
	{
		yield return new WaitForSeconds(0.5f);
		_collider.enabled = true;
	}

	private void SetPlayerOnPlatform(Collider2D other, bool value)
	{
		var player = other.gameObject.GetComponent<PlayerMovement>();
		if (player != null)
		{
			_playerOnPlatform = value;
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		SetPlayerOnPlatform(other.collider, true);
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		SetPlayerOnPlatform(other.collider, false);
	}
}
