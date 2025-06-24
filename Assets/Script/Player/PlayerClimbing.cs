using System.Collections;
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

	private LadderData currentLadderData;
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

				// Snap vào giữa thang (giữ nguyên Y)
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
			playerAnimator.SetBool("IsClimbing", true); // Đang leo
			playerAnimator.speed = 1f; // Ngay khi leo thang, đứng yên ở frame đầu
			StartCoroutine(PauseClimbAnimation());
		}
	}

	private IEnumerator PauseClimbAnimation()
	{
		yield return new WaitForSecondsRealtime(0.05f);
		if (isClimbing && Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.1f)
		{
			playerAnimator.speed = 0f;
		}
	}

	private void ClimbLadder()
	{
		float verticalInput = Input.GetAxisRaw("Vertical");

		// Giới hạn đỉnh thang: tự động thoát
		if (currentLadderData != null)
		{
			if (verticalInput > 0 && transform.position.y >= currentLadderData.topPoint.position.y)
			{
				ExitClimb();
				return;
			}
			else if (verticalInput < 0 && transform.position.y <= currentLadderData.bottomPoint.position.y)
			{
				// Ở đáy thang vẫn leo lên được, nhưng không cho rớt tiếp
				playerRigidbody.velocity = Vector2.zero;
				verticalInput = 0f;
			}
		}

		// Di chuyển trên thang
		playerRigidbody.velocity = new Vector2(0, verticalInput * climbSpeed);

		// Animation climbing
		if (playerAnimator != null)
		{
			bool isMoving = Mathf.Abs(verticalInput) > 0.01f;
			playerAnimator.speed = isMoving ? 1f : 0f;
		}
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
			playerAnimator.speed = 1f; // Trả lại tốc độ anim bình thường
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(transform.position, new Vector3(snapRange, 2f, 0));
	}
}
