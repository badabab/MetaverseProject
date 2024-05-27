/*using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

public class MongoDB : MonoBehaviour
{
    void Start()
    {
        string connnectionString = "mongodb+srv://MetaversePro:MetaversePro@cluster0.ed1au27.mongodb.net/";
        MongoClient mongoClient = new MongoClient(connnectionString);
        List<BsonDocument> dbList = mongoClient.ListDatabases().ToList();
        IMongoDatabase sampleDB = mongoClient.GetDatabase("Logins");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/