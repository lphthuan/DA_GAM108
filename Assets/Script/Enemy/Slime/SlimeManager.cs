using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManage : MonoBehaviour
{
	[SerializeField] private Transform checkPoint1;
	[SerializeField] private Transform checkPoint2;
	[SerializeField] private Rigidbody2D platformRigidbody;
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private Animator anim;

	private Transform targetPoint;
	private bool isWaiting = false;

	void Start()
	{
		if (checkPoint1 != null && checkPoint2 != null)
		{
			transform.position = checkPoint1.position;
			targetPoint = checkPoint2;
		}
	}

	void Update()
	{
		if (checkPoint1 == null || checkPoint2 == null || isWaiting) return;

		Vector2 newPosition = Vector2.MoveTowards(platformRigidbody.position, targetPoint.position,
			moveSpeed * Time.deltaTime);

		platformRigidbody.MovePosition(newPosition);

		// Cập nhật animation di chuyển
		if (anim != null)
		{
			anim.SetBool("IsMove", true);
		}

		// Nếu gần tới điểm đích
		if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
		{
			StartCoroutine(WaitAtCheckpoint());
		}

		// Flip theo hướng
		Vector2 direction = targetPoint.position - transform.position;
		if (direction.x > 0.01f)
		{
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}
		else if (direction.x < -0.01f)
		{
			transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}
	}

	private IEnumerator WaitAtCheckpoint()
	{
		isWaiting = true;

		if (anim != null)
		{
			anim.SetBool("IsMove", false);
		}

		// Đợi 2 giây
		yield return new WaitForSeconds(3f);

		// Đảo hướng
		targetPoint = (targetPoint == checkPoint1) ? checkPoint2 : checkPoint1;
		isWaiting = false;
	}
}
