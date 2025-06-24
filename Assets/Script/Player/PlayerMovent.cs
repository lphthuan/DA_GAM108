using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private Rigidbody2D playerRigidbody;
	[SerializeField] private float moveSpeed = 6f;
	[SerializeField] private float jumpForce = 10f;
	[SerializeField] private Animator playerAnimator;
	[SerializeField] private SpriteRenderer playerSpriteRenderer;
	[SerializeField] private BoxCollider2D playerCollider;
	[SerializeField] private LayerMask terrainLayer;
	[SerializeField] private Transform playerTransform;

	private bool jumpCheck = false;
	private GameObject currentPlatform = null;

	void Update()
	{
		Movement();
		UpdateAnimator();
	}

	private void Movement()
	{
		float horizontal = Input.GetAxis("Horizontal");
		playerRigidbody.velocity = new Vector2(horizontal * moveSpeed, playerRigidbody.velocity.y);
		if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
		{
			playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
		}
		if (IsGrounded() == true)
		{
			jumpCheck = true;
		}
		if (IsGrounded() == false && jumpCheck == true && Input.GetKeyDown(KeyCode.Space))
		{
			playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
			jumpCheck = false;
		}

		//Bấm S khi đứng trên platform sẽ rơi xuống
		if (Input.GetKeyDown(KeyCode.S))
		{
			if (currentPlatform != null)
			{
				PlatformController platformController = currentPlatform.GetComponent<PlatformController>();
				if (platformController != null)
				{
					platformController.DropPlayer();
					playerRigidbody.AddForce(Vector2.down * 2f, ForceMode2D.Impulse);
				}
			}
		}
	}

	private void UpdateAnimator()
	{
		var currentScale = playerTransform.localScale;
		if (playerRigidbody.velocity.x < 0)
		{
			playerTransform.localScale = new Vector3(-1f * Mathf.Abs(currentScale.x),
				currentScale.y, currentScale.z);
		}
		else if (playerRigidbody.velocity.x > 0)
		{
			playerTransform.localScale = new Vector3(1f * Mathf.Abs(currentScale.x),
				currentScale.y, currentScale.z);
		}

		if (Input.anyKey == true)
		{
			playerAnimator.SetBool("IsMove", true);
		}
		else
		{
			playerAnimator.SetBool("IsMove", false);
		}
		if (playerRigidbody.velocity.y > .1f)
		{
			playerAnimator.SetInteger("State", 1);
		}
		else if (playerRigidbody.velocity.y < -.1f)
		{
			playerAnimator.SetInteger("State", -1);
		}
		else
		{
			playerAnimator.SetInteger("State", 0);
		}
	}

	private bool IsGrounded()
	{
		return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f,
			Vector2.down, 0.1f, terrainLayer);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Platform"))
		{
			ContactPoint2D[] contacts = new ContactPoint2D[1];
			collision.GetContacts(contacts);
			if (contacts[0].normal.y > 0.5f)
			{
				currentPlatform = collision.gameObject;
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject == currentPlatform)
		{
			currentPlatform = null;
		}
	}
}