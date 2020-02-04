using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;
using Utils;

public class EnemySpawner : MonoBehaviour
{
	public LevelVariable CurrentLevel;
	public EnemyRuntimeSet CurrentEnemies;
	public Transform SpawnPosition;

	private bool _spawningDone
	{
		get { return _enemyOrder.Count > 0; }
	}
	
	private float _spawnTimer;

	private Queue<Enemy> _enemyOrder;

	private void OnEnable()
	{
		Reset();
	}

	public void Reset()
	{
		_spawnTimer = 0;

		_enemyOrder = new Queue<Enemy>();
		
		for (int i = 0; i < CurrentLevel.CurrentValue.EnemyCount; i++)
		{
			_enemyOrder.Enqueue(CurrentLevel.CurrentValue.Enemies.RandomElement());
		}
		
		if(CurrentLevel.CurrentValue.Boss != null)
			_enemyOrder.Enqueue(CurrentLevel.CurrentValue.Boss);
	}
	
	private void Update()
	{
		if(CurrentLevel.CurrentValue == null || _spawningDone)
			return;

		_spawnTimer += Time.deltaTime;
		if (_spawnTimer > CurrentLevel.CurrentValue.SpawnDelay)
		{
			if (CurrentEnemies.Count < CurrentLevel.CurrentValue.MaxEnemiesOnScreen)
			{
				SpawnEnemy(_enemyOrder.Dequeue());
			}				
		}
	}

	public void SpawnEnemy(Enemy enemy)
	{
		GameObject go = GameObject.Instantiate(enemy.gameObject, SpawnPosition);
		Enemy spawnedEnemy = go.GetComponent<Enemy>();
		spawnedEnemy.ItemsToDrop = GenerateLoot();
	}

	public Item[] GenerateLoot()
	{
		return null;
	}
}
