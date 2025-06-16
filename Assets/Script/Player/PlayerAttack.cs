using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject playerBulletPrefab;
    [SerializeField] private Transform firePoint;

    public void PerformAttack()
    {
        GameObject bullet = Instantiate(playerBulletPrefab, firePoint.position, firePoint.rotation);
        PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();

        if (bulletScript != null)
        {
            Vector2 fireDirection = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
            bulletScript.SerDirection(fireDirection);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Animator anim = GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("IsAtk");
            }
        }
    }
}
