using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Collections.Generic;
using UnityEngine;

public class PersonalManager : MonoBehaviour
{
    // 게시글 리스트
    private List<Personal> _personal = new List<Personal>();
    public List<Personal> Personals => _personal;
    // 콜렉션
    private IMongoCollection<Personal> _personalCollection;
    public static PersonalManager Instance { get; private set; }
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
        string connnectionString = "mongodb+srv://MetaversePro:MetaversePro@cluster0.ed1au27.mongodb.net/";
        MongoClient mongoClient = new MongoClient(connnectionString);
        IMongoDatabase db = mongoClient.GetDatabase("Logins");
        // 3. 특정 콜렉션 연결
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
    public string UserNameMach()
    {
        string nickname = PlayerPrefs.GetString("LoggedInId");

        if (string.IsNullOrEmpty(nickname))
        {
            Debug.LogError("사용자 이름을 찾을 수 없습니다.");
            return string.Empty;
        }

        var filter = Builders<Personal>.Filter.Eq("Name", nickname);
        var user = _personalCollection.Find(filter).Any();
        return user ? nickname : string.Empty;
    }
    public void UpdateCharacterIndex(int characterIndex)
    {
        string name = PlayerPrefs.GetString("LoggedInId");

        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("사용자 이름을 찾을 수 없습니다.");
            return;
        }

        var filter = Builders<Personal>.Filter.Eq(p => p.Name, name);
        var update = Builders<Personal>.Update.Set(p => p.CharacterIndex, characterIndex);

        var result = _personalCollection.UpdateOne(filter, update);
        Debug.Log("Matched Count: " + result.MatchedCount);
        Debug.Log("Modified Count: " + result.ModifiedCount);
    }
    public int CheckCharacterIndex()
    {
        string name = PlayerPrefs.GetString("LoggedInId");

        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("사용자 이름을 찾을 수 없습니다.");
            return -1;
        }

        var filter = Builders<Personal>.Filter.Eq(p => p.Name, name);
        var user = _personalCollection.Find(filter).FirstOrDefault();

        if (user != null)
        {
            Debug.Log("CharacterIndex: " + user.CharacterIndex);
            return user.CharacterIndex;
        }
        else
        { return -1; }
    }

    public void CoinUpdate(string name, int coinAdd)
    {
        var filter = Builders<Personal>.Filter.Eq(p => p.Name, name);
        var user = _personalCollection.Find(filter).FirstOrDefault();

        if (user != null)
        {
            int newCoins = user.Coins + coinAdd; 
            var update = Builders<Personal>.Update.Set(p => p.Coins, newCoins);
            var result = _personalCollection.UpdateOne(filter, update);

            // Save updated coin count to PlayerPrefs
            PlayerPrefs.SetInt($"{name}_Coins", newCoins);
            PlayerPrefs.Save();
        }
    }
    public void SpendCoins(int number)
    {
        string name = PlayerPrefs.GetString("LoggedInId");

        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("사용자 이름을 찾을 수 없습니다.");
        }
        var filter = Builders<Personal>.Filter.Eq(p => p.Name, name);
        var user = _personalCollection.Find(filter).FirstOrDefault();
        if (user != null)
        {
            int newCoins = user.Coins - number;
            var update = Builders<Personal>.Update.Set(p => p.Coins, newCoins);
            var result = _personalCollection.UpdateOne(filter, update);
        }
    }
    public int CheckCoins()
    {
        string name = PlayerPrefs.GetString("LoggedInId");

        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("사용자 이름을 찾을 수 없습니다.");
            return -1;
        }

        var filter = Builders<Personal>.Filter.Eq(p => p.Name, name);
        var user = _personalCollection.Find(filter).FirstOrDefault();

        if (user != null)
        {
            return user.Coins;
        }
        else
        { return -1; }
    }
}
