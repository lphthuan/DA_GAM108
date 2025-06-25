using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[SerializeField] private GameObject gameOverPanel;
	[SerializeField] private TextMeshProUGUI timePlayedText;
	[SerializeField] private GameObject playerObject;

	[Header("Teleport Settings")]
	public List<GameObject> teleportingExit = new List<GameObject>();
	public List<Transform> teleportDestinations = new List<Transform>();

	[Header("Checkpoint Settings")]
	[SerializeField] private Transform spawnPointLv1;
	private Transform currentCheckpoint;
	private bool hasTeleported = false;

	private float startTime;
	private float timePlayed;
	private bool isGameOver = false;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		StartGame();
	}

	void Update()
	{
		if (!isGameOver)
		{
			timePlayed = Time.time - startTime;
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				RestartGame();
			}
		}
	}

	void StartGame()
	{
		isGameOver = false;
		startTime = Time.time;
		if (gameOverPanel != null)
		{
			gameOverPanel.SetActive(false);
		}
		Time.timeScale = 1f;
	}

	public void GameOver()
	{
		if (isGameOver) return;

		isGameOver = true;
		Time.timeScale = 0f;

		if (gameOverPanel != null)
		{
			gameOverPanel.SetActive(true);
		}

		if (timePlayedText != null)
		{
			timePlayedText.text = "Time: " + timePlayed.ToString("F2") + "s";
		}

		if (playerObject != null)
		{
			playerObject.SetActive(false);
		}
	}

	public void RestartGame()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		if (gameOverPanel != null)
		{
			gameOverPanel.SetActive(false);
		}
	}

	public void SetCurrentCheckpoint(Transform checkpoint)
	{
		currentCheckpoint = checkpoint;
		hasTeleported = true;
	}

	public Transform GetCheckpointOrSpawn()
	{
		if (hasTeleported && currentCheckpoint != null)
		{
			return currentCheckpoint;
		}
		else
		{
			return spawnPointLv1;
		}
	}
}