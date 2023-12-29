using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;
    public TextMeshProUGUI timerText;
    public int requiredCubes = 10; // Set the required number of cubes
    private float timerDuration = 120f; // 2 minutes in seconds
    public float currentTime;
    private int collectedCubes = 0;
    private bool isGameOver = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        // Initialize the timer
        currentTime = timerDuration;
    }

    private void Update()
    {
        if (!isGameOver)
        {
            // Update the timer
            UpdateTimerDisplay();

            // Check if the timer has reached 0 or the required cubes are not collected
            if (currentTime > 0f)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                // Timer has reached 0, and required cubes are not collected
                GameOver();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        // Update the TextMeshPro display
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timerString;
    }

    public void CollectCube()
    {
        // Call this method when the player collects a cube
        collectedCubes++;

        // Check if the required number of cubes is reached
        if (collectedCubes >= requiredCubes)
        {
            GameWon();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over - Time is up, and required cubes not collected!");
        // You can add additional game over actions here
        // Set game over state
        isGameOver = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameWon()
    {
        Debug.Log("Congratulations! You've collected all the required cubes in time.");
        // You can add additional win actions here

        // Stop the timer or perform other actions as needed

        // Set game over state
        isGameOver = true;
    }
}
