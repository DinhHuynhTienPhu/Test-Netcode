using System.Collections;
using System.Collections.Generic;
using EasyJoystick;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovermentInputController : MonoBehaviour
{
  public PlayerNetwork playerNetwork;
  public Joystick joystick;
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (playerNetwork == null)
    {
      var players = FindObjectsOfType<PlayerNetwork>();
      foreach (var player in players)
      {
        if (player.IsOwner)
        {
          playerNetwork = player;
          break;
        }
      }
    }
    if (playerNetwork == null) { return; }
    Vector3 move = new Vector3(joystick.Horizontal(), 0, joystick.Vertical());
    playerNetwork.Move(move);
  }
  public void OnClickShoot()
  {
    playerNetwork?.Shoot();
  }
}
