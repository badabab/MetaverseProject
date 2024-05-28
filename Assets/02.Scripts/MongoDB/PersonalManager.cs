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
<<<<<<< HEAD
<<<<<<< HEAD
=======
=======
>>>>>>> 8244951 ([이윤석] 공지사항 게시판으로 변경 중)
=======
            Personal personal = new Personal()
            {
                Name = name,
                Password = password,
            };
            _personalCollection.InsertOne(personal);
        PlayerCanvasAbility.Instance.SetNickname(name);
>>>>>>> 7ecaa3ce7e7a3cbe49fee68fa783cf66758e7592
<<<<<<< HEAD
>>>>>>> 8244951 ([이윤석] 공지사항 게시판으로 변경 중)
=======
>>>>>>> 8244951 ([이윤석] 공지사항 게시판으로 변경 중)
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
<<<<<<< HEAD
    {
        var filter = Builders<Personal>.Filter.Eq("Name", name);
        var update = Builders<Personal>.Update.Set("CharacterIndex", characterIndex);
        _personalCollection.UpdateOne(filter, update);
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

=======
    {
        var filter = Builders<Personal>.Filter.Eq("Name", name);
        var update = Builders<Personal>.Update.Set("CharacterIndex", characterIndex);
        _personalCollection.UpdateOne(filter, update);
>>>>>>> 8244951 ([이윤석] 공지사항 게시판으로 변경 중)
    }
}
