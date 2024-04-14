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

    void Start()
    {
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
        for (int i = 1; i <= 8; i++)
        {
            GameObject enemyClone = Instantiate(enemyPrefab);
            enemyClone.name = "Enemy" + i;
            enemyClone.transform.position = new Vector3(Random.Range(-10.0f, 10.0f), 1.5f, Random.Range(-10.0f, 10.0f));
            enemyClone.AddComponent<Enemy>();
            enemyClone.GetComponent<Enemy>().speed = Random.Range(0.001f, 0.003f);
            enemies.Add(enemyClone);
        }
    }

    void MoveEnemy(GameObject enemy)
    {
        Vector3 directionToPlayer = new Vector3(player.transform.position.x, enemy.transform.position.y, player.transform.position.z) - enemy.transform.position;
        enemy.transform.rotation = Quaternion.LookRotation(directionToPlayer);
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, new Vector3(player.transform.position.x, enemy.transform.position.y, player.transform.position.z), enemy.GetComponent<Enemy>().speed);

        if (Vector3.Distance(enemy.transform.position, player.transform.position) < 2.0f)
        {
            enemies.Remove(enemy);
            Destroy(enemy);
        }
        if (enemy.transform.position.y > 1.0f) // Change this value to your desired maximum height
        {
            enemy.transform.position = new Vector3(enemy.transform.position.x, 1.0f, enemy.transform.position.z);
        }
        if (Vector3.Dot(directionToPlayer.normalized, enemy.transform.forward) < 0.9f)
        {
            enemy.transform.position = new Vector3(Random.Range(-10.0f, 10.0f), 1.5f, Random.Range(-10.0f, 10.0f));
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
        public Break_Ghost breakGhost; // Add this


        void Start()
        {
            spawnDollAndMove = FindObjectOfType<SpawnDollAndMove>();
            breakGhost = GetComponent<Break_Ghost>();
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Cylinder"))
            {
                spawnDollAndMove.RemoveEnemy(gameObject);
                breakGhost.break_Ghost(); // Break the ghost
                                          // AudioSource audioSource = spawnDollAndMove.deathSound.GetComponent<AudioSource>(); // Get the AudioSource from spawnDollAndMove
                                          // audioSource.Play(); // Play the sound
                AudioSource.PlayClipAtPoint(spawnDollAndMove.deathSound, transform.position); // Play the sound
                Destroy(gameObject, 4f);
            }
        }
    }
}