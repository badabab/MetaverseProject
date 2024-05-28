using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeManager : MonoBehaviour
{
    private IMongoCollection<Notice> _noticeCollection;
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
        string connectionString = "mongodb+srv://MetaversePro:MetaversePro@cluster0.ed1au27.mongodb.net/";
        MongoClient mongoClient = new MongoClient(connectionString);
        IMongoDatabase db = mongoClient.GetDatabase("Logins");
        _noticeCollection = db.GetCollection<Notice>("Notices");
    }

    public List<Notice> GetAllNotices()
    {
        return _noticeCollection.Find(Builders<Notice>.Filter.Empty).ToList();
    }
}

public class Notice
{
    public ObjectId Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}

