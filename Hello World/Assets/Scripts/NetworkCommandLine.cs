using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkCommandLine : MonoBehaviour {
    private NetworkManager netManager;

    private void Start () {
        netManager = GetComponentInParent<NetworkManager>();
        if (Application.isEditor) return;
        Dictionary<string, string> args = GetCommandlineArgs();
        if (args.TryGetValue("-mlapi", out string mlapiValue)) {
            switch (mlapiValue) {
                case "server":
                    netManager.StartServer();
                    break;
                case "host":
                    netManager.StartHost();
                    break;
                case "client":
                    netManager.StartClient();
                    break;
            }
        }
    }

    private Dictionary<string, string> GetCommandlineArgs () {
        Dictionary<string, string> argDictionary = new();
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; ++i) {
            string arg = args[i].ToLower();
            if (arg.StartsWith("-")) {
                string value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;
                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }
}