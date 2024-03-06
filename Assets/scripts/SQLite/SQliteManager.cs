using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mono.Data.Sqlite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SQliteManager : MonoBehaviour
{
    private string fileName = "Colors.db";
    [SerializeField] private TextMeshProUGUI _labelPath;
    [SerializeField] private TextMeshProUGUI _labelDatabase;
    [SerializeField] private TMP_InputField _inputID;
    [SerializeField] private TMP_InputField _inputColor;
    [SerializeField] private Button _buttonGetById;
    [SerializeField] private Button _buttonDelete;
    [SerializeField] public GameObject _spawnTBody;
    [SerializeField] public GameObject _tBody;
    [SerializeField] private float _verticalSpacing = 10f;

    private List<Colors> _colors = new List<Colors>();
    void Start()
    {
        CreateDatabase();

        _labelPath.text = $"Path: {PathManager.GetPath(fileName)}";

        _labelPath = GameObject.Find("labelpath").GetComponent<TextMeshProUGUI>();
        _labelDatabase = GameObject.Find("labelDatabase").GetComponent<TextMeshProUGUI>();

        _inputID = GameObject.Find("inputIdColor").GetComponent<TMP_InputField>();
        _inputColor = GameObject.Find("inputNameColor").GetComponent<TMP_InputField>();

        GetColors();
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

                    Colors.newColorEN.ForEach(color =>
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

    public void GetColors()
    {
        ClearClones();
        using (var connection = new SqliteConnection($"Data Source=" + PathManager.GetPath(fileName)))
        {
            connection.Open();

            var sql = "SELECT * FROM Color LIMIT 10";

            var obj = connection.Query<Colors>(sql).ToList();

            _colors.Clear();

            _colors.AddRange(obj);

            connection.Close();

            Vector3 spawnPosition = _tBody.transform.position + new Vector3(0f, -_verticalSpacing, 0f);

            foreach (var item in _colors)
            {
                GameObject newPanel = Instantiate(_tBody, spawnPosition, Quaternion.identity);

                newPanel.transform.SetParent(_spawnTBody.transform, false);

                TextMeshProUGUI idLabel = newPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI nameLabel = newPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                Button button1 = newPanel.transform.GetChild(2).GetComponent<Button>();
                Button button2 = newPanel.transform.GetChild(3).GetComponent<Button>();

                idLabel.text = item.IdColor.ToString();
                nameLabel.text = item.Name;

                button1.onClick.AddListener(() => GetColorId(item.IdColor));

                button2.onClick.AddListener(() => DeleteColor(item.IdColor));

                spawnPosition += new Vector3(0f, -(_verticalSpacing * 11), 0f);
            }
        }
    }
    public void GetColorId(int idColor)
    {
        using (var connection = new SqliteConnection($"Data Source=" + PathManager.GetPath(fileName)))
        {
            connection.Open();

            var sql = "SELECT * FROM Color WHERE IdColor = @IdColor";

            var obj = connection.Query<Colors>(sql, new { IdColor = idColor }).FirstOrDefault();

            connection.Close();

            _inputID.text = obj.IdColor.ToString();
            _inputColor.text = obj.Name;
        }
    }
    public void AddColor()
    {
        using (var connection = new SqliteConnection($"Data Source=" + PathManager.GetPath(fileName)))
        {
            connection.Open();

            if (string.IsNullOrEmpty(_inputColor.text))
            {
                _labelDatabase.text = "Color is required";
                ResetFields();
                return;
            }
            else
            {
                var findName = connection.Query<Colors>("SELECT * FROM Color WHERE Name = @Name", new { Name = _inputColor.text.Trim().ToUpper() }).FirstOrDefault();

                if (findName != null)
                {
                    _labelDatabase.text = "Color already exists";

                    ResetFields();

                    return;
                }
                else
                {
                    var sql = "INSERT INTO Color (Name) VALUES (@Name)";

                    connection.Execute(sql, new { Name = _inputColor.text.Trim().ToUpper() });

                    _labelDatabase.text = "Color added";

                    ResetFields();
                }
                connection.Close();
            }
        }
    }
    public void DeleteColor(int idColor)
    {
        using (var connection = new SqliteConnection($"Data Source=" + PathManager.GetPath(fileName)))
        {
            connection.Open();

            var sql = "DELETE FROM Color WHERE IdColor = @IdColor";

            connection.Execute(sql, new { IdColor = idColor });

            ResetFields();

            connection.Close();
        }
    }
    public void UpdateColor()
    {
        using (var connection = new SqliteConnection($"Data Source=" + PathManager.GetPath(fileName)))
        {
            connection.Open();

            var sql = "UPDATE Color SET Name = @Name WHERE IdColor = @IdColor";

            if (string.IsNullOrEmpty(_inputID.text))
            {
                _labelDatabase.text = "ID is required";
                ResetFields();
                return;
            }
            else if (string.IsNullOrEmpty(_inputColor.text))
            {
                _labelDatabase.text = "Color is required";
                ResetFields();
                return;
            }
            else
            {
                connection.Execute(sql, new { Name = _inputColor.text.Trim().ToUpper(), IdColor = _inputID.text });

                _labelDatabase.text = "Color updated";

                ResetFields();

                connection.Close();
            }


        }
    }
    private void ResetFields()
    {
        _inputID.text = "";
        _inputColor.text = "";
        GetColors();
        Task.Delay(2000).ContinueWith(t => _labelDatabase.text = "Database: OK");
    }
    private void ClearClones()
    {
        foreach (Transform child in _spawnTBody.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
