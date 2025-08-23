using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

public class RunnerBootstrap : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkObject playerPrefab;

    NetworkRunner _runner;

    async void Start()
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = Application.platform != RuntimePlatform.LinuxServer;  // 서버는 입력 제공하지 않음

        var sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
        var gameStartArgs = new StartGameArgs
        {
            GameMode = Application.platform == RuntimePlatform.LinuxServer ? GameMode.Server : GameMode.Client,
            SessionName = "room-1",
            Scene = SceneRef.FromIndex(1),      // 1번씬 == Game
            SceneManager = sceneManager
        };

        await _runner.StartGame(gameStartArgs);
    }

    // Update is called once per frame
    void Update()
    {

    }


    #region ## INetworkRunnerCallbacks

    bool LeftHeld() => Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    bool RightHeld() => Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer) return;

        var spawnPos = new Vector3(0, 2, 0);    // start position
        runner.Spawn(playerPrefab, spawnPos, Quaternion.identity, player);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!runner.IsClient) return;

        var data = new JumpInput();
        var b = default(NetworkButtons);

        if (LeftHeld()) b.Set(0, true);
        if (RightHeld()) b.Set(1, true);
        data.Buttons = b;
        input.Set(data);
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }


    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    #endregion

}
