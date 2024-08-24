using UnityEngine;
using TMPro;
using SFB;
using T2G.UnityAdapter;

public class GameDescForm : MonoBehaviour
{
    [SerializeField] TMP_InputField _GameDescName;

    [SerializeField] TMP_InputField _GameTitle;
    [SerializeField] TMP_Dropdown _Genre;
    [SerializeField] TMP_Dropdown _ArtStyle;
    [SerializeField] TMP_InputField _Version;
    [SerializeField] TMP_InputField _MinorVersion;
    [SerializeField] TMP_InputField _Developer;

    [SerializeField] TMP_Dropdown _GameEngine;
    [SerializeField] TMP_InputField _Path;
    [SerializeField] TMP_InputField _ProjectName;

    [SerializeField] TMP_InputField _GameWorldName;
    [SerializeField] TMP_Dropdown _Ground;
    [SerializeField] TMP_Dropdown _Sunlight;
    [SerializeField] TMP_Dropdown _PlayerCharacter;
    [SerializeField] TMP_Dropdown _Camera;
    [SerializeField] TMP_Dropdown _GameGoal;
    [SerializeField] TMP_Dropdown _HUD;

    [SerializeField] GameDescList _GameDescList;

    private void OnEnable()
    {
        InitForm();
    }

    void InitForm(GameDesc gameDesc = null)
    {
        if(gameDesc == null)
        {
            _GameDescName.text = string.Empty;
            _GameTitle.text = string.Empty;
            _Genre.value = 0; 
            _ArtStyle.value = 0;
            _Version.text = "0";
            _MinorVersion.text = "1";
            _Developer.text = Settings.User;
            _GameEngine.value = 0;
            _Path.text = PlayerPrefs.GetString(Defs.k_ProjectPathname, string.Empty);
            _GameWorldName.text = "World1";
            _Ground.value = 0;
            _Sunlight.value = 0;
            _PlayerCharacter.value = 0;
            _Camera.value = 0;
            _GameGoal.value = 0;
            _HUD.value = 0;
        }
        else
        {
            _GameDescName.text = gameDesc.Name;
            _GameTitle.text = gameDesc.Title;
            _Genre.value = _Genre.options.FindIndex(option => option.text.CompareTo(gameDesc.Genre) == 0);
            _ArtStyle.value = _ArtStyle.options.FindIndex(option => option.text.CompareTo(gameDesc.ArtStyle) == 0);
            _Version.text = gameDesc.VersionNumber.ToString();
            _MinorVersion.text = gameDesc.MinorVersionNumber.ToString();
            _Developer.text = gameDesc.Developer;
            _GameEngine.value = _GameEngine.options.FindIndex(option => option.text.CompareTo(gameDesc.Project.Engine) == 0);
            _Path.text = gameDesc.Project.Path;
            _GameWorldName.text = gameDesc.GameWorlds[0].Name;
            _Ground.value = _Ground.options.FindIndex(option => option.text.CompareTo(gameDesc.GameWorlds[0].Ground) == 0);
            _Sunlight.value = _Sunlight.options.FindIndex(option => option.text.CompareTo(gameDesc.GameWorlds[0].SunLight) == 0);
            _PlayerCharacter.value = _PlayerCharacter.options.FindIndex(option => option.text.CompareTo(gameDesc.GameWorlds[0].PlayerCharacter) == 0);
            _Camera.value = _Camera.options.FindIndex(option => option.text.CompareTo(gameDesc.GameWorlds[0].Camera) == 0);
            _GameGoal.value = _GameGoal.options.FindIndex(option => option.text.CompareTo(gameDesc.GameWorlds[0].GameGoal) == 0);
            _HUD.value = _HUD.options.FindIndex(option => option.text.CompareTo(gameDesc.GameWorlds[0].HUD) == 0);
        }
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

    public void OnSave()
    {
        var gameDesc = new GameDesc();

        gameDesc.Name = _GameDescName.text;
        gameDesc.Title = _GameTitle.text;
        gameDesc.Genre = _Genre.options[_Genre.value].text;
        gameDesc.ArtStyle = _ArtStyle.options[_ArtStyle.value].text;
        gameDesc.VersionNumber = int.Parse(_Version.text);
        gameDesc.MinorVersionNumber = int.Parse(_MinorVersion.text);
        gameDesc.Developer = _Developer.text;
        gameDesc.Project.Engine = _GameEngine.options[_GameEngine.value].text;
        gameDesc.Project.Path = _Path.text;
        gameDesc.GameWorlds[0].Name = _GameWorldName.text;
        gameDesc.GameWorlds[0].Ground = _Ground.options[_Ground.value].text;
        gameDesc.GameWorlds[0].SunLight = _Sunlight.options[_Sunlight.value].text;
        gameDesc.GameWorlds[0].PlayerCharacter = _PlayerCharacter.options[_PlayerCharacter.value].text;
        gameDesc.GameWorlds[0].Camera = _Camera.options[_Camera.value].text;
        gameDesc.GameWorlds[0].GameGoal = _GameGoal.options[_GameGoal.value].text;
        gameDesc.GameWorlds[0].HUD = _HUD.options[_HUD.value].text;

        JsonParser.SerializeAndSave(gameDesc);
    }

    public void OnCancel()
    {
        gameObject.SetActive(false);
    }

}
