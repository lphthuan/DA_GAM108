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

	private bool jumpCheck = false;

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
	}

	private void UpdateAnimator()
	{
		if (playerRigidbody.velocity.x < 0)
		{
			playerSpriteRenderer.flipX = true;
		}
		else if (playerRigidbody.velocity.x > 0)
		{
			playerSpriteRenderer.flipX = false;
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
}