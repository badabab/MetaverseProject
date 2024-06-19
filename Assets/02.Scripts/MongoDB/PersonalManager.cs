using MongoDB.Bson;
using MongoDB.Driver;
using Photon.Pun;
using Photon.Voice.PUN;
using System.Collections.Generic;
using UnityEngine;

public class PersonalManager : MonoBehaviour
{
    private List<Personal> _personal = new List<Personal>();
    public List<Personal> Personals => _personal;

    private IMongoCollection<Personal> _personalCollection;
    public static PersonalManager Instance { get; private set; }

    private string _cachedUserName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        string connectionString = "mongodb+srv://MetaversePro:MetaversePro@cluster0.ed1au27.mongodb.net/";
        MongoClient mongoClient = new MongoClient(connectionString);
        IMongoDatabase db = mongoClient.GetDatabase("Logins");
        _personalCollection = db.GetCollection<Personal>("Log");
    }

    public void JoinList(string name, string password, int characterIndex)
    {
        Personal personal = new Personal()
        {
            Name = name,
            Password = password,
            CharacterIndex = characterIndex
        };
        _personalCollection.InsertOne(personal);
    }

    public Personal Login(string name, string password) 
    {
        var filter = Builders<Personal>.Filter.Eq("Name", name) & Builders<Personal>.Filter.Eq("Password", password);
        return _personalCollection.Find(filter).FirstOrDefault();
    }

    public bool CheckUser(string name, string password)
    {
        var filter = Builders<Personal>.Filter.Eq("Name", name) & Builders<Personal>.Filter.Eq("Password", password);
        return _personalCollection.Find(filter).Any();
    }

    public bool ChangingNickName(string newName)
    {
        // newName이 null 또는 빈 문자열인지 확인
        if (string.IsNullOrEmpty(newName))
        {
            Debug.LogError("새 이름이 유효하지 않습니다.");
            return false;
        }

        string originalName = GetCachedUserName();
        if (string.IsNullOrEmpty(originalName)) return false;

        var filter = Builders<Personal>.Filter.Eq("Name", originalName);
        var update = Builders<Personal>.Update.Set("Name", newName);

        var result = _personalCollection.UpdateOne(filter, update);
        Debug.Log($"MongoDB Update Result: {result}");

        if (result.ModifiedCount > 0)
        {
            _cachedUserName = newName; // Update cache
            PlayerPrefs.SetString("LoggedInId", newName);
            PlayerPrefs.Save();
            PhotonNetwork.NickName = newName;
            Debug.Log($"Updated NickName to: {PhotonNetwork.NickName}");
            return true;
        }
        return false;
    }

    private string GetCachedUserName()
    {
        if (string.IsNullOrEmpty(_cachedUserName))
        {
            _cachedUserName = PlayerPrefs.GetString("LoggedInId");

            if (string.IsNullOrEmpty(_cachedUserName))
            {
                Debug.LogError("사용자 이름을 찾을 수 없습니다.");
                return string.Empty;
            }

            var filter = Builders<Personal>.Filter.Eq("Name", _cachedUserName);
            if (!_personalCollection.Find(filter).Any())
            {
                Debug.LogError("유효하지 않은 사용자 이름입니다.");
                _cachedUserName = string.Empty;
            }
        }
        return _cachedUserName;
    }

    public bool UpdateCharacterIndex(int characterIndex)
    {
        string name = GetCachedUserName();
        if (string.IsNullOrEmpty(name)) return false;

        var filter = Builders<Personal>.Filter.Eq(p => p.Name, name);
        var update = Builders<Personal>.Update.Set(p => p.CharacterIndex, characterIndex);

        var result = _personalCollection.UpdateOne(filter, update);
        return result.ModifiedCount > 0;
    }

    public int CheckCharacterIndex()
    {
        string name = GetCachedUserName();
        if (string.IsNullOrEmpty(name)) return -1;

        var filter = Builders<Personal>.Filter.Eq(p => p.Name, name);
        var user = _personalCollection.Find(filter).FirstOrDefault();
        return user?.CharacterIndex ?? -1;
    }

    public bool CoinUpdate(string name, int coinAdd)
    {
        var filter = Builders<Personal>.Filter.Eq(p => p.Name, name);
        var update = Builders<Personal>.Update.Inc(p => p.Coins, coinAdd); // 원자적 증가 연산

        var result = _personalCollection.UpdateOne(filter, update);

        if (result.ModifiedCount > 0)
        {
            var user = _personalCollection.Find(filter).FirstOrDefault();
            if (user != null)
            {
                PlayerPrefs.SetInt($"{name}_Coins", user.Coins);
                PlayerPrefs.Save();
                return true;
            }
        }
        return false;
    }

    public bool SpendCoins(int number)
    {
        string name = GetCachedUserName();
        if (string.IsNullOrEmpty(name)) return false;

        var filter = Builders<Personal>.Filter.Eq(p => p.Name, name);
        var update = Builders<Personal>.Update.Inc(p => p.Coins, -number); // 원자적 감소 연산

        var result = _personalCollection.UpdateOne(filter, update);
        return result.ModifiedCount > 0;
    }

    public int CheckCoins()
    {
        string name = GetCachedUserName();
        if (string.IsNullOrEmpty(name)) return -1;

        var filter = Builders<Personal>.Filter.Eq(p => p.Name, name);
        var user = _personalCollection.Find(filter).FirstOrDefault();
        return user?.Coins ?? -1;
    }
}
