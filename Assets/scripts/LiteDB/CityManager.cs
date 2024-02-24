using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LiteDB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CityManager : MonoBehaviour
{
    private string fileName = "City.db";
    [SerializeField] private TextMeshProUGUI _infoPath;
    [SerializeField] private TextMeshProUGUI _database;
    [SerializeField] private TMP_InputField _inputID;
    [SerializeField] private TMP_InputField _inputCity;
    [SerializeField] public GameObject _spawnTBody;
    [SerializeField] public GameObject _tBody;
    [SerializeField] private float _verticalSpacing = 10f;

    private List<City> _cities = new List<City>();

    void Start()
    {
        _database = GameObject.Find("database").GetComponent<TextMeshProUGUI>();
        _infoPath = GameObject.Find("path").GetComponent<TextMeshProUGUI>();
        _infoPath.text = $"Path: {PathManager.GetPath(fileName)}";
        _inputID = GameObject.Find("inputID").GetComponent<TMP_InputField>();
        _inputCity = GameObject.Find("inputCity").GetComponent<TMP_InputField>();

        CreateDatabase();
        GetCity();
    }

    public void CreateDatabase()
    {
        if (!File.Exists(PathManager.GetPath(fileName)))
        {
            using (var db = new LiteDatabase(PathManager.GetPath(fileName)))
            {
                var collection = db.GetCollection<City>("Cities");
                _database.text = "Database Created";

                foreach (var item in City.newCity)
                {
                    collection.Insert(item);
                }
            }
        }
        else
        {
            _database.text = "Database: OK";
        }
    }

    public void GetCity()
    {
        ClearClones();

        using (var db = new LiteDatabase(PathManager.GetPath(fileName)))
        {
            var collection = db.GetCollection<City>("Cities");

            var obj = collection.FindAll();

            _cities.Clear();
            _cities.AddRange(obj);

            Vector3 spawnPosition = _tBody.transform.position + new Vector3(0f, -_verticalSpacing, 0f);

            foreach (var item in _cities)
            {
                GameObject newPanel = Instantiate(_tBody, spawnPosition, Quaternion.identity);

                newPanel.transform.SetParent(_spawnTBody.transform, false);

                newPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.IdCity.ToString();
                newPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.Name;

                Button button1 = newPanel.transform.GetChild(2).GetComponent<Button>();
                Button button2 = newPanel.transform.GetChild(3).GetComponent<Button>();

                button1.onClick.AddListener(() => GetCityId(item.IdCity));
                button2.onClick.AddListener(() => DeleteCity(item.IdCity));

                spawnPosition += new Vector3(0f, -(_verticalSpacing * 8), 0f);
            }
        }
    }

    public void GetCityId(ObjectId idCity)
    {
        using (var db = new LiteDatabase(PathManager.GetPath(fileName)))
        {
            var collection = db.GetCollection<City>("Cities");

            var obj = collection.FindById(idCity);

            _inputID.text = obj.IdCity.ToString();
            _inputCity.text = obj.Name;
        }
    }

    public void AddCity()
    {
        using (var db = new LiteDatabase(PathManager.GetPath(fileName)))
        {
            var collection = db.GetCollection<City>("Cities");

            if (string.IsNullOrEmpty(_inputCity.text.Trim().ToUpper()))
            {
                _database.text = "City is required";

                ResetFiel();

                return;
            }
            else
            {
                var findName = collection.FindOne(x => x.Name == _inputCity.text.Trim().ToUpper());

                if (findName != null)
                {
                    _database.text = "City already exists";

                    ResetFiel();

                    return;
                }
                else
                {
                    collection.Insert(new City { Name = _inputCity.text.Trim().ToUpper() });
                    _database.text = "Added Successfully";
                    ResetFiel();
                }
            }
        }
    }

    public void UpdateCity()
    {
        using (var db = new LiteDatabase(PathManager.GetPath(fileName)))
        {
            var collection = db.GetCollection<City>("Cities");

            if (string.IsNullOrEmpty(_inputID.text.Trim()))
            {
                _database.text = "ID is required";
                ResetFiel();
                return;
            }
            else if (string.IsNullOrEmpty(_inputCity.text.Trim().ToUpper()))
            {
                _database.text = "City is required";
                ResetFiel();
                return;
            }
            else
            {
                var obj = collection.FindById(new ObjectId(_inputID.text.Trim()));

                if (obj == null)
                {
                    _database.text = "City not found";
                    ResetFiel();
                    return;
                }
                else
                {

                    obj.Name = _inputCity.text.Trim().ToUpper();

                    collection.Update(obj);

                    _database.text = "Updated Successfully";

                    ResetFiel();
                }
            }
        }
    }

    public void DeleteCity(ObjectId idCity)
    {
        using (var db = new LiteDatabase(PathManager.GetPath(fileName)))
        {
            var collection = db.GetCollection<City>("Cities");

            collection.Delete(idCity);

            _database.text = "Deleted Successfully";

            ResetFiel();
        }
    }

    private void ResetFiel()
    {
        _inputID.text = "";
        _inputCity.text = "";
        GetCity();
        Task.Delay(2000).ContinueWith(t => _database.text = "Database: OK");
    }

    private void ClearClones()
    {
        foreach (Transform child in _spawnTBody.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
