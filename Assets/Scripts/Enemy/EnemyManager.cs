using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameLord.Pool;
using FrameLord.EventDispatcher;
using System;

public class EnemyManager : MonoBehaviour
{
    public Transform[] zombieStartPoint;
    public Transform[] devilStartPoint;
    public Transform[] juggerStartPoint;

    public float spawnTimer = 2f;

    private int devilSpawnRatio = 15;
    private int juggernautSpawnRatio = 50;
    private int difficultyRatio = 10;
    private float difficultyDivider = 0.9f;
    public int maxZombies = 0;

    public static int pointsZombie = 10;
    public static int pointsDevil = 150;
    public static int pointsJuggernaut = 500;

    private int enemiesCreated;
    private static int currentEnemies;
    private static bool isPlayerAlive;
    private float time = 0f;


    private Player player;

    public Transform topLeft, downRight; // margenes del mapa

    private State state;

    enum State
    {
        SpawningEnemies,
        FinishedGame
    }

    // Start is called before the first frame update
    void Start()
    {
        isPlayerAlive = true;
        SetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.SpawningEnemies:
                if (isPlayerAlive)
                {
                    time += Time.deltaTime;
                    if (time > spawnTimer)
                    {
                        time = 0;
                        if (currentEnemies <= maxZombies || maxZombies == 0)
                        {
                            SpawnEnemy();
                            enemiesCreated++;
                            currentEnemies++;
                            if (enemiesCreated != 0 && enemiesCreated % difficultyRatio == 0)
                            {
                                spawnTimer = spawnTimer * difficultyDivider;
                                player.RegenerateHealth();
                            }
                        }
                    }
                }
                else
                {
                    state = State.FinishedGame;
                }
                break;
            case State.FinishedGame:
                GameEventDispatcher.Instance.Dispatch(this, new EvnPlayerDied());
                break;
        }
        
    }

    private void SpawnEnemy() // Spawnea enemigos de cada tipo usando los que ya estan creados en el pool
    {
        if(enemiesCreated != 0 && enemiesCreated % devilSpawnRatio == 0)
        {
            var devil = PoolManager.Instance.GetPool("Devil").GetItem();
            if (devil != null)
            {
                Devil createdDevil = devil.gameObject.GetComponent<Devil>();
                createdDevil.startPosition = ChangeDevilSpawnPosition();
                devil.gameObject.transform.position = new Vector2(createdDevil.startPosition.position.x, createdDevil.startPosition.position.y);
                createdDevil.topLeft = topLeft;
                createdDevil.downRight = downRight;
                devil.gameObject.SetActive(true);
            }
            else // si no hay mas objetos en el pool para utilizar crea mas
            {
                var itemsN = PoolManager.Instance.GetPool("Devil").itemsToPreCreate + 1;
                PoolManager.Instance.GetPool("Devil").Create(itemsN);
                PoolManager.Instance.GetPool("Devil").itemsToPreCreate += 1;
            }
        }
        else if(enemiesCreated != 0 && enemiesCreated % juggernautSpawnRatio == 0)
        {
            var juggernaut = PoolManager.Instance.GetPool("Juggernaut").GetItem();
            if (juggernaut != null)
            {
                Juggernaut createdJuggernaut = juggernaut.gameObject.GetComponent<Juggernaut>();
                createdJuggernaut.startPosition = ChangeJuggernautSpawnPosition();
                juggernaut.gameObject.transform.position = new Vector2(createdJuggernaut.startPosition.position.x, createdJuggernaut.startPosition.position.y);
                createdJuggernaut.topLeft = topLeft;
                createdJuggernaut.downRight = downRight;
                juggernaut.gameObject.SetActive(true);
            }
            else // si no hay mas objetos en el pool para utilizar crea mas
            {
                var itemsN = PoolManager.Instance.GetPool("Juggernaut").itemsToPreCreate + 1;
                PoolManager.Instance.GetPool("Juggernaut").Create(itemsN);
                PoolManager.Instance.GetPool("Juggernaut").itemsToPreCreate += 1;
            }
        }
        else
        {
            var zombie = PoolManager.Instance.GetPool("Zombie").GetItem();
            if (zombie != null)
            {
                Zombie createdZombie = zombie.gameObject.GetComponent<Zombie>();
                createdZombie.startPosition = ChangeZombieSpawnPosition();
                zombie.gameObject.transform.position = new Vector2(createdZombie.startPosition.position.x, createdZombie.startPosition.position.y);
                createdZombie.topLeft = topLeft;
                createdZombie.downRight = downRight;
                zombie.gameObject.SetActive(true);
            }
            else // si no hay mas objetos en el pool para utilizar crea mas
            {
                var itemsN = PoolManager.Instance.GetPool("Zombie").itemsToPreCreate + 1;
                PoolManager.Instance.GetPool("Zombie").Create(itemsN);
                PoolManager.Instance.GetPool("Zombie").itemsToPreCreate += 1;
            }
        }
    }

    // Los Changes van variando la posicion en la que se spawnean los enemigos

    private int zombieSpawnPosition = 0;
    private Transform ChangeZombieSpawnPosition()
    {        
        if (zombieSpawnPosition >= zombieStartPoint.Length)
            zombieSpawnPosition = 0;
        return zombieStartPoint[zombieSpawnPosition++];
    }

    private int devilSpawnPosition = 0;
    private Transform ChangeDevilSpawnPosition()
    {
        if (devilSpawnPosition >= devilStartPoint.Length)
            devilSpawnPosition = 0;
        return devilStartPoint[devilSpawnPosition++];
    }

    private int juggernautSpawnPosition = 0;
    private Transform ChangeJuggernautSpawnPosition()
    {
        if (juggernautSpawnPosition >= juggerStartPoint.Length)
            juggernautSpawnPosition = 0;
        return juggerStartPoint[juggernautSpawnPosition++];
    }

    public static void ZombieDied()
    {
        currentEnemies--;
        GameManager.Instance.NotifyEnemyDeath(pointsZombie);
    }

    public static void DevilDied()
    {
        currentEnemies--;
        GameManager.Instance.NotifyEnemyDeath(pointsDevil);
    }

    public static void JuggernautDied()
    {
        currentEnemies--;
        GameManager.Instance.NotifyEnemyDeath(pointsJuggernaut);
    }
    public static void PlayerDied()
    {
        isPlayerAlive = false;
    }

    private void SetPlayer()
    {
        var go = GameObject.FindGameObjectsWithTag("Player");
        if (go.Length > 0)
            player = go[0].GetComponent<Player>();
    }
}
