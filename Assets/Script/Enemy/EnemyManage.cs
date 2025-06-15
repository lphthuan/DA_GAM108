using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManage : MonoBehaviour
{

    [SerializeField] private PlatformEffector2D platformEffector;
    [SerializeField] private Transform checkPoint1;
    [SerializeField] private Transform checkPoint2;
    [SerializeField] private Rigidbody2D platformRigidbody;
    [SerializeField] private float waitTimeBeforeReenable = 0.5f;
    [SerializeField] private float moveSpeed = 3f;

    private Transform targetPoint;

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
        if (checkPoint1 == null || checkPoint2 == null) return;
        Vector2 newPosition = Vector2.MoveTowards(platformRigidbody.position, targetPoint.position,
            moveSpeed * Time.fixedDeltaTime);
        platformRigidbody.MovePosition(newPosition);
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (targetPoint == checkPoint1)
            {
                targetPoint = checkPoint2;
            }
            else
            {
                targetPoint = checkPoint1;
            }
        }
        void Update()
        {
            if (checkPoint1 == null || checkPoint2 == null) return;

            Vector2 newPosition = Vector2.MoveTowards(platformRigidbody.position, targetPoint.position,
                moveSpeed * Time.fixedDeltaTime);
            platformRigidbody.MovePosition(newPosition);

            // Flip theo hướng di chuyển
            Vector2 direction = targetPoint.position - transform.position;
            if (direction.x > 0.01f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x < -0.01f)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                targetPoint = (targetPoint == checkPoint1) ? checkPoint2 : checkPoint1;
            }
        }

    }

    public void DropPlayer()
    {
        if (platformEffector != null)
        {
            platformEffector.rotationalOffset = 180f;
            Invoke(nameof(ResetEffector), waitTimeBeforeReenable);
        }
    }
    private void ResetEffector()
    {
        if (platformEffector != null)
        {
            platformEffector.rotationalOffset = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            bool playerOnTop = false;
            foreach (ContactPoint2D contact in contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    playerOnTop = true;
                    break;
                }
            }
            if (playerOnTop)
            {
                collision.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.parent == transform)
            {
                collision.transform.SetParent(null);
            }
        }
    }
}
