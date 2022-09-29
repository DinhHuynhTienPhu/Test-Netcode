using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Authentication;
using Unity.Services.Relay;

public struct RelayHostData
{
  public string JoinCode;
  public string IPv4Address;
  public ushort Port;
  public Guid AllocationID;
  public byte[] AllocationIDBytes;
  public byte[] ConnectionData;
  public byte[] Key;
}
public struct RelayJoinData
{
  public string JoinCode;
  public string IPv4Address;
  public ushort Port;
  public Guid AllocationID;
  public byte[] AllocationIDBytes;
  public byte[] ConnectionData;
  public byte[] HostConnectionData;
  public byte[] Key;
}
public class RelayManager : MonoBehaviour
{
  public static RelayManager Instance;

  [SerializeField]
  private string evironment = "production";

  [SerializeField]
  private int maxConnections = 5;

  public bool IsRelayEnabled => Transport != null && Transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;

  public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();



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

  public async Task<RelayHostData> SetupRelay()
  {

    Debug.Log("relay server starting with max connections = " + maxConnections);

    InitializationOptions options = new InitializationOptions()
    .SetEnvironmentName(evironment);

    await UnityServices.InitializeAsync(options);
    if (!AuthenticationService.Instance.IsSignedIn)
    {
      await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    Unity.Services.Relay.Models.Allocation allocation =
    await Relay.Instance.CreateAllocationAsync(maxConnections);

    RelayHostData relayHostData = new RelayHostData
    {
      Key = allocation.Key,
      IPv4Address = allocation.RelayServer.IpV4,
      Port = (ushort)allocation.RelayServer.Port,
      AllocationID = allocation.AllocationId,
      AllocationIDBytes = allocation.AllocationIdBytes,
      ConnectionData = allocation.ConnectionData

    };
    relayHostData.JoinCode = await Relay.Instance.GetJoinCodeAsync(relayHostData.AllocationID);

    Transport.SetRelayServerData(relayHostData.IPv4Address, relayHostData.Port,
    relayHostData.AllocationIDBytes, relayHostData.Key, relayHostData.ConnectionData);

    Debug.Log("relay server started with join code = " + relayHostData.JoinCode);
    return relayHostData;
  }

  public async Task<RelayJoinData> JoinRelay(string joinCode)
  {
    InitializationOptions options = new InitializationOptions()
    .SetEnvironmentName(evironment);

    await UnityServices.InitializeAsync(options);
    if (!AuthenticationService.Instance.IsSignedIn)
    {
      await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    Unity.Services.Relay.Models.JoinAllocation allocation =
     await Relay.Instance.JoinAllocationAsync(joinCode);

    RelayJoinData relayJoinData = new RelayJoinData
    {
      Key = allocation.Key,
      IPv4Address = allocation.RelayServer.IpV4,
      Port = (ushort)allocation.RelayServer.Port,
      AllocationID = allocation.AllocationId,
      AllocationIDBytes = allocation.AllocationIdBytes,
      ConnectionData = allocation.ConnectionData,
      HostConnectionData = allocation.HostConnectionData,
      JoinCode = joinCode
    };

    Transport.SetRelayServerData(relayJoinData.IPv4Address, relayJoinData.Port,
    relayJoinData.AllocationIDBytes, relayJoinData.Key, relayJoinData.ConnectionData, relayJoinData.HostConnectionData);


    Debug.Log("client joined relay server with join code = " + relayJoinData.JoinCode);
    return relayJoinData;
  }
}
