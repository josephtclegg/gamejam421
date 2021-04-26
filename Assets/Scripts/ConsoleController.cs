/// <summary>
/// Handles parsing and execution of console commands, as well as collecting log output.
/// Copyright (c) 2014-2015 Eliot Lash
/// </summary>
using UnityEngine;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public delegate void CommandHandler(string[] args);

public class ConsoleController
{
    private GameObject controller;
    private GameObject player;

    #region Event declarations
    // Used to communicate with ConsoleView
    public delegate void LogChangedHandler(string[] log);
    public event LogChangedHandler logChanged;

    public delegate void VisibilityChangedHandler(bool visible);
    public event VisibilityChangedHandler visibilityChanged;
    #endregion

    /// <summary>
    /// Object to hold information about each command
    /// </summary>
    class CommandRegistration
    {
        public string command { get; private set; }
        public CommandHandler handler { get; private set; }
        public string help { get; private set; }

        public CommandRegistration(string command, CommandHandler handler, string help)
        {
            this.command = command;
            this.handler = handler;
            this.help = help;
        }
    }

    /// <summary>
    /// How many log lines should be retained?
    /// Note that strings submitted to appendLogLine with embedded newlines will be counted as a single line.
    /// </summary>
    const int scrollbackSize = 20;

    Queue<string> scrollback = new Queue<string>(scrollbackSize);
    List<string> commandHistory = new List<string>();
    Dictionary<string, CommandRegistration> commands = new Dictionary<string, CommandRegistration>();

    public string[] log { get; private set; } //Copy of scrollback as an array for easier use by ConsoleView

    const string repeatCmdName = "!!"; //Name of the repeat command, constant since it needs to skip these if they are in the command history

    public ConsoleController(GameObject p, GameObject c)
    {
        //When adding commands, you must add a call below to registerCommand() with its name, implementation method, and help text.
        registerCommand("echo", echo, "\nechoes arguments back as array (for testing argument parser)\n");
        registerCommand("help", help, "Print this help.\n");
        registerCommand("hide", hide, "Hide the console.\n");
        registerCommand(repeatCmdName, repeatCommand, "Repeat last command.\n");
        registerCommand("cd", cd, "Change directory.\n");
        registerCommand("chmod", chmod, "Change the mode of each file.\n");
        registerCommand("ssh", ssh, "Log into a remote machine.\n");

        player = p;
        controller = c;
    }

    void registerCommand(string command, CommandHandler handler, string help)
    {
        commands.Add(command, new CommandRegistration(command, handler, help));
    }

    public void appendLogLine(string line)
    {
        Debug.Log(line);

        if (scrollback.Count >= ConsoleController.scrollbackSize)
        {
            scrollback.Dequeue();
        }
        scrollback.Enqueue(line);

        log = scrollback.ToArray();
        if (logChanged != null)
        {
            logChanged(log);
        }
    }

    public void runCommandString(string commandString)
    {
        appendLogLine("\n$ " + commandString + "\n");

        string[] commandSplit = parseArguments(commandString);
        string[] args = new string[0];
        if (commandSplit.Length < 1)
        {
            appendLogLine(string.Format("1: Unable to process command '{0}'", commandString));
            return;

        }
        else if (commandSplit.Length >= 2)
        {
            int numArgs = commandSplit.Length - 1;
            args = new string[numArgs];
            Array.Copy(commandSplit, 1, args, 0, numArgs);
        }
        runCommand(commandSplit[0].ToLower(), args);
        commandHistory.Add(commandString);
    }

    public void runCommand(string command, string[] args)
    {
        CommandRegistration reg = null;
        if (!commands.TryGetValue(command, out reg))
        {
            appendLogLine(string.Format("Unknown command '{0}', type 'help' for list.", command));
        }
        else
        {
            if (reg.handler == null)
            {
                appendLogLine(string.Format("2: Unable to process command '{0}', handler was null.", command));
            }
            else
            {
                reg.handler(args);
            }
        }
    }

    static string[] parseArguments(string commandString)
    {
        LinkedList<char> parmChars = new LinkedList<char>(commandString.ToCharArray());
        bool inQuote = false;
        var node = parmChars.First;
        while (node != null)
        {
            var next = node.Next;
            if (node.Value == '"')
            {
                inQuote = !inQuote;
                parmChars.Remove(node);
            }
            if (!inQuote && node.Value == ' ')
            {
                node.Value = ' ';
            }
            node = next;
        }
        char[] parmCharsArr = new char[parmChars.Count];
        parmChars.CopyTo(parmCharsArr, 0);
        return (new string(parmCharsArr)).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    }

    public void updateGameState()
    {
        controller.GetComponent<GameController>().updateGameState();
    }

    #region Command handlers
    //Implement new commands in this region of the file.

    void echo(string[] args)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string arg in args)
        {
            sb.AppendFormat("{0},", arg);
        }
        sb.Remove(sb.Length - 1, 1);
        appendLogLine(sb.ToString());
    }

    void help(string[] args)
    {
        updateGameState();
        foreach (CommandRegistration reg in commands.Values)
        {
            appendLogLine(string.Format("{0}: {1}", reg.command, reg.help));
        }
    }

    void hide(string[] args)
    {
        if (visibilityChanged != null)
        {
            visibilityChanged(false);
        }
    }

    void repeatCommand(string[] args)
    {
        for (int cmdIdx = commandHistory.Count - 1; cmdIdx >= 0; --cmdIdx)
        {
            string cmd = commandHistory[cmdIdx];
            if (String.Equals(repeatCmdName, cmd))
            {
                continue;
            }
            runCommandString(cmd);
            break;
        }
    }

    void cd(string[] args)
    {
        Regex regex = new Regex(@"^\/home\/eggert\/(\d+)");
        Match match = regex.Match(args[0]);
        if (!match.Success) {
            appendLogLine(string.Format("cd: no such file or directory: {0}", args[0]));
            return;
        }

        String doorNum = match.Groups[1].Value;
        GameObject doors = GameObject.FindGameObjectsWithTag("Doors")[0];
        foreach(Transform d in doors.transform) {
            Door door = d.GetChild(0).gameObject.GetComponent<Door>();
            Debug.Log(string.Format("Found door number {0}; looking for {1}", door == null ? "null" : door.GetDoorNumber(), doorNum));
            if (door != null && door.GetDoorNumber() == doorNum) {
                Debug.Log(string.Format("teleporting player from {0} to {1}", player.transform.position, d.position));
                player.GetComponent<PlayerController>().GetCharacterController().Move(d.position + d.forward - player.transform.position);
                visibilityChanged(false);
                return;
            }
        }
    }

    void chmod(string[] args)
    {
        string mode = args[0];
        string file = args[1];

        if (mode != "777" || file != "bathroom") {
            appendLogLine(string.Format("Permissions for file {0} are not modifiable.", file));
            return;
        }

        PlayerController pCont = player.GetComponent<PlayerController>();
        if (!pCont.IsInFrontOfBathroom() || controller.GetComponent<GameController>().GetCurrentState() != State.SomeGoalsCompleted) {
            appendLogLine(string.Format("Permission denied."));
            Debug.Log(string.Format("Player is {0}in front of bathroom and current state is {1}",
                        !pCont.IsInFrontOfBathroom() ? "not " : "", controller.GetComponent<GameController>().GetCurrentState()));
            return;
        }

        appendLogLine("Bathroom unlocked.");
        DisappearingDoor bathroom = GameObject.FindGameObjectsWithTag("Bathroom")[0].GetComponent<DisappearingDoor>();
        if (bathroom == null)
            Debug.Log("Bathroom null");
        bathroom.SetLocked(false);
        updateGameState();
    }

    void ssh(string[] args)
    {
        if (args.Length < 3) {
            appendLogLine("SSH target required.");
            return;
        }
        if (args[0] != "-i") {
            appendLogLine(string.Format("ssh requires an identity file using -i"));
            return;
        }

        string keyfile = args[1];
        string target = args[2];

        GameController gc = controller.GetComponent<GameController>();
        if (gc.GetCurrentState() != State.ManyGoalsCompleted
                || keyfile != gc.EscapeKeyFile()
                || target != gc.EscapeTarget()) {
            appendLogLine(string.Format("Permission denied."));
            Debug.Log(string.Format("Current state is {0}, keyfile is {1}, target is {2}",
                        controller.GetComponent<GameController>().GetCurrentState(),
                        keyfile == gc.EscapeKeyFile() ? "correct" : "wrong",
                        target == gc.EscapeTarget() ? "correct" : "wrong"));
            return;
        }

        appendLogLine("You win");
        updateGameState();
    }

    #endregion
}
