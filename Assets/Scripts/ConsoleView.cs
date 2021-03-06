using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;

public class ConsoleView : MonoBehaviour
{
    public GameObject player;
    public GameObject controller;
    ConsoleController console;

    bool didShow = false;

    public GameObject viewContainer; //Container for console view, should be a child of this GameObject
    public Text logTextArea;
    public InputField inputField;

    void Start()
    {
        console = new ConsoleController(player, controller);
        if (console != null)
        {
            console.visibilityChanged += onVisibilityChanged;
            console.logChanged += onLogChanged;
        }
        updateLogStr(console.log);
        inputField.onEndEdit.AddListener(delegate { runCommand(); });
    }

    ~ConsoleView()
    {
        console.visibilityChanged -= onVisibilityChanged;
        console.logChanged -= onLogChanged;
    }

    void Update()
    {
        //Toggle visibility when tilde key pressed
        if (Input.GetKeyUp("`"))
        {
            toggleVisibility();
        }

        //Toggle visibility when 5 fingers touch.
        if (Input.touches.Length == 5)
        {
            if (!didShow)
            {
                toggleVisibility();
                didShow = true;
            }
        }
        else
        {
            didShow = false;
        }
    }

    void toggleVisibility()
    {
        setVisibility(!viewContainer.activeSelf);
    }

    void setVisibility(bool visible)
    {
        if (visible)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
        viewContainer.SetActive(visible);
    }

    void onVisibilityChanged(bool visible)
    {
        setVisibility(visible);
    }

    void onLogChanged(string[] newLog)
    {
        updateLogStr(newLog);
    }

    void updateLogStr(string[] newLog)
    {
        if (newLog == null)
        {
            logTextArea.text = "";
        }
        else
        {
            logTextArea.text = string.Join(" ", newLog);
        }
    }

    /// <summary>
    /// Event that should be called by anything wanting to submit the current input to the console.
    /// </summary>
    public void runCommand()
    {
        console.runCommandString(inputField.text);
        inputField.text = "";
    }

}
