using System;
using System.Collections.Generic;
using UnityEngine;

public partial class Executor
{
    static Executor _instance = null;
    public static Executor Instance
    {
        get 
        {
            if (_instance != null)
            {
                _instance = new Executor();
            }
            return _instance; 
        }
    }

    public class ScriptCommand
    {
        public string Command { get; set; }
        public List<string> Arguments { get; set; }

        public ScriptCommand(string command, List<string> arguments)
        {
            Command = command;
            Arguments = arguments;
        }
    }

    public class ScriptParser
    {
        public List<ScriptCommand> Parse(string[] scriptLines)
        {
            List<ScriptCommand> commands = new List<ScriptCommand>();

            foreach (var line in scriptLines)
            {
                var parts = line.Split(' ');
                string command = parts[0];
                var arguments = new List<string>(parts[1..]);

                commands.Add(new ScriptCommand(command, arguments));
            }

            return commands;
        }
    }

    Dictionary<string, Action<ScriptCommand>> _commandHandlers;

    public Executor()
    {
        // Initialize the dictionary with supported commands.
        _commandHandlers = new Dictionary<string, Action<ScriptCommand>>
        {
            { "ADD_OBJECT", HandleAddObject },
            { "SET_PROPERTY", HandleSetProperty },
            { "SET_SKY_TIME", HandleSetSkyTime }
        };
    }

    public void Execute(List<ScriptCommand> commands)
    {
        foreach(var command in commands)
        {
            Execute(command);
        }
    }

    public void Execute(ScriptCommand command)
    {
        if (_commandHandlers.ContainsKey(command.Command))
        {
            _commandHandlers[command.Command](command);
        }
        else
        {
            Debug.LogWarning($"[Executor.Execute] Unrecognized command: {command.Command}");
        }
    }

}
