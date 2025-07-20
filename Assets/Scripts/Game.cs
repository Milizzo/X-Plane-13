using System;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logs;

    private void Awake()
    {
        try
        {
            var network = NetworkManager.Singleton;
            switch (Buttons.Mode)
            {
                case Buttons.CurrentMode.Join:
                    network.GetComponent<UnityTransport>().SetConnectionData(Buttons.JoinIP, 7777);
                    if (!network.StartClient()) throw new("Failed to start client");
                    break;
                case Buttons.CurrentMode.Host:
                    if (!network.StartHost()) throw new("Failed to start host");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (logs != null)
            {
                Application.logMessageReceived += (condition, _, type) => logs.text = $"{type}: {condition}";
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);

            SceneManager.LoadScene(0);

            if (NetworkManager.Singleton != null) Destroy(NetworkManager.Singleton.gameObject);
        }
    }
}