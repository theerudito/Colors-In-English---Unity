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
    private string _premium = "premium";
    private int _score = 0;
    private int _points = 10;
    private int _time = 10;
    private float _updateInterval = 1f;
    private float _nextUpdateTime;
    private string _language = "EN";
    private bool _flagEN = true;
    private bool _sound = true;
    private bool _isOpen = false;

    [Header("AUDIO SOURCE")]
    [SerializeField] private AudioSource _audioSource;

    [Header("LABELS")]
    [SerializeField] private TextMeshProUGUI[] _labels;

    [Header("BUTTONS GAME")]
    [SerializeField] private Button[] _buttonsColors;

    [Header("TEXT IN BUTTONS")]
    [SerializeField] private TextMeshProUGUI[] _textInButtons;

    [Header("SOUND FILES")]
    [SerializeField] private AudioClip[] _soundFiles;

    [Header("IMAGES AVATAR")]
    [SerializeField] private Sprite[] imgAvatar;

    [Header("IMAGES LANGUAGE")]
    [SerializeField] private Sprite[] imgLanguages;

    [Header("IMAGES PREMIUM")]
    [SerializeField] private Sprite[] imgPremium;

    [Header("IMAGES SOUND")]
    [SerializeField] private Sprite[] imgSound;

    [Header("PANELS")]
    [SerializeField] private GameObject[] _panels;

    void Awake()
    {
        _isOpen = false;

        InAppManager.Instance.OpenStore();

        Button btnAds = GameObject.Find("btnAds").GetComponent<Button>();

        if (LocalStorage.LoadKey(_premium) == true || InAppManager.Instance.HasPurchasedNonConsumable(_premium) == true)
        {
            LocalStorage.SaveData(_premium, _premium);
            btnAds.GetComponent<Image>().sprite = imgPremium[1];
            btnAds.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        }
        else
        {
            btnAds.GetComponent<Image>().sprite = imgPremium[0];

            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                AdsBanner.Instance.LoadAdsBanner();
                //AdsIntersticial.Instance.LoadAdsIntersticial();
                AdsRewarded.Instance.LoadAdsRewarded();
            });
        }

        #region SOUND
        if (LocalStorage.LoadKey("Sound") == true)
        {
            _sound = LocalStorage.LoadData("Sound") == "ON" ? true : false;

            if (_sound == true)
            {
                _audioSource.mute = false;
                GameObject.Find("imgSound").GetComponent<Image>().sprite = imgSound[1];
            }
            else
            {
                _audioSource.mute = true;
                GameObject.Find("imgSound").GetComponent<Image>().sprite = imgSound[0];
            }
        }
        else
        {
            _sound = true;
        }
        #endregion SOUND

        #region LANGUAGE
        if (LocalStorage.LoadKey("Language") == true)
        {
            _language = LocalStorage.LoadData("Language");

            if (_language == "EN")
            {
                GameObject.Find("imgLanguage").GetComponent<Image>().sprite = imgLanguages[1];
            }
            else
            {
                GameObject.Find("imgLanguage").GetComponent<Image>().sprite = imgLanguages[0];
            }
        }
        else
        {
            _language = "EN";
            GameObject.Find("imgLanguage").GetComponent<Image>().sprite = imgLanguages[0];
        }
        #endregion LANGUAGE

        #region  SCORE
        if (LocalStorage.LoadKey("Score") == true)
        {
            _score = int.Parse(LocalStorage.LoadData("Score"));
            _labels[3].text = _score.ToString();
        }
        else
        {
            _score = 0;
            _labels[3].text = _score.ToString();
        }
        #endregion SCORE
    }

    void Start()
    {
        _labels[0] = GameObject.Find("labelTime").GetComponent<TextMeshProUGUI>();
        _labels[1] = GameObject.Find("time").GetComponent<TextMeshProUGUI>();
        _labels[2] = GameObject.Find("labelScore").GetComponent<TextMeshProUGUI>();
        _labels[3] = GameObject.Find("score").GetComponent<TextMeshProUGUI>();
        _labels[4] = GameObject.Find("labelPath").GetComponent<TextMeshProUGUI>();
        _labels[5] = GameObject.Find("textAnswer").GetComponent<TextMeshProUGUI>();
        _labels[6] = GameObject.Find("labelColor").GetComponent<TextMeshProUGUI>();
        _labels[7] = GameObject.Find("textFollow").GetComponent<TextMeshProUGUI>();
        _labels[8] = GameObject.Find("textDeveloper").GetComponent<TextMeshProUGUI>();
        _labels[9] = GameObject.Find("labelStore").GetComponent<TextMeshProUGUI>();

        _labels[0].text = _language == "EN" ? "TIME" : "TIEMPO";
        _labels[2].text = _language == "EN" ? "SCORE" : "PUNTOS";
        _labels[4].text = _language == "EN" ? "DATABASE: OK" : "BASE DE DATOS: OK";
        _labels[5].text = _language == "EN" ? "WHAT COLOR IS" : "QUE COLOR ES";
        _labels[7].text = _language == "EN" ? "FOLLOW US" : "SIGUENOS";
        _labels[8].text = _language == "EN" ? _developerEN : _developerES;


        _panels.ToList().ForEach(panel => panel.SetActive(false));

        GameObject.Find("imgDefault").GetComponent<Image>().sprite = imgAvatar[0];

        _audioSource = GameObject.Find("Canvas Game").GetComponent<AudioSource>();

        StartCoroutine(CountdownTimer());

        GenerateColors();
    }

    public void CheckColor(int index)
    {
        var color = _buttonsColors[index].GetComponentInChildren<TextMeshProUGUI>();

        Image _imageDefault = GameObject.Find("imgDefault").GetComponent<Image>();

        if (_labels[6].text == color.text)
        {
            _score += _points;
            LocalStorage.SaveData("Score", _score.ToString());
            _labels[3].text = _score.ToString();
            _audioSource.clip = _soundFiles[0];
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
            LocalStorage.SaveData("Score", _score.ToString());
            _labels[3].text = _score.ToString();
            _audioSource.clip = _soundFiles[1];
            _audioSource.Play();
            _imageDefault.sprite = imgAvatar[2];
            _buttonsColors[index].interactable = false;
            _buttonsColors[index].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
    }

    private void GenerateColors()
    {
        if (_language == "EN")
        {
            var colorsEN = Colors.newColorEN.ToList();

            var colorRamdom = new System.Random().Next(1, colorsEN.Count);

            var selectColor = colorsEN.Where(x => x.IdColor == colorRamdom).FirstOrDefault();

            List<int> MyIndex = new List<int> { 1, 2, 3, 4, 5, selectColor.IdColor };

            MyIndex = MyIndex.OrderBy(x => new System.Random().Next()).ToList();

            _labels[6].text = selectColor.Name;


            if (selectColor.Name.Length < 7)
            {
                _labels[6].fontSize = 60;
            }
            else if (selectColor.Name.Length < 10)
            {
                _labels[6].fontSize = 50;
            }
            else
            {
                _labels[6].fontSize = 40;
            }


            _flagEN = false;

            for (int i = 0; i < _buttonsColors.Length; i++)
            {
                _buttonsColors[i].GetComponent<Image>().color = GetColor(colorsEN.Where(x => x.IdColor == MyIndex[i]).FirstOrDefault().Hex);
                _textInButtons[i].text = colorsEN.Where(x => x.IdColor == MyIndex[i]).FirstOrDefault().Name;
                _textInButtons[i].color = Color.white;
                _textInButtons[i].color = new Color(0, 0, 0, 0);
            }
        }
        else
        {
            var colorsES = Colors.newColorES.ToList();

            var colorRamdom = new System.Random().Next(1, colorsES.Count);

            var selectColor = colorsES.Where(x => x.IdColor == colorRamdom).FirstOrDefault();

            List<int> MyIndex = new List<int> { 1, 2, 3, 4, 5, selectColor.IdColor };

            MyIndex = MyIndex.OrderBy(x => new System.Random().Next()).ToList();

            _labels[6].text = selectColor.Name;

            if (selectColor.Name.Length < 7)
            {
                _labels[6].fontSize = 60;
            }
            else if (selectColor.Name.Length < 10)
            {
                _labels[6].fontSize = 50;
            }
            else
            {
                _labels[6].fontSize = 40;
            }

            _flagEN = true;

            for (int i = 0; i < _buttonsColors.Length; i++)
            {
                _buttonsColors[i].GetComponent<Image>().color = GetColor(colorsES.Where(x => x.IdColor == MyIndex[i]).FirstOrDefault().Hex);
                _textInButtons[i].text = colorsES.Where(x => x.IdColor == MyIndex[i]).FirstOrDefault().Name;
                _textInButtons[i].color = new Color(0, 0, 0, 0);
            }
        }
    }

    private Color GetColor(string hex)
    {
        Color color = new Color();
        UnityEngine.ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }

    private IEnumerator ResetData()
    {
        _buttonsColors.ToList().ForEach(btn => btn.interactable = false);
        _buttonsColors.ToList().ForEach(btn => btn.GetComponentInChildren<TextMeshProUGUI>().color = Color.black);

        yield return new WaitForSeconds(1f);

        _buttonsColors.ToList().ForEach(btn => btn.interactable = true);
        _buttonsColors.ToList().ForEach(btn => btn.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0, 0, 0, 0));

        GenerateColors();

        _labels[1].color = Color.white;

        _time = 10;

        _labels[1].text = _time.ToString();

        _audioSource.clip = _soundFiles[2];

        _audioSource.Stop();

        GameObject.Find("imgDefault").GetComponent<Image>().sprite = imgAvatar[0];

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
                _labels[1].text = _time.ToString();

                if (_time == 5)
                {
                    _labels[1].color = Color.red;
                    _audioSource.clip = _soundFiles[2];
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
        ClickSound();

        Image _imgLanguage = GameObject.Find("imgLanguage").GetComponent<Image>();

        if (_flagEN == true)
        {
            _flagEN = false;
            _language = "EN";
            _labels[0].text = "TIME";
            _labels[2].text = "SCORE";
            _labels[4].text = "DATABASE: OK";
            _labels[5].text = "WHAT COLOR IS";
            _labels[7].text = "FOLLOW US";
            _labels[8].text = _developerEN;
            _imgLanguage.sprite = imgLanguages[1];
            LocalStorage.SaveData("Language", _language);
            GenerateColors();
        }
        else
        {
            _flagEN = true;
            _language = "ES";
            _labels[0].text = "TIEMPO";
            _labels[2].text = "PUNTOS";
            _labels[4].text = "BASE DE DATOS: OK";
            _labels[5].text = "QUE COLOR ES";
            _labels[7].text = "SIGUENOS";
            _labels[8].text = _developerES;
            _imgLanguage.sprite = imgLanguages[0];
            LocalStorage.SaveData("Language", _language);
            GenerateColors();
        }

        ClosePanel("panelConfig");
    }

    public void ChangeSound()
    {
        Image _imgSound = _panels.FirstOrDefault(panel => panel.name == "panelConfig").transform.Find("imgSound").GetComponent<Image>();
        ClickSound();
        if (_sound)
        {
            _audioSource.mute = true;
            _imgSound.sprite = imgSound[0];
            LocalStorage.SaveData("Sound", "OFF");
            _sound = false;
            ClosePanel("panelConfig");
        }
        else
        {
            _audioSource.mute = false;
            _imgSound.sprite = imgSound[1];
            LocalStorage.SaveData("Sound", "ON");
            _sound = true;
            ClosePanel("panelConfig");
        }
    }

    public void BuyPremium()
    {

        ClickSound();

        Button btnAds = GameObject.Find("btnAds").GetComponent<Button>();

        if (InAppManager.Instance.HasPurchasedNonConsumable(_premium) == true)
        {
            OpenPanel("panelInfo");
            _labels[9].text = _language == "EN" ? "YOU ARE ALREADY PREMIUM" : "YA ERES PREMIUM";
            LocalStorage.SaveData(_premium, _premium);
            btnAds.GetComponent<Image>().sprite = imgPremium[1];
            btnAds.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
            return;
        }
        else
        {
            InAppManager.OnPurchaseSuccess += HandleSuccessfulPurchase;
            InAppManager.OnPurchaseError += HandleFailedPurchase;

            InAppManager.Instance.BuyNonConsumable(_premium);
        }
    }

    void HandleSuccessfulPurchase(string productId)
    {
        Button btnAds = GameObject.Find("btnAds").GetComponent<Button>();
        Debug.Log("Compra exitosa para el producto con ID: " + productId);
        InAppManager.Instance.SetPurchaseMessage(_language == "EN" ? "THANKS FOR YOUR PURCHASE" : "GRACIAS POR TU COMPRA");
        OpenPanel("panelInfo");
        _labels[9].text = InAppManager.Instance.GetPurchaseMessage();
        btnAds.GetComponent<Image>().sprite = imgPremium[1];
        btnAds.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        LocalStorage.SaveData(_premium, _premium);
        OnDisable();
    }

    void HandleFailedPurchase(string productId)
    {
        Button btnAds = GameObject.Find("btnAds").GetComponent<Button>();
        Debug.Log("Compra fallida para el producto con ID: " + productId);
        InAppManager.Instance.SetPurchaseMessage(_language == "EN" ? "PURCHASE FAILED" : "COMPRA FALLIDA");
        OpenPanel("panelInfo");
        _labels[9].text = InAppManager.Instance.GetPurchaseMessage();
        btnAds.GetComponent<Image>().sprite = imgPremium[0];
        OnDisable();
    }

    void OnDisable()
    {
        InAppManager.OnPurchaseSuccess -= HandleSuccessfulPurchase;
        InAppManager.OnPurchaseError -= HandleFailedPurchase;
    }

    public void OpenPanel(string panelName)
    {
        if (panelName == "panelConfig")
        {
            if (_isOpen == false)
            {
                _panels.FirstOrDefault(panel => panel.name == panelName).SetActive(true);
                RotateButton(true);
                ClickSound();
                _isOpen = true;
            }
            else
            {
                _panels.FirstOrDefault(panel => panel.name == panelName).SetActive(false);
                ClickSound();
                RotateButton(false);
                _isOpen = false;
            }

        }
        if (panelName == "panelInfo")
        {
            _panels.FirstOrDefault(panel => panel.name == panelName).SetActive(true);
        }
    }

    public void ClosePanel(string panelName)
    {

        if (panelName == "panelConfig")
        {
            _isOpen = false;
            _panels.FirstOrDefault(panel => panel.name == panelName).SetActive(false);
            RotateButton(false);
            ClickSound();
        }
        if (panelName == "panelInfo")
        {
            _panels.FirstOrDefault(panel => panel.name == panelName).SetActive(false);
            ClickSound();
        }
    }

    private void ClickSound()
    {
        _audioSource.clip = _soundFiles[3];
        _audioSource.volume = 0.5f;
        _audioSource.Play();
    }

    private void RotateButton(bool isRotate)
    {
        if (isRotate == true)
        {
            Button btnConfig = GameObject.Find("btnOpen").GetComponent<Button>();
            btnConfig.transform.Rotate(0, 0, -40);
        }
        else
        {
            Button btnConfig = GameObject.Find("btnOpen").GetComponent<Button>();
            btnConfig.transform.Rotate(0, 0, 40);
        }
    }
}
