using UnityEngine;

public class LevelExitPoint : MonoBehaviour
{
    [SerializeField] private int levelToLoadIndex;
    [SerializeField] private GameManager gameManager;

    private bool hasTriggered;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered || !other.CompareTag("Player")) return;
        hasTriggered = true;
        gameManager.levelManager.LoadNextLevel();
    }
}
