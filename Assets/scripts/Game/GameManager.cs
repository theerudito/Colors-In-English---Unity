using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private string fileName = "Colors.db";
    private string _developer = "MADE WITH ❤️ BY BETWEEN BYTE SOFTWARE " + "- " + DateTime.Now.Year.ToString();
    [SerializeField] private TextMeshProUGUI _labelDeveloper;
    [SerializeField] private TextMeshProUGUI _labelPath;
    [SerializeField] private TextMeshProUGUI _labelScore;
    [SerializeField] private TextMeshProUGUI _labelTime;
    [SerializeField] private Image _imageHappy;
    [SerializeField] private Image _imageSad;
    [SerializeField] private Image _imageDefault;

    void Start()
    {
        _labelDeveloper = GameObject.Find("textDeveloper").GetComponent<TextMeshProUGUI>();
        _labelDeveloper.text = _developer;
        _labelPath = GameObject.Find("labelPath").GetComponent<TextMeshProUGUI>();
        _labelScore = GameObject.Find("labelScore").GetComponent<TextMeshProUGUI>();
        _labelTime = GameObject.Find("labelTime").GetComponent<TextMeshProUGUI>();
        _imageHappy = GameObject.Find("imgHappy").GetComponent<Image>();
        _imageSad = GameObject.Find("imgSad").GetComponent<Image>();
        _imageDefault = GameObject.Find("imgDefault").GetComponent<Image>();

        _labelPath.text = PathManager.GetPath(fileName);
    }
}
