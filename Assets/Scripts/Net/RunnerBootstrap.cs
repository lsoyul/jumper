using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class RunnerBootstrap : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NetworkRunner runnerPrefab;    // Runner 프리팹 참조
    [SerializeField] private NetworkObject playerPrefab;    // Player 프리팹 참조

    public string SessionName = "world-001";
    public int MaxPlayers = 100;

    [Header("Scene")]
    public SceneRef mainScene;                              // 빌드 세팅 Scene 인덱스 기반

    NetworkRunner runner;

    async void Start()
    {
        // 단일 인스턴스 보장
        if (FindObjectOfType<NetworkRunner>())              
            return;

        // 커맨드라인 파싱
        var args = Environment.GetCommandLineArgs();
        // 기본값 : 에디터/스탠드얼론은 클라, 서버빌드/배치모드면 서버
        var isServer = Application.isBatchMode || Application.platform == RuntimePlatform.LinuxServer;

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-mode" && i + 1 < args.Length)
            {
                var val = args[i + 1].ToLowerInvariant();
                if (val == "server")
                    isServer = true;
                else if (val == "client")
                    isServer = false;
            }

            if (args[i] == "-session" && i + 1 < args.Length)
            {
                SessionName = args[i + 1];
            }
        }
    }

}
