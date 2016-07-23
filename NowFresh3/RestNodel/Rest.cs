using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Bson.Serialization.Attributes;
namespace RestModel {  
    public class Rest
    {  
        [BsonElement("_id")]
    public int StudentID {
        get;
        set;
    }
    public string RollNo {
        get;
        set;
    }
    public string Name {
        get;
        set;
    }
    public string Class {
        get;
        set;
    }
}  
}  