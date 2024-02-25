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
    private string _developer = "MADE WITH BY BETWEEN BYTE SOFTWARE " + "- " + DateTime.Now.Year.ToString();
    private int _score = 0;
    private int _points = 10;
    private int _time = 30;
    private float _updateInterval = 1f;
    private float _nextUpdateTime;


    [SerializeField] private TextMeshProUGUI _labelDeveloper;
    [SerializeField] private TextMeshProUGUI _labelPath;
    [SerializeField] private TextMeshProUGUI _labelScore;
    [SerializeField] private TextMeshProUGUI _labelTime;
    [SerializeField] private Image _imageHappy;
    [SerializeField] private Image _imageSad;
    [SerializeField] private Image _imageDefault;
    [SerializeField] private TextMeshProUGUI _labelColor;
    [SerializeField] private Button[] _buttonsColors;
    [SerializeField] private TextMeshProUGUI[] _textInButtons;
    [SerializeField] private AudioClip _soundCorrect;
    [SerializeField] private AudioClip _soundIncorrect;
    [SerializeField] private AudioClip _soundClock;
    [SerializeField] private AudioSource _audioSource;

    void Start()
    {
        CreateDatabase();
        _labelDeveloper = GameObject.Find("textDeveloper").GetComponent<TextMeshProUGUI>();
        _labelDeveloper.text = _developer;
        _labelPath = GameObject.Find("labelPath").GetComponent<TextMeshProUGUI>();
        _labelScore = GameObject.Find("score").GetComponent<TextMeshProUGUI>();
        _labelTime = GameObject.Find("time").GetComponent<TextMeshProUGUI>();
        _imageHappy = GameObject.Find("imgHappy").GetComponent<Image>();
        _imageSad = GameObject.Find("imgSad").GetComponent<Image>();
        _imageDefault = GameObject.Find("imgDefault").GetComponent<Image>();
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
        if (_labelColor.text == color.text)
        {
            _score += _points;
            _labelScore.text = _score.ToString();
            _imageHappy.gameObject.SetActive(true);
            _imageSad.gameObject.SetActive(false);
            _imageDefault.gameObject.SetActive(false);
            _audioSource.clip = _soundCorrect;
            _audioSource.Play();
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
            _imageHappy.gameObject.SetActive(false);
            _imageSad.gameObject.SetActive(true);
            _imageDefault.gameObject.SetActive(false);
            _audioSource.clip = _soundIncorrect;
            _audioSource.Play();

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
        _imageHappy.gameObject.SetActive(false);
        _imageSad.gameObject.SetActive(false);
        _imageDefault.gameObject.SetActive(true);
        GenerateColors();
        _labelTime.color = Color.white;
        _time = 30;
        _labelTime.text = _time.ToString();
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

                if (_time == 15)
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
    public void LauncherURL(string url)
    {
        Application.OpenURL(url);
    }
}
