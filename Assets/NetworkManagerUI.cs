using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour
{

  public Button serverbtn, hostbtn, clientbtn;
  public InputField joinCodeInput;
  private void Awake()
  {
    serverbtn.onClick.AddListener(() =>
    {
      NetworkManager.Singleton.StartServer();
    });

    hostbtn.onClick.AddListener(async () =>
    {
      if (RelayManager.Instance.IsRelayEnabled)
      {
        await RelayManager.Instance.SetupRelay();
      }

      if (NetworkManager.Singleton.StartHost())
      {
        Debug.Log("Host started");
      }

    });

    clientbtn.onClick.AddListener(async () =>
    {
      if (RelayManager.Instance.IsRelayEnabled)
      {
        await RelayManager.Instance.JoinRelay(joinCodeInput.text);
      }

      if (NetworkManager.Singleton.StartClient())
      {
        Debug.Log("Client started");
      }
    });
  }
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
