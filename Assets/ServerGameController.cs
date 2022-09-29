using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using EasyJoystick;
using System;
using UnityEngine.UI;

public class ServerGameController : NetworkBehaviour
{
  public static ServerGameController Instance;

  public Transform enemyPrefab;
  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Destroy(this);
    }
  }
  // Start is called before the first frame update
  void Start()
  {
    Debug.Log("ServerGameController,isServer:" + IsServer);

    StartCoroutine(SpawnEnemy());

  }

  // Update is called once per frame
  void Update()
  {

  }
  IEnumerator SpawnEnemy()
  {
    while (true)
    {
      if (IsServer)
      {

        yield return new WaitForSeconds(3);
        var spawned = Instantiate(enemyPrefab, new Vector3(UnityEngine.Random.Range(-10, 10), 1, UnityEngine.Random.Range(-10, 10)), Quaternion.identity);
        spawned.gameObject.GetComponent<NetworkObject>().Spawn();
      }
      else
      {
        yield return new WaitForSeconds(3);
      }
    }
  }
  [ContextMenu("SpawnEnemy")]
  public void Spawn()
  {
    if (IsServer)
    {

      var spawned = Instantiate(enemyPrefab, new Vector3(UnityEngine.Random.Range(-10, 10), 1, UnityEngine.Random.Range(-10, 10)), Quaternion.identity);
      spawned.gameObject.GetComponent<NetworkObject>().Spawn();
    }
  }
}
