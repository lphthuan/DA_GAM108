using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
	[SerializeField] private Transform spawnPoint; // Điểm hồi sinh

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (spawnPoint != null)
			{
				collision.transform.position = spawnPoint.position;
			}
			else
			{
				Debug.LogWarning("Chưa gán spawn point cho gai!");
			}
		}
	}
}
