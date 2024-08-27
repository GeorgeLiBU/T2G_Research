using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using T2G.UnityAdapter;

public class CmdConnect : Command
{
    public static readonly string CommandKey = "Connect";

    public override bool Execute(params string[] args)
    {
        if (args.Length > 0)
        {
            float timeoutScale = 1.0f;
            if(float.TryParse(args[0], out timeoutScale))
            {
                CommunicatorClient.Instance.StartClient(timeoutScale);
                return true;
            }
        }
        CommunicatorClient.Instance.StartClient();
        return true;
    }

    public override string GetKey()
    {
        return CommandKey.ToLower();
    }

    public override string[] GetArguments()
    {
        string[] args = { };
        return args;
    }
}
