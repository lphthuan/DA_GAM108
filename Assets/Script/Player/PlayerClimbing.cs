using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
	[Header("Climbing Settings")]
	[SerializeField] private float climbSpeed = 3f;
	[SerializeField] private float snapRange = 0.5f;
	[SerializeField] private LayerMask ladderLayer;

	[Header("References")]
	[SerializeField] private Rigidbody2D playerRigidbody;
	[SerializeField] private Animator playerAnimator;
	[SerializeField] private Collider2D playerCollider;
	[SerializeField] private PlayerMovement playerMovement;
	[SerializeField] private PlayerAttack playerAttack;

	private bool isClimbing = false;
	private Transform currentLadder;

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
				ExitClimb(); // Cho phép nhảy ra khỏi thang
		}
	}

	private void TrySnapToLadder()
	{
		// Check các collider xung quanh trong snapRange và cao 2 đơn vị
		Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(snapRange, 2f), 0, ladderLayer);

		foreach (var hit in hits)
		{
			if (hit.CompareTag("Ladder"))
			{
				currentLadder = hit.transform;

				// Snap về giữa thang, giữ nguyên trục Y
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
			playerAnimator.SetBool("IsClimbing", true);
	}

	private void ClimbLadder()
	{
		float verticalInput = Input.GetAxisRaw("Vertical");
		playerRigidbody.velocity = new Vector2(0, verticalInput * climbSpeed);

		// Đứng yên nếu không bấm lên/xuống
		if (Mathf.Abs(verticalInput) < 0.1f)
			playerRigidbody.velocity = Vector2.zero;

		// Gọi animation leo thang
		if (playerAnimator != null)
			playerAnimator.SetBool("IsClimbing", true);
	}

	private void ExitClimb()
	{
		isClimbing = false;
		playerRigidbody.gravityScale = 3f;

		if (playerMovement != null) playerMovement.enabled = true;
		if (playerAttack != null) playerAttack.enabled = true;

		if (playerAnimator != null)
			playerAnimator.SetBool("IsClimbing", false);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(transform.position, new Vector3(snapRange, 2f, 0));
	}
}
