using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDollAndMove : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyPrefab;
    public GameObject weaponOnCrushedEnemy;
    public AudioClip deathSound;
    private List<GameObject> enemies = new List<GameObject>();

    private float enemyYcoordinate = 1.5f;

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

        if (enemies.Count < 4)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        // for (int i = 1; i <= 8; i++)
        // {
        //     GameObject enemyClone = Instantiate(enemyPrefab);
        //     enemyClone.name = "Enemy" + i;
        //     enemyClone.transform.position = new Vector3(Random.Range(-10.0f, 10.0f), 1.5f, Random.Range(-10.0f, 10.0f));
        //     enemyClone.AddComponent<Enemy>();
        //     enemyClone.GetComponent<Enemy>().speed = Random.Range(0.001f, 0.003f);
        //     enemies.Add(enemyClone);
        // }


        for (int i = 1; i <= 8; i++)
        {
            GameObject enemyClone = Instantiate(enemyPrefab);
            enemyClone.name = "Enemy" + i;
            enemyClone.transform.position = new Vector3(Random.Range(-10.0f, 10.0f), enemyYcoordinate, Random.Range(-10.0f, 10.0f));
            Enemy enemyComponent = enemyClone.AddComponent<Enemy>();
            enemyComponent.speed = Random.Range(0.001f, 0.003f);
            enemyComponent.spawnDollAndMove = this; // Pass the reference directly
            enemies.Add(enemyClone);
        }
    }

    void MoveEnemy(GameObject enemy)
    {
        Vector3 directionToPlayer = new Vector3(player.transform.position.x, enemy.transform.position.y, player.transform.position.z) - enemy.transform.position;
        enemy.transform.rotation = Quaternion.LookRotation(directionToPlayer);
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, new Vector3(player.transform.position.x, enemy.transform.position.y, player.transform.position.z), enemy.GetComponent<Enemy>().speed);

        if (Vector3.Distance(enemy.transform.position, player.transform.position) < enemyYcoordinate)
        {
            enemies.Remove(enemy);
            Destroy(enemy);
        }
        if (enemy.transform.position.y > 3.0f) // Change this value to your desired maximum height
        {
            enemy.transform.position = new Vector3(enemy.transform.position.x, enemyYcoordinate, enemy.transform.position.z);
        }
        if (Vector3.Dot(directionToPlayer.normalized, enemy.transform.forward) < 0.9f)
        {
            enemy.transform.position = new Vector3(Random.Range(-10.0f, 10.0f), enemyYcoordinate, Random.Range(-10.0f, 10.0f));
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
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

        void Start()
        {

            breakGhost = GetComponent<Break_Ghost>();
            lastPosition = transform.position;
        }

        void Update()
        {
            if (transform.position == lastPosition)
            {
                Vector3 directionToPlayer = spawnDollAndMove.player.transform.position - transform.position;
                transform.position = Vector3.MoveTowards(transform.position, directionToPlayer, speed);
            }

            if (transform.position.x < -10.0f || transform.position.x > 10.0f || transform.position.z < -10.0f || transform.position.z > 10.0f)
            {
                spawnDollAndMove.RemoveEnemy(gameObject);
                Destroy(gameObject);
            }

            // Update the last position
            lastPosition = transform.position;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Cylinder"))
            {
                Debug.Log("Enemy collided with Cylinder");
                spawnDollAndMove.RemoveEnemy(gameObject);
                breakGhost.break_Ghost(); // Break the ghost
                                          // AudioSource audioSource = spawnDollAndMove.deathSound.GetComponent<AudioSource>(); // Get the AudioSource from spawnDollAndMove
                                          // audioSource.Play(); // Play the sound
                AudioSource.PlayClipAtPoint(spawnDollAndMove.deathSound, transform.position); // Play the sound
                Destroy(gameObject, 2f);
                Debug.Log("enemy destroyed");
            }
        }
    }
}