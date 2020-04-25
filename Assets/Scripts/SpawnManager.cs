using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private float _spawnDelayEnemy = 5.0f;
    [SerializeField] private GameObject[] _powerUpPrefab;
    private int _numOfPowerups = 3; // This is the maximum of powerups we can spawn in (which works well with our Ransom.Range()-call).
    private bool _spawn = true;



    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_spawn)
        {
            Vector3 position = new Vector3(Random.Range(-9.0f, 9.0f), 8.0f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnDelayEnemy); // Wait n seconds and then continue executing the code.
            // yield return null; // Wait for one frame and then continue with this function/loop.
        }
        // We will never get over here, we are stuck in the loop for ever, but we 'yield' and let the rest of Unity do it's thing!
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_spawn)
        {
            yield return new WaitForSeconds(Random.Range(5.0f, 9.0f)); // Wait n seconds and then continue executing the code.
            Vector3 position = new Vector3(Random.Range(-9.0f, 9.0f), 8.0f, 0f);
            GameObject newPowerUp = Instantiate(_powerUpPrefab[Random.Range(0, _numOfPowerups)], position, Quaternion.identity);
            //newPowerUp.transform.parent = _powerUpContainer.transform;
        }
    }

    public void OnPlayerDeath()
    {
        _spawn = false;
    }

    public void StartSpawning()
    {
        StartCoroutine("SpawnEnemyRoutine");
        StartCoroutine(SpawnPowerUpRoutine()); // We can use the function or the function name when calling the coroutine to start.
    }
}
