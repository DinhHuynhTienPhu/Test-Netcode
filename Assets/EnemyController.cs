using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "bullet")
    {
      if (IsHost) GetComponent<NetworkObject>().Despawn();
      //Destroy(gameObject);
    }
  }
}
