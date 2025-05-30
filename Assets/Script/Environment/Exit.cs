using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
	private bool TeleportPlayer(GameObject playerObject)
	{
		GameManager gameManager = GameManager.Instance;

		if (gameManager == null)
		{
			Debug.LogError("GameManager instance không được tìm thấy.");
			return false;
		}

		for (int i = 0; i < gameManager.teleportingExit.Count; i++)
		{
			if (gameManager.teleportingExit[i] == gameObject)
			{
				if (i < gameManager.teleportDestinations.Count && gameManager.teleportDestinations[i] != null)
				{
					playerObject.transform.position = gameManager.teleportDestinations[i].position;
					return true;
				}
			}
		}

		return false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			GameObject playerObject = collision.gameObject;

			if (!TeleportPlayer(playerObject))
			{
				GameManager.Instance.GameOver();
				//Destroy(gameObject);
			}
		}
	}

}
