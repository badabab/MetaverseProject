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
<<<<<<< HEAD
        Personal personal = new Personal()
        {
            Name = name,
            Password = password,
            CharacterIndex = characterIndex
        };
        _personalCollection.InsertOne(personal);
=======
            Personal personal = new Personal()
            {
                Name = name,
                Password = password,
            };
            _personalCollection.InsertOne(personal);
        PlayerCanvasAbility.Instance.SetNickname(name);
>>>>>>> 7ecaa3ce7e7a3cbe49fee68fa783cf66758e7592
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
    public void UpdateCharacterIndex(string name, int characterIndex)
    {
        var filter = Builders<Personal>.Filter.Eq("Name", name);
        var update = Builders<Personal>.Update.Set("CharacterIndex", characterIndex);
        _personalCollection.UpdateOne(filter, update);
    }
}
