using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public enum ArticleType
{
    Normal,
    Notice,
}
// 게시글을 나타내는 데이터 클래스
[Serializable]
[BsonIgnoreExtraElements]
public class Article 
{
    [BsonId]
    public ObjectId Id;             // 유일한 주민번호: Id, _id, id (시간 + 기기ID + 프로세스ID + count)
    public ArticleType ArticleType; // 일반글? 공지사항글이냐?
    [BsonElement("Name")]
    public string Name;             // 글쓴이
    public string Content;          // 글 내용
}

[Serializable]
public class ArticleData
{
    public List<Article> Data;

    public ArticleData(List<Article> data)
    {
        Data = data;
    }
} 

