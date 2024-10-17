using UnityEngine;
using TMPro;
using SFB;
using T2G.UnityAdapter;
using System.IO;
using UnityEngine.UI;

public class GameDescForm : MonoBehaviour
{
    static readonly string k_DefaultGameDescNameKey = "DefaultGameDescName";

    [SerializeField] TMP_InputField _GameDescName;

    [SerializeField] TMP_InputField _GameTitle;
    [SerializeField] TMP_Dropdown _Genre;
    [SerializeField] TMP_Dropdown _ArtStyle;
    [SerializeField] TMP_InputField _Developer;

    [SerializeField] TMP_Dropdown _GameEngine;
    [SerializeField] TMP_InputField _Path;
    [SerializeField] TMP_InputField _ProjectName;

    [SerializeField] TMP_InputField _GameStory;

    [SerializeField] GameObject _ProfileView;
    [SerializeField] GameObject _JsonView;
    [SerializeField] Button _ViewProfileButton;
    [SerializeField] Button _ViewJsonButton;
    [SerializeField] TMP_InputField _InputJson;

    [SerializeField] TMP_Dropdown _SelectSampleGameDesc;

    [SerializeField] GameDescList _GameDescList;

    GameDesc _gameDesc;

    private void OnEnable()
    {
        _SelectSampleGameDesc.ClearOptions();
        for (int i = 0; i < GameDesc.SampleGameDescNames.Length; ++i)
        {
            _SelectSampleGameDesc.options.Add(new TMP_Dropdown.OptionData(GameDesc.SampleGameDescNames[i]));
        }

        var gameDescName = PlayerPrefs.GetString(k_DefaultGameDescNameKey, string.Empty);
        var gameDesc = JsonParser.LoadGameDesc(gameDescName);
        InitForm(gameDesc);
        SetViewPanel(true);
    }

    public void SetViewPanel(bool viewProfile)
    {
        _ProfileView.SetActive(viewProfile);
        _JsonView.SetActive(!viewProfile);
        _ViewProfileButton.interactable = !viewProfile;
        _ViewProfileButton.gameObject.SetActive(!viewProfile);
        _ViewJsonButton.interactable = viewProfile;
        _ViewJsonButton.gameObject.SetActive(_ViewJsonButton);
    }

    void InitForm(GameDesc gameDesc = null)
    {
        if (gameDesc == null)
        {
            _gameDesc = new GameDesc();
            _gameDesc.Name = "New Game";
            _gameDesc.Developer = Settings.User;
            _gameDesc.Project.Path = PlayerPrefs.GetString(Defs.k_ProjectPathname, string.Empty);
            _gameDesc.Project.Name = Path.GetFileName(_gameDesc.Project.Path);
            OnSave();
        }
        else
        {
            _gameDesc = gameDesc;
        }

        _GameDescName.text = _gameDesc.Name;
        _GameTitle.text = _gameDesc.Title;
        _Genre.value = _Genre.options.FindIndex(option => option.text.CompareTo(_gameDesc.Genre) == 0);
        _ArtStyle.value = _ArtStyle.options.FindIndex(option => option.text.CompareTo(_gameDesc.ArtStyle) == 0);
        _Developer.text = _gameDesc.Developer;
        _GameEngine.value = _GameEngine.options.FindIndex(option => option.text.CompareTo(_gameDesc.Project.Engine) == 0);
        _Path.text = _gameDesc.Project.Path;
        _ProjectName.text = _gameDesc.Project.Name;
        _GameStory.text = _gameDesc.GameStory;

    }

    public void OnSelectPath()
    {
        string[] paths = StandaloneFileBrowser.OpenFolderPanel("Choose project path", string.Empty, false);
        if(paths.Length > 0)
        {
            _Path.text = paths[0];
        }
    }

    public void OnLoadGameDesc()
    {
        _GameDescList.LoadGameDescCallback = (gameDescName) =>
        {
            var gameDesc = JsonParser.LoadGameDesc(gameDescName);
            InitForm(gameDesc);
        };
        _GameDescList.gameObject.SetActive(true);
    }

    public void OnLoadSample()
    {
        GameDesc gameDesc = new GameDesc();
        if(GameDesc.GetSampleGameDesc(_SelectSampleGameDesc.value, ref gameDesc))
        {
            _gameDesc = gameDesc;
            InitForm(gameDesc);
        }
    }

    public GameDesc GetGameDesc()
    {
        _gameDesc.Name = _GameDescName.text;
        _gameDesc.Title = _GameTitle.text;
        _gameDesc.Genre = _Genre.options[_Genre.value].text;
        _gameDesc.ArtStyle = _ArtStyle.options[_ArtStyle.value].text;
        _gameDesc.Developer = _Developer.text;
        _gameDesc.Project.Engine = _GameEngine.options[_GameEngine.value].text;
        _gameDesc.Project.Path = _Path.text;
        _gameDesc.Project.Name = _ProjectName.text;
        _gameDesc.GameStory = _GameStory.text;
        return _gameDesc;
    }

    public void OnSave()
    {
        var gameDesc = GetGameDesc();
        JsonParser.SerializeAndSave(gameDesc);
        PlayerPrefs.SetString(k_DefaultGameDescNameKey, gameDesc.Name);
    }

    public void OnCancel()
    {
        gameObject.SetActive(false);
    }

}
