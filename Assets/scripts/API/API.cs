using System.Net.Http;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class API : MonoBehaviour
{

    public class Data
    {
        public int id { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string avatar { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
    }

    private int[] lista = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

    [SerializeField]
    private Image _avatar;

    [SerializeField]
    private Image _avatarDefault;

    [SerializeField]
    private TextMeshProUGUI _labelId;

    [SerializeField]
    private TextMeshProUGUI _labelFirstName;

    [SerializeField]
    private TextMeshProUGUI _labelLastName;

    [SerializeField]
    private TextMeshProUGUI _labelEmail;

    [SerializeField]
    private TMP_InputField _input;

    [SerializeField]
    private TextMeshProUGUI _infoNetwork;

    private TMP_Dropdown _myList;

    private string url = "https://reqres.in/api/users";


    void Start()
    {

        _myList = GameObject.Find("myList").GetComponent<TMP_Dropdown>();
        _myList.ClearOptions();

        _myList.captionText.text = "Select an ID";

        _myList.captionText.fontSize = 30;

        foreach (var item in lista)
        {
            _myList.options.Add(new TMP_Dropdown.OptionData() { text = item.ToString() });
        }

        _infoNetwork = GameObject.Find("infoNetwork").GetComponent<TextMeshProUGUI>();

        _avatar = GameObject.Find("avatar").GetComponent<Image>();
        _avatarDefault = GameObject.Find("avatarDefault").GetComponent<Image>();

        _input = GameObject.Find("input").GetComponent<TMP_InputField>();

        _labelId = GameObject.Find("labelID").GetComponent<TextMeshProUGUI>();
        _labelFirstName = GameObject.Find("labelFirstName").GetComponent<TextMeshProUGUI>();
        _labelLastName = GameObject.Find("labelLastName").GetComponent<TextMeshProUGUI>();
        _labelEmail = GameObject.Find("labelEmail").GetComponent<TextMeshProUGUI>();

        _input.text = 1.ToString();

        GetUserInfo();
    }

    public async void GetUserInfo()
    {
        try
        {
            if (ValidateNumber(_input.text.Trim()) == false)
            {
                _infoNetwork.text = "Network: User not found";
                await Task.Delay(2000).ContinueWith(t => _infoNetwork.text = "Network: OK");
                return;
            }
            else
            {
                HttpClient fetch = new HttpClient();
                var response = await fetch.GetAsync($"{url}/{_input.text.Trim()}");

                var result = await response.Content.ReadAsStringAsync();

                var json = JsonConvert.DeserializeObject<Root>(result).data;

                _infoNetwork.text = "Network: OK";

                GetAvatar.GetAvatarURL(json.avatar, _avatar, _avatarDefault);

                _labelId.text = json.id.ToString();
                _labelFirstName.text = json.first_name;
                _labelLastName.text = json.last_name;
                _labelEmail.text = json.email;
                _infoNetwork.text = "Network: OK";
            }
        }
        catch (Exception ex)
        {
            _infoNetwork.text = ex.Message.ToString();
            Debug.LogError(ex.Message.ToString());
        }
    }

    public void SelectOption()
    {
        _input.text = _myList.options[_myList.value].text;
        GetUserInfo();
    }

    private bool ValidateNumber(string input)
    {
        string regx = @"^(1[0-2]|[1-9])$";
        return Regex.IsMatch(input, regx);
    }
}
