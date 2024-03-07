using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using GoogleMobileAds.Api;


public class GameManager : MonoBehaviour
{
    private string _developerEN = "MADE BY BETWEEN BYTE SOFTWARE " + "- " + DateTime.Now.Year.ToString();
    private string _developerES = "HECHO POR BETWEEN BYTE SOFTWARE " + "- " + DateTime.Now.Year.ToString();
    private int _score = 0;
    private int _points = 10;
    private int _time = 10;
    private float _updateInterval = 1f;
    private float _nextUpdateTime;
    private string _language = "EN";
    private bool _sound = true;

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
    [SerializeField] private Sprite[] imgSound;
    [SerializeField] private GameObject[] _panels;

    void Awake()
    {
        InAppManager.Instance.OpenStore();

        Button btnAds = GameObject.Find("btnAds").GetComponent<Button>();


        if (PlayerPrefs.GetString("removeAds") == "true")
        {
            btnAds.GetComponent<Image>().sprite = imgPremium[1];
            btnAds.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
        }
        else
        {
            btnAds.GetComponent<Image>().sprite = imgPremium[0];
        }
    }
    void Start()
    {

        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            //AdsBanner.Instance.LoadAdsBanner();
            //AdsIntersticial.Instance.LoadAdsIntersticial();
            //AdsRewarded.Instance.LoadAdsRewarded();
        });


        _panels.ToList().ForEach(panel => panel.SetActive(false));

        _labelPath = GameObject.Find("labelPath").GetComponent<TextMeshProUGUI>();
        _labelScore = GameObject.Find("score").GetComponent<TextMeshProUGUI>();
        _labelTime = GameObject.Find("time").GetComponent<TextMeshProUGUI>();
        _labelColor = GameObject.Find("labelColor").GetComponent<TextMeshProUGUI>();

        _labelPath.text = "DATABASE: OK";

        Image _imageDefault = GameObject.Find("imgDefault").GetComponent<Image>();
        _imageDefault.sprite = imgAvatar[0];

        _labelColor = GameObject.Find("labelColor").GetComponent<TextMeshProUGUI>();
        _audioSource = GameObject.Find("Canvas Game").GetComponent<AudioSource>();

        StartCoroutine(CountdownTimer());
        GenerateColors();
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
        if (_language == "ES")
        {
            var colorsEN = Colors.newColorEN.ToList();

            var colorRamdom = new System.Random().Next(1, colorsEN.Count);

            var selectColor = colorsEN.Where(x => x.IdColor == colorRamdom).FirstOrDefault();

            List<int> MyIndex = new List<int> { 1, 2, 3, 4, 5, selectColor.IdColor };

            MyIndex = MyIndex.OrderBy(x => new System.Random().Next()).ToList();

            _labelColor.text = selectColor.Name;

            for (int i = 0; i < _buttonsColors.Length; i++)
            {
                _buttonsColors[i].GetComponent<Image>().color = GetColor(colorsEN.Where(x => x.IdColor == MyIndex[i]).FirstOrDefault().Hex);
                _textInButtons[i].text = colorsEN.Where(x => x.IdColor == MyIndex[i]).FirstOrDefault().Name;
            }
        }
        else
        {
            var colorsES = Colors.newColorES.ToList();

            var colorRamdom = new System.Random().Next(1, colorsES.Count);

            var selectColor = colorsES.Where(x => x.IdColor == colorRamdom).FirstOrDefault();

            List<int> MyIndex = new List<int> { 1, 2, 3, 4, 5, selectColor.IdColor };

            MyIndex = MyIndex.OrderBy(x => new System.Random().Next()).ToList();

            _labelColor.text = selectColor.Name;

            for (int i = 0; i < _buttonsColors.Length; i++)
            {
                _buttonsColors[i].GetComponent<Image>().color = GetColor(colorsES.Where(x => x.IdColor == MyIndex[i]).FirstOrDefault().Hex);
                _textInButtons[i].text = colorsES.Where(x => x.IdColor == MyIndex[i]).FirstOrDefault().Name;
            }
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
        Image _imgLanguage = GameObject.Find("imgLanguage").GetComponent<Image>();

        if (_language == "EN")
        {
            _language = "ES";
            _labelDeveloper.text = _developerEN;
            _textFollow.text = "FOLLOW US";
            _textAnswer.text = "WHAT COLOR IS";
            _imgLanguage.sprite = imgLanguages[0];

            GenerateColors();
        }
        else
        {
            _language = "EN";
            _labelDeveloper.text = _developerES;
            _textFollow.text = "SIGUENOS";
            _textAnswer.text = "QUE COLOR ES";
            _imgLanguage.sprite = imgLanguages[1];
            GenerateColors();
        }

        _panels.FirstOrDefault(panel => panel.name == "panelConfig").SetActive(false);
    }

    public void BuyPremium(string productId)
    {
        InAppManager.Instance.BuyProductID(productId);

        Button btnAds = GameObject.Find("btnAds").GetComponent<Button>();

        if (imgPremium[0] == btnAds.GetComponent<Image>().sprite)
        {
            btnAds.GetComponent<Image>().sprite = imgPremium[0];
        }
        else
        {
            btnAds.GetComponent<Image>().sprite = imgPremium[1];
            //btnAds.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
        }
    }

    public void OpenPanel(string panelName)
    {
        if (panelName == "panelConfig")
        {
            _panels.FirstOrDefault(panel => panel.name == "panelConfig").SetActive(true);
            _panels.FirstOrDefault(panel => panel.name == "panelInfo").SetActive(false);
        }
        else
        {
            _panels.FirstOrDefault(panel => panel.name == "panelConfig").SetActive(false);
            _panels.FirstOrDefault(panel => panel.name == "panelInfo").SetActive(true);
        }
    }

    public void ClosePanel(string panelName)
    {
        _panels.FirstOrDefault(panel => panel.name == "panelInfo").SetActive(false);
    }

    public void Sound()
    {
        Image _imgSound = _panels.FirstOrDefault(panel => panel.name == "panelConfig").transform.Find("imgSound").GetComponent<Image>();

        if (_sound)
        {
            _audioSource.mute = true;
            _imgSound.sprite = imgSound[0];
            _sound = false;
            _panels.FirstOrDefault(panel => panel.name == "panelConfig").SetActive(false);
        }
        else
        {
            _audioSource.mute = false;
            _imgSound.sprite = imgSound[1];
            _sound = true;
            _panels.FirstOrDefault(panel => panel.name == "panelConfig").SetActive(false);
        }
    }
}
