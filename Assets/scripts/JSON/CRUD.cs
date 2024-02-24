using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CRUD : MonoBehaviour
{
    private Image _avatar;

    private Image _imageDefault;

    private TextMeshProUGUI _labelPath;

    private TextMeshProUGUI _labelDatabase;

    [SerializeField] private TMP_InputField _inputID;

    [SerializeField] private TMP_InputField _inputName;

    [SerializeField] private TMP_InputField _inputClan;

    [SerializeField] private TMP_InputField _inputAge;

    [SerializeField]
    private string _path;
    private string img = $"https://api.dicebear.com/7.x/micah/png?seed=";
    private string radius = "&radius=50";
    private string background = "&backgroundColor=d1d4f9";
    private string nameFile = "Data.json";
    private int _idCharacter;

    [SerializeField] private GameObject _spawn;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _verticalSpacing = 10f;

    void Start()
    {
        _path = PathManager.GetPath(nameFile);

        _avatar = GameObject.Find("avatar").GetComponent<Image>();
        _imageDefault = GameObject.Find("imgDefault").GetComponent<Image>();

        _labelPath = GameObject.Find("labelPath").GetComponent<TextMeshProUGUI>();
        _labelDatabase = GameObject.Find("database").GetComponent<TextMeshProUGUI>();

        _inputID = GameObject.Find("inputID").GetComponent<TMP_InputField>();
        _inputName = GameObject.Find("inputName").GetComponent<TMP_InputField>();
        _inputClan = GameObject.Find("inputClan").GetComponent<TMP_InputField>();
        _inputAge = GameObject.Find("inputAge").GetComponent<TMP_InputField>();


        if (!File.Exists(_path))
        {
            var character = Character.characters;
            var obj = JsonConvert.SerializeObject(character);
            File.WriteAllText(_path, obj);
            Debug.Log("File created");
            _labelDatabase.text = "Database created";
        }
        else
        {
            Debug.Log("File exists");
            _labelDatabase.text = "Database: OK";
            ReadJSON();

        }

        GET_Register();
    }

    private void GET_Register()
    {
        var json = File.ReadAllText(_path);

        var data = JsonConvert.DeserializeObject<List<Character>>(json);

        Vector3 spawnPosition = _prefab.transform.position;

        foreach (var item in data)
        {
            GameObject newPanel = Instantiate(_prefab, spawnPosition, Quaternion.identity);
            newPanel.transform.SetParent(_spawn.transform, false);

            newPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.IdCharacter.ToString();
            GetAvatar.GetAvatarURL(item.Avatar, newPanel.transform.GetChild(1).GetComponent<Image>(), _imageDefault);
            newPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.Name;
            newPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = item.Clan;
            newPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = item.Age.ToString();

            newPanel.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => GET_Register(item.IdCharacter));
            newPanel.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(() => DELETE_Register(item.IdCharacter));

            spawnPosition += new Vector3(0f, -_verticalSpacing, 0f);
        }
    }

    public void GET_Register(int id)
    {
        _imageDefault.gameObject.SetActive(false);
        var json = File.ReadAllText(_path);

        var data = JsonConvert.DeserializeObject<List<Character>>(json);

        var find = data.Where(x => x.IdCharacter == id).FirstOrDefault();

        GetAvatar.GetAvatarURL(find.Avatar, _avatar, _imageDefault);

        _idCharacter = find.IdCharacter;
        _inputID.text = find.IdCharacter.ToString();
        _inputName.text = find.Name;
        _inputClan.text = find.Clan;
        _inputAge.text = find.Age.ToString();

    }

    public void POST_Register()
    {
        if (File.Exists(_path) == false) return;
        {
            var json = File.ReadAllText(_path);
            var data = JsonConvert.DeserializeObject<List<Character>>(json);

            var newUser = new Character
            {
                IdCharacter = data.Count + 1,
                Name = _inputName.text.ToUpper(),
                Clan = _inputClan.text.ToUpper(),
                Age = int.Parse(_inputAge.text),
                Avatar = $"{img}img{data.Count + 1}{radius}{background}"
            };

            WriteJSON(newUser);
            _labelDatabase.text = "Added Successfully";
            ClearFields();
        }
    }

    public void UPDATE_Register()
    {
        if (File.Exists(_path) == false) return;
        {
            var json = File.ReadAllText(_path);
            var data = JsonConvert.DeserializeObject<List<Character>>(json);

            var find = data.Where(x => x.IdCharacter == _idCharacter).FirstOrDefault();

            find.Name = _inputName.text.ToUpper();
            find.Clan = _inputClan.text.ToUpper();
            find.Age = int.Parse(_inputAge.text);
            find.Avatar = $"{img}img{data.Count + 1}{radius}{background}";

            WriteJSON(find);
            _labelDatabase.text = "Updated Successfully";
            ClearFields();
        }
    }

    public void DELETE_Register(int id)
    {
        Debug.Log("DELETE");
    }

    private void ClearFields()
    {
        _inputID.text = "";
        _inputName.text = "";
        _inputClan.text = "";
        _inputAge.text = "";
        _avatar.sprite = _imageDefault.sprite;
        _imageDefault.gameObject.SetActive(true);
        Task.Delay(2000).ContinueWith(t => _labelDatabase.text = "Database: OK");
    }

    private void WriteJSON(Character data)
    {
        try
        {
            List<Character> existingData = new List<Character>();
            if (File.Exists(_path))
            {
                string json = File.ReadAllText(_path);
                existingData = JsonConvert.DeserializeObject<List<Character>>(json);
            }

            existingData.Add(data);

            string updatedJson = JsonConvert.SerializeObject(existingData, Formatting.Indented);

            File.WriteAllText(_path, updatedJson);

            Debug.Log("File updated");
        }
        catch (Exception ex)
        {
            _labelPath.text = ex.Message.ToString();
            Debug.Log("Error: " + ex.Message.ToString());
        }
    }

    private void ReadJSON()
    {
        try
        {
            if (File.Exists(_path))
            {
                var json = File.ReadAllText(_path);
                var data = JsonConvert.DeserializeObject<List<Character>>(json);

                _labelPath.text = $"Path: {_path}";
            }
            else
            {
                _labelPath.text = "File not exists";
                Debug.Log("File not exists");
            }
        }
        catch (Exception ex)
        {
            _labelPath.text = ex.Message.ToString();
            Debug.Log("Error: " + ex.Message.ToString());
        }
    }
}

