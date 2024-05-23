using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnDollAndMove : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyPrefab;
    public GameObject weaponOnCrushedEnemy;
    public AudioClip deathSound;

    public AudioClip approachSound; // New AudioClip for the approach sound

    private List<GameObject> enemies = new List<GameObject>();
    public AudioClip gameOverAudio;

    private float enemyYcoordinate = 1.5f;

    // Add a counter for defeated enemies
    public int defeatedEnemiesCount = 0;
    public TextMeshPro defeatedEnemiesCountText;

    public GameObject gameOverText;


    private int timesPlayerCaught = 0;

    public int healthNumber = 3;
    public bool canMove = true;

    private float maxRadiusBoundary = 10.0f;


    private float triggerDistanceBetweenPlayerEnemy = 0.9f;

    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3f);
        SpawnEnemy();
    }

    void Update()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = rotationToPlayer;

        foreach (var enemy in enemies.ToArray())
        {
            MoveEnemy(enemy);
        }

        if (enemies.Count < 3)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        for (int i = 1; i <= 6; i++)
        {
            GameObject enemyClone = Instantiate(enemyPrefab);
            enemyClone.name = "Enemy" + i;
            enemyClone.transform.position = new Vector3(Random.Range(-maxRadiusBoundary, maxRadiusBoundary), enemyYcoordinate, Random.Range(-maxRadiusBoundary, maxRadiusBoundary));
            Enemy enemyComponent = enemyClone.AddComponent<Enemy>();
            enemyComponent.speed = Random.Range(0.001f, 0.003f);
            enemyComponent.spawnDollAndMove = this; // Pass the reference directly

            AudioSource enemyApproachSource = enemyClone.AddComponent<AudioSource>(); // Initialize the AudioSource on the enemy
            enemyApproachSource.clip = approachSound; // Set the AudioClip
            enemyApproachSource.volume = 0.0f;
            enemyApproachSource.Play(); // Start playing the sound

            enemies.Add(enemyClone);
        }
    }


    void MoveEnemy(GameObject enemy)
    {
        if (!canMove) // Add this line
            return;


        Vector3 directionToPlayer = new Vector3(player.transform.position.x, enemy.transform.position.y, player.transform.position.z) - enemy.transform.position;
        enemy.transform.rotation = Quaternion.LookRotation(directionToPlayer);
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, new Vector3(player.transform.position.x, enemy.transform.position.y, player.transform.position.z), enemy.GetComponent<Enemy>().speed);

        if (Vector3.Distance(enemy.transform.position, player.transform.position) < enemyYcoordinate)
        {
            enemies.Remove(enemy);
            Destroy(enemy);
            timesPlayerCaught++;
            if (timesPlayerCaught >= healthNumber)
            {
                GameOver();
            }
        }
        if (enemy.transform.position.y > enemyYcoordinate) // Change this value to your desired maximum height
        {
            enemy.transform.position = new Vector3(enemy.transform.position.x, enemyYcoordinate, enemy.transform.position.z);
        }
        if (Vector3.Dot(directionToPlayer.normalized, enemy.transform.forward) < triggerDistanceBetweenPlayerEnemy)
        {
            enemy.transform.position = new Vector3(Random.Range(-maxRadiusBoundary, maxRadiusBoundary), enemyYcoordinate, Random.Range(-maxRadiusBoundary, maxRadiusBoundary));
        }
    }


    public void GameOver()
    {
        Debug.Log("Game Over");
        canMove = false;
        gameOverText.SetActive(true);

        foreach (var enemy in enemies)
        {
            Destroy(enemy);

        }
        AudioSource.PlayClipAtPoint(gameOverAudio, player.transform.position); // Play the sound
        StartCoroutine(RestartGameAfterDelay(5)); // 3 seconds delay
    }

    IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        defeatedEnemiesCount++;
        defeatedEnemiesCountText.text = "Defeated Enemies: " + defeatedEnemiesCount;
        if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 1 && defeatedEnemiesCount >= 15)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public class Enemy : MonoBehaviour
    {
        public float speed;
        public ParticleSystem deathEffect;
        public bool isDead = false;
        public GameObject weaponOnCrushedEnemy;
        public SpawnDollAndMove spawnDollAndMove;
        public Break_Ghost breakGhost;

        private Vector3 lastPosition;
        private bool canMove = false;

        private float soundEffectDistance = 8.0f;

        private int delayBetweenSound = 2;

        void Start()
        {
            breakGhost = GetComponent<Break_Ghost>();
            lastPosition = transform.position;
            StartCoroutine(EnableMovementAfterDelay(1.0f)); // Add a delay before the enemy can start moving
        }

        IEnumerator EnableMovementAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            canMove = true;
        }

        private bool isPlaying = false; // Add this line

        void Update()
        {
            if (canMove && transform.position == lastPosition)
            {
                Vector3 directionToPlayer = spawnDollAndMove.player.transform.position - transform.position;
                transform.position = Vector3.MoveTowards(transform.position, directionToPlayer, speed);
            }

            if (transform.position.x < -spawnDollAndMove.maxRadiusBoundary || transform.position.x > spawnDollAndMove.maxRadiusBoundary || transform.position.z < -spawnDollAndMove.maxRadiusBoundary || transform.position.z > spawnDollAndMove.maxRadiusBoundary)
            {
                spawnDollAndMove.RemoveEnemy(gameObject);
                Destroy(gameObject);
            }

            // Update the last position
            lastPosition = transform.position;

            AudioSource approachSource = GetComponent<AudioSource>(); // Get the AudioSource from the enemy

            float distanceToPlayer = Vector3.Distance(transform.position, spawnDollAndMove.player.transform.position);

            // Only play the sound effect if the enemy is within soundEffectDistance units from the player
            if (distanceToPlayer <= soundEffectDistance && !isPlaying)
            {
                StartCoroutine(PlaySoundWithDelay(approachSource, distanceToPlayer, delayBetweenSound));
            }
        }

        IEnumerator PlaySoundWithDelay(AudioSource approachSource, float distanceToPlayer, float delay)
        {
            isPlaying = true;

            // Calculate the volume based on the distance to the player (closer = louder)
            approachSource.volume = 0.15f / distanceToPlayer;
            // Play the sound
            approachSource.Play();
            yield return new WaitForSeconds(delay);
            isPlaying = false;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Cylinder"))
            {
                Debug.Log("Enemy collided with Cylinder");
                spawnDollAndMove.RemoveEnemy(gameObject);
                breakGhost.break_Ghost();
                AudioSource.PlayClipAtPoint(spawnDollAndMove.deathSound, transform.position); // Play the sound
                Destroy(gameObject, 2f);
                Debug.Log("enemy destroyed");
            }
        }
    }
}