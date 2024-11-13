using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace T2G.UnityAdapter
{
    public partial class Executor
    {
        static Executor _instance = null;
        public static Executor Instance
        {
            get
            {
                if (_instance == null)
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

        private List<ScriptCommand> Parse(string[] instructions)
        {
            List<ScriptCommand> commands = new List<ScriptCommand>();

            foreach (var instruction in instructions)
            {
                var commandTuple = ParseInstruction(instruction);

                commands.Add(new ScriptCommand(commandTuple.command, commandTuple.arguments));
            }

            return commands;
        }

        private (string command, List<string> arguments) ParseInstruction(string instruction)
        {
            if (string.IsNullOrWhiteSpace(instruction))
            {
                throw new ArgumentException("Input cannot be null or whitespace.");
            }
            // Regular expression to match the command and arguments
            var matches = Regex.Matches(instruction, @"[\""].+?[\""]|[^ ]+");
            if (matches.Count == 0)
            {
                throw new ArgumentException("No command found in input.");
            }

            var command = matches[0].Value.Trim('"');
            var arguments = new List<string>();

            for (int i = 1; i < matches.Count; ++i)
            {
                arguments.Add(matches[i].Value.Trim('"'));
            }

            return (command, arguments);
        }

        Dictionary<string, Action<ScriptCommand>> _commandHandlers;

        public Executor()
        {
            // Initialize the dictionary with supported commands.
            _commandHandlers = new Dictionary<string, Action<ScriptCommand>>
            {
                { "CREATE_WORLD", HandleCreateWorld },
                { "CREATE_OBJECT", HandleCreateObject },
                { "ADDON", HandleAddOn }
            };
        }

        private void Execute(List<ScriptCommand> commands)
        {
            foreach (var command in commands)
            {
                Execute(command);
            }
        }

        private void Execute(ScriptCommand command)
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

        public bool Execute(string message)
        {
            if (message.Substring(0, 4).CompareTo("INS>") == 0)
            {
                var commandTuple = ParseInstruction(message.Substring(4));
                var command = new ScriptCommand(commandTuple.command, commandTuple.arguments);
                Execute(command);
                return true;
            }
            return false;
        }

        public static void RespondCompletion(bool succeeded)
        {
            if (succeeded)
            {
                CommunicatorServer.Instance.SendMessage("Done!");
            }
            else
            {
                CommunicatorServer.Instance.SendMessage("Failed!");
            }
        }
    }
}
