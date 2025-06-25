using System.Collections;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
	[Header("Climbing Settings")]
	[SerializeField] private float climbSpeed = 3f;
	[SerializeField] private float snapRange = 0.5f;
	[SerializeField] private LayerMask ladderLayer;
	[SerializeField] private LayerMask platformLayer;

	[Header("References")]
	[SerializeField] private Rigidbody2D playerRigidbody;
	[SerializeField] private Animator playerAnimator;
	[SerializeField] private Collider2D playerCollider;
	[SerializeField] private PlayerMovement playerMovement;
	[SerializeField] private PlayerAttack playerAttack;

	private LadderData currentLadderData;
	private bool isClimbing = false;
	private Transform currentLadder;
	private float waitForAnimation = 0f;
	private bool isIgnoringPlatform = false;

	void Update()
	{
		if (!isClimbing)
		{
			if (Input.GetKeyDown(KeyCode.F))
				TrySnapToLadder();
		}
		else
		{
			ClimbLadder();

			if (Input.GetKeyDown(KeyCode.Space))
				ExitClimb();
		}
	}

	private void TrySnapToLadder()
	{
		Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(snapRange, 2f), 0, ladderLayer);

		foreach (var hit in hits)
		{
			if (hit.CompareTag("Ladder"))
			{
				currentLadder = hit.transform;
				currentLadderData = hit.GetComponent<LadderData>();
				if (currentLadderData == null) return;

				Vector3 snapPos = new Vector3(hit.bounds.center.x, transform.position.y, transform.position.z);
				transform.position = snapPos;

				EnterClimb();
				break;
			}
		}
	}

	private void EnterClimb()
	{
		isClimbing = true;
		playerRigidbody.gravityScale = 0f;
		playerRigidbody.velocity = Vector2.zero;

		if (playerMovement != null) playerMovement.enabled = false;
		if (playerAttack != null) playerAttack.enabled = false;

		if (playerAnimator != null)
		{
			playerAnimator.SetBool("IsClimbing", true);
			playerAnimator.speed = 1f;
			waitForAnimation = 1;
			StartCoroutine(PauseClimbAnimation());
		}
	}

	private IEnumerator PauseClimbAnimation()
	{
		yield return new WaitForSecondsRealtime(0.3f);
		if (isClimbing && Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.1f)
		{
			waitForAnimation = 0;
			playerAnimator.speed = 0f;
		}
	}

	private void ClimbLadder()
	{
		float verticalInput = Input.GetAxisRaw("Vertical");

		// Giới hạn trên/dưới
		if (currentLadderData != null)
		{
			if (verticalInput > 0 && transform.position.y >= currentLadderData.topPoint.position.y)
			{
				ExitClimb();
				return;
			}
			else if (verticalInput < 0 && transform.position.y <= currentLadderData.bottomPoint.position.y)
			{
				playerRigidbody.velocity = Vector2.zero;
				verticalInput = 0f;
			}
		}

		playerRigidbody.velocity = new Vector2(0, verticalInput * climbSpeed);

		// 👉 Nếu đang nhấn ↓ và chưa ignore → kiểm tra nền dưới và bỏ va chạm
		if (verticalInput < -0.1f && !isIgnoringPlatform)
		{
			TryIgnorePlatformBelow();
		}

		// Animation climbing
		if (playerAnimator != null)
		{
			bool isMoving = Mathf.Abs(verticalInput) > 0.01f;
			playerAnimator.speed = isMoving ? 1f : waitForAnimation;
		}
	}

	private void TryIgnorePlatformBelow()
	{
		// Tạo box nhỏ dưới chân player để kiểm tra nền
		Vector2 boxCenter = (Vector2)transform.position + Vector2.down * 0.55f;
		Vector2 boxSize = new Vector2(0.5f, 0.1f);

		Collider2D platform = Physics2D.OverlapBox(boxCenter, boxSize, 0f, platformLayer);

		if (platform != null && platform != playerCollider)
		{
			StartCoroutine(IgnorePlatformTemporarily(platform));
		}
	}

	private IEnumerator IgnorePlatformTemporarily(Collider2D platform)
	{
		isIgnoringPlatform = true;

		if (playerCollider != null && platform != null)
		{
			Physics2D.IgnoreCollision(playerCollider, platform, true);
			yield return new WaitForSeconds(0.5f);
			if (platform != null)
				Physics2D.IgnoreCollision(playerCollider, platform, false);
		}

		isIgnoringPlatform = false;
	}

	private void ExitClimb()
	{
		isClimbing = false;
		playerRigidbody.gravityScale = 2f;

		if (playerMovement != null) playerMovement.enabled = true;
		if (playerAttack != null) playerAttack.enabled = true;

		if (playerAnimator != null)
		{
			playerAnimator.SetBool("IsClimbing", false);
			playerAnimator.speed = 1f;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube((Vector2)transform.position + Vector2.down * 0.55f, new Vector2(0.5f, 0.1f));
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(transform.position, new Vector3(snapRange, 2f, 0));
	}
}
