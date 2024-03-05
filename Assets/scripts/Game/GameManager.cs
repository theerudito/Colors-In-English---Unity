using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.IO;
using Mono.Data.Sqlite;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private string fileName = "Colors.db";
    private string _developerEN = "MADE WITH BY BETWEEN BYTE SOFTWARE " + "- " + DateTime.Now.Year.ToString();
    private string _developerES = "HECHO POR ENTRE BYTE SOFTWARE " + "- " + DateTime.Now.Year.ToString();
    private int _score = 0;
    private int _points = 10;
    private int _time = 10;
    private float _updateInterval = 1f;
    private float _nextUpdateTime;
    private string _language = "EN";

    [SerializeField] private TextMeshProUGUI _labelPath;
    [SerializeField] private TextMeshProUGUI _labelScore;
    [SerializeField] private TextMeshProUGUI _labelTime;
    [SerializeField] private TextMeshProUGUI _labelColor;

    [SerializeField] private Button[] _buttonsColors;
    [SerializeField] private TextMeshProUGUI[] _textInButtons;

    [SerializeField] private AudioClip _soundCorrect;
    [SerializeField] private AudioClip _soundIncorrect;
    [SerializeField] private AudioClip _soundClock;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Sprite[] imgAvatar;
    [SerializeField] private Sprite[] imgLanguages;
    [SerializeField] private Sprite[] imgPremium;

    void Start()
    {
        CreateDatabase();

        _labelPath = GameObject.Find("labelPath").GetComponent<TextMeshProUGUI>();
        _labelScore = GameObject.Find("score").GetComponent<TextMeshProUGUI>();
        _labelTime = GameObject.Find("time").GetComponent<TextMeshProUGUI>();

        Image _imgPro = GameObject.Find("imgPro").GetComponent<Image>();
        _imgPro.sprite = imgPremium[1];

        Image _imgLanguage = GameObject.Find("imgLanguaje").GetComponent<Image>();
        _imgLanguage.sprite = imgLanguages[0];

        Image _imageDefault = GameObject.Find("imgDefault").GetComponent<Image>();
        _imageDefault.sprite = imgAvatar[0];

        _labelColor = GameObject.Find("labelColor").GetComponent<TextMeshProUGUI>();
        _audioSource = GameObject.Find("Canvas Game").GetComponent<AudioSource>();
        StartCoroutine(CountdownTimer());
        GenerateColors();
    }

    private void CreateDatabase()
    {
        if (!File.Exists(PathManager.GetPath(fileName)))
        {
            try
            {
                FileStream fileStream = File.Create(PathManager.GetPath(fileName));
                fileStream.Close();
                _labelPath.text = "DATABASE: CREATED";

                using (var connection = new SqliteConnection($"Data Source=" + PathManager.GetPath(fileName)))
                {
                    connection.Open();

                    var sql = "CREATE TABLE IF NOT EXISTS Color (IdColor INTEGER PRIMARY KEY, Name TEXT NOT NULL, Hex TEXT NOT NULL)";

                    connection.Execute(sql);

                    Colors.newColor.ForEach(color =>
                    {
                        sql = "INSERT INTO Color (Name, Hex) VALUES (@Name, @Hex)";
                        connection.Execute(sql, new { Name = color.Name, Hex = color.Hex });
                    });

                    connection.Close();
                }
                Task.Delay(2000).ContinueWith(t => _labelPath.text = "DATABASE: OK");
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message.ToString());
            }
        }
        else
        {
            _labelPath.text = "DATABASE: OK";
        }
    }

    public void CheckColor(TextMeshProUGUI color)
    {
        Image _imageDefault = GameObject.Find("imgDefault").GetComponent<Image>();

        if (_labelColor.text == color.text)
        {
            _score += _points;
            _labelScore.text = _score.ToString();
            _audioSource.clip = _soundCorrect;
            _audioSource.Play();
            _imageDefault.sprite = imgAvatar[1];
            StartCoroutine(CountdownTimer());
            StartCoroutine(ResetData());
        }
        else
        {
            if (_score == 0)
                _score = 10;
            else
                _score -= _points;
            _labelScore.text = _score.ToString();
            _audioSource.clip = _soundIncorrect;
            _audioSource.Play();
            _imageDefault.sprite = imgAvatar[2];
        }
    }

    private void GenerateColors()
    {
        using (var connection = new SqliteConnection($"Data Source=" + PathManager.GetPath(fileName)))
        {
            connection.Open();

            var sql = "SELECT * FROM Color";

            var colors = connection.Query<Colors>(sql).ToList();

            var colorRamdom = new System.Random().Next(1, colors.Count);

            var selectColor = colors.Where(x => x.IdColor == colorRamdom).FirstOrDefault();

            List<int> MyIndex = new List<int> { 1, 2, 3, 4, 5, selectColor.IdColor };

            MyIndex = MyIndex.OrderBy(x => new System.Random().Next()).ToList();

            _labelColor.text = selectColor.Name;

            for (int i = 0; i < _buttonsColors.Length; i++)
            {
                _buttonsColors[i].GetComponent<Image>().color = GetColor(colors.Where(x => x.IdColor == MyIndex[i]).FirstOrDefault().Hex);
                _textInButtons[i].text = colors.Where(x => x.IdColor == MyIndex[i]).FirstOrDefault().Name;
            }
            connection.Close();
        }
    }

    private Color GetColor(string hex)
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }

    private IEnumerator ResetData()
    {
        _buttonsColors.ToList().ForEach(btn => btn.interactable = false);
        yield return new WaitForSeconds(1f);
        _buttonsColors.ToList().ForEach(btn => btn.interactable = true);
        GenerateColors();
        _labelTime.color = Color.white;
        _time = 10;
        _labelTime.text = _time.ToString();
        _audioSource.clip = _soundClock;
        _audioSource.Stop();
        Image _imageDefault = GameObject.Find("imgDefault").GetComponent<Image>();
        _imageDefault.sprite = imgAvatar[0];
        StartCoroutine(CountdownTimer());
    }

    private IEnumerator CountdownTimer()
    {
        _nextUpdateTime = Time.time + _updateInterval;

        while (_time > 0)
        {
            yield return null;

            if (Time.time >= _nextUpdateTime)
            {
                _time--;
                _labelTime.text = _time.ToString();

                if (_time == 5)
                {
                    _labelTime.color = Color.red;
                    _audioSource.clip = _soundClock;
                    _audioSource.Play();
                }
                _nextUpdateTime += _updateInterval;
            }
        }
        StartCoroutine(ResetData());
    }

    public void LauncherURL(string url) => Application.OpenURL(url);

    public void ChangeLanguage()
    {
        // PlayerPrefs.SetString("Language", language);
        // PlayerPrefs.Save();
        // Application.Quit();

        TextMeshProUGUI _textFollow = GameObject.Find("textFollow").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI _labelDeveloper = GameObject.Find("textDeveloper").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI _textAnswer = GameObject.Find("textAnswer").GetComponent<TextMeshProUGUI>();
        Image _imgLanguage = GameObject.Find("imgLanguaje").GetComponent<Image>();

        if (_language == "EN")
        {
            _language = "ES";
            _labelDeveloper.text = _developerEN;
            _textFollow.text = "FOLLOW US";
            _textAnswer.text = "WHAT COLOR IS";
            _imgLanguage.sprite = imgLanguages[0];
        }
        else
        {
            _language = "EN";
            _labelDeveloper.text = _developerES;
            _textFollow.text = "SIGUENOS";
            _textAnswer.text = "QUE COLOR ES";
            _imgLanguage.sprite = imgLanguages[1];
        }
    }

    private void ShowBanner()
    {

    }

    private void GetPremium()
    {

    }

    private void BuyPremium()
    {

    }
}
