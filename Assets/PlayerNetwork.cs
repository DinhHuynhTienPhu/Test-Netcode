using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using EasyJoystick;
using System;
using UnityEngine.UI;

public class PlayerNetwork : NetworkBehaviour
{

  public NetworkVariable<float> Health = new NetworkVariable<float>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
  public Joystick joystick;
  public Transform bullet;

  public Slider healthSlider;
  public Canvas canvasWorld;
  private void Awake()
  {
    if (IsLocalPlayer)
    {
      Debug.Log("Local player");
    }
  }
  // Update is called once per frame
  void Update()
  {
    canvasWorld.transform.LookAt(Camera.main.transform);
    healthSlider.maxValue = 100;
    healthSlider.value = Health.Value;


  }
  public void Move(Vector3 move)
  {
    if (!IsOwner)
    {
      return;
    }

    transform.position += move * Time.deltaTime * 5;
    //rotate the player base on move direction
    if (move != Vector3.zero)
    {
      transform.rotation = Quaternion.LookRotation(move);
    }
  }

  internal void Shoot()
  {

    var spawned = Instantiate(bullet, transform.position, transform.rotation);
    spawned.gameObject.name = this.OwnerClientId.ToString();
    StartCoroutine(TranslateBullet(spawned));
    Destroy(spawned.gameObject, 5);
    ShootServerRpc(this.OwnerClientId);

  }
  IEnumerator TranslateBullet(Transform spawned)
  {
    while (true)
    {
      if (spawned == null) { break; }
      spawned.position += spawned.forward * Time.deltaTime * 10;
      yield return new WaitForEndOfFrame();
    }
  }

  [ServerRpc]
  public void ShootServerRpc(ulong ignore)
  {
    Debug.Log("ShootServerRpc" + ignore);
    ShootClientRpc(ignore);
  }
  [ClientRpc]
  public void ShootClientRpc(ulong ignore)
  {
    Debug.Log("ShootClientRpc" + ignore);
    if (IsOwner) return;

    var spawned = Instantiate(bullet, transform.position, transform.rotation);
    spawned.gameObject.name = this.OwnerClientId.ToString();

    StartCoroutine(TranslateBullet(spawned));
    Destroy(spawned.gameObject, 5);
  }


  private void OnTriggerEnter(Collider other)
  {
    Debug.Log("OnTriggerEnter" + other.name);
    if (other.gameObject.tag == "bullet")
    {
      if (other.gameObject.name == this.OwnerClientId.ToString())
      {
        return;
      }
      if (IsServer == false)
      {
        return;
      }
      Health.Value -= 10;
      if (Health.Value <= 0)
      {
        Destroy(gameObject);
      }
    }
  }
}
