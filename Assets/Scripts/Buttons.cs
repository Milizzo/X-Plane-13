using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [InspectorName("IP Field")] [SerializeField]
    private TMP_InputField ipField;

    [CanBeNull] public static string JoinIP { get; private set; }
    public static CurrentMode Mode { get; private set; } = CurrentMode.Host;

    public enum CurrentMode
    {
        Join,
        Host,
    }

    public void Play()
    {
        Mode = CurrentMode.Join;
        JoinIP = ipField.text;

        SceneManager.LoadScene(1);
    }

    public void Host()
    {
        Mode = CurrentMode.Host;
        JoinIP = null;

        SceneManager.LoadScene(1);
    }
}