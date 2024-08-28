using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SimAssistant : MonoBehaviour
{
    [SerializeField] GameObject _AssistantDialogs;
    [SerializeField] GameDescForm _GameDescForm;

    GameDesc _gameDesc = new GameDesc();

    string[] _prompts = { 
        "hello",
        "hi",
        "create a new game",
        "make a new game",
        "generate game"
    };

    string[] _responses = {
        "Hi {user}, I am {assistant}, your game development assistant. What can I do for you?",
        "Hello {user}, I am {assistant} who will assist you to develop games. What can I do for you?",
        "Okay! I need some initial information about the game, please fill up the Game Description form.",
        "Generating the game ..."
    };

  
    Dictionary<string, List<int>> _promptResponseMap = new Dictionary<string, List<int>>();
    Dictionary<string, Func<string, int>> _responseActionMap = new Dictionary<string, Func<string, int>>();

    List<string> _matchedPrompts = new List<string>();

    static SimAssistant _instance = null;
    public static SimAssistant Instance => _instance;

    private void Awake()
    {
        _instance = this;

        _promptResponseMap.Add(_prompts[0], new List<int>(new int[] { 0, 1 }));
        _promptResponseMap.Add(_prompts[1], new List<int>(new int[] { 0, 1 }));
        _promptResponseMap.Add(_prompts[2], new List<int>(new int[] { 2 }));
        _promptResponseMap.Add(_prompts[3], new List<int>(new int[] { 2 }));
        _promptResponseMap.Add(_prompts[4], new List<int>(new int[] { 3 }));

        _responseActionMap.Add(_responses[2], CollectGameProjectInformation);
        _responseActionMap.Add(_responses[3], GenerateGameFromGameDesc);
    }

    public void ProcessPrompt(string prompt, Action<string> callBack)
    {
        string promptKey = string.Empty;
        string responseMessage = "Sorry, I don't understand what you mean!";

        _matchedPrompts.Clear();
        if(Utilities.FindTopMatches(prompt, _prompts, 3, 0.5f, ref _matchedPrompts))
        {
            int count = _matchedPrompts.Count;
            if (count > 1)
            {
                promptKey = _matchedPrompts[UnityEngine.Random.Range(0, count)];
            }
            else if(count > 0)
            {
                promptKey = _matchedPrompts[0];
            }

            if (_promptResponseMap.TryGetValue(promptKey, out var responseOptions))
            {
                count = responseOptions.Count;
                if (count > 1)
                {
                    responseMessage = _responses[responseOptions[UnityEngine.Random.Range(0, count)]];
                }
                else if (count > 0)
                {
                    responseMessage = _responses[responseOptions[0]];
                }

                if (_responseActionMap.TryGetValue(responseMessage, out var function))
                {
                    int result = (function?.Invoke(responseMessage)).Value;
                    if (result > 0)
                    {
                        responseMessage = _responses[result];
                    }
                }
            }
        }
        callBack?.Invoke(responseMessage);
    }


    int CollectGameProjectInformation(string responseMessage)
    {
        _GameDescForm.gameObject.SetActive(true);
        return 0;
    }

    async Task CreateProjectFromGameDesc(GameDesc gameDesc)
    {
        bool completed = false;
        string[] args = new string[1] { gameDesc.GetProjectPathName() };
        CommandSystem.Instance.ExecuteCommand(
            (succeeded, sender, message) => {
                completed = true;
            }
            , "CreateProject"
            , args
            );

        while (!completed)
        {
            await Task.Delay(100);
        }
    }

    async Task InitProject(GameDesc gameDesc)
    {
        bool completed = false;
        string[] args = new string[1] { gameDesc.GetProjectPathName() };
        CommandSystem.Instance.ExecuteCommand(
            (succeeded, sender, message) =>
            {
                completed = true;
            }
            , "InitProject"
            , args
            );
        while (!completed)
        {
            await Task.Delay(100);
        }
    }


    async Task OpenProject(GameDesc gameDesc)
    {
        bool completed = false;
        string[] args = new string[1] { gameDesc.GetProjectPathName() };
        CommandSystem.Instance.ExecuteCommand(
            (succeeded, sender, message) =>
            {
                completed = true;
            }
            , "OpenProject"
            , args
            );
        while (!completed)
        {
            await Task.Delay(100);
        }
    }

    async Task Connect()
    {
        bool completed = false;
        string[] args = new string[1] { "200" };
        CommandSystem.Instance.ExecuteCommand((succeeded, sender, message) => { completed = true; }, "Connect", args);
        while (!completed)
        {
            await Task.Delay(100);
        }
    }

    async void GenerateGameAsync(GameDesc gameDesc)
    {
        ConsoleController console = ConsoleController.Instance;
        await CreateProjectFromGameDesc(gameDesc);
        console.WriteConsoleMessage(ConsoleController.eSender.Assistant, "Project was created. initilaizing the project ...");
        await InitProject(gameDesc);
        console.WriteConsoleMessage(ConsoleController.eSender.Assistant, "Project was initialized, opening the project in ...");
        await OpenProject(gameDesc);
        console.WriteConsoleMessage(ConsoleController.eSender.Assistant, "Project was opened. Connecting ...");
        await Connect();
        console.WriteConsoleMessage(ConsoleController.eSender.Assistant, "Project was opened. Importing assets ...");

        //...
        //Create scene
        //Add or confiure main camera
        //Add or configure sky
        //Generate ground
        //Add player 
        //Add player controller
        //Add camera controller
        //Add HUD
        //Add game goal

        //play (optional)
    }

    int GenerateGameFromGameDesc(string responseMessage)
    {
        if(!_GameDescForm.gameObject.activeSelf)
        {
            CollectGameProjectInformation(_responses[3]);
            return 2;
        }

        var gameDesc = _GameDescForm.GetGameDesc();
        GenerateGameAsync(gameDesc);

        return 0;
    }

    public void OnDestopPanelResized(float desktopHeight)
    {
        var rectTransform = _AssistantDialogs.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(0.0f, desktopHeight);
    }
}
