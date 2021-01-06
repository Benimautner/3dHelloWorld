using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEditor;
using UnityEngine;

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Web.UI;
using MiniJSON;

public class MissionManager : MonoBehaviour
{
    public GameObject arrow;
    public GameObject player;
    public GameObject head;
    
    private Vector3 _target;

    private void Start()
    {
        _target = new Vector3(10, 10, 10);
        
        LoadJson();

    }

    private void Update()
    {
        Vector3 dir = (_target) - (player.transform.position);

        float x = Vector2.SignedAngle(Vector3ToVector2Horizontal(dir), Vector3ToVector2Horizontal(head.transform.forward));
        
        arrow.transform.eulerAngles = new Vector3(0, 0, -x + 180);
    }

    Vector2 Vector3ToVector2Horizontal(Vector3 vin)
    {
        return new Vector2(vin.x, vin.z);
    }

    public void LoadJson()
    {
        
        print(Application.streamingAssetsPath);

        if (File.Exists(Application.streamingAssetsPath + "/missions.save")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenRead(Application.streamingAssetsPath + "/missions.save"); 
            GameInfo a = JsonUtility.FromJson<GameInfo>(file.ToString());
            print(a);
        }
        else {
            
            //FileStream fs = File.Create(Application.streamingAssetsPath + "/missions.save");
            GameInfo gameInfo = new GameInfo();
            gameInfo.missions = new Dictionary<string, Tuple<float, float>>();
            gameInfo.missions.Add("first", new Tuple<float, float>(564, 5));
            gameInfo.missions.Add("second", new Tuple<float, float>(88652, 5563));
            gameInfo.missions.Add("third", new Tuple<float, float>(1, 1));
            gameInfo.missions.Add("fourth", new Tuple<float, float>(56774, 860));


            
            string json = Json.Serialize(gameInfo.missions);
            print("json: " + json);
            var newGameInfo = Json.Deserialize(json) as Dictionary<string, object>;
            foreach (var obj in newGameInfo) {
                print("key: " + obj.Key + " Tuple: " + ParseStringToTuple(obj.Value.ToString()));
            }

        }

    }

    private Tuple<float, float> ParseStringToTuple(string inString)
    {
        var charList = new List<char>(inString.ToCharArray());
        int start = charList.FindIndex(u => u == "(".ToCharArray()[0]);
        int endFirst = charList.FindIndex(u => u == ",".ToCharArray()[0]);
        int endLast = charList.FindIndex(u => u == ")".ToCharArray()[0]);
        
        string val1 = inString.Substring(start+1,endFirst-start-1);
        string val2 = inString.Substring(endFirst+1, endLast-endFirst-1);
        
        print(val1);
        
        return new Tuple<float, float>(int.Parse(val1), int.Parse(val2));
    }

    public class GameInfo
    {
        public struct Mission
        {
            public string name;
            public float x;
            public float y;

            public Mission(string name, float x, float y)
            {
                this.name = name;
                this.x = x;
                this.y = y;
            }

        }
        public Dictionary<string, Tuple<float, float>> missions;

        /*
        public string ConvertToString()
        {
            var output = "";
            output += "{ \n";
            output += "\"missions\": [ \n";
            int ctr = 0;
            foreach (var coord in missions) {
                output += "{ \n";
                output += "\"name\": \"" + coord.Key + "\",\n";
                output += "\"0\": \"" + coord.Value + "\", \n";
                output += "\"1\": \"" + coord.Value + "\" \n";
                output += (ctr == missions.Count - 1) ? "}\n" : "},\n";
                ctr++;
            }

            output += "] \n";
            output += "} \n";
            
            output = JsonUtility.ToJson(missions);
            return output;
        }

        public static GameInfo ConvertFromString(string input)
        {
            var newCoords = JsonUtility.FromJson<Dictionary<string, float>>(input);
            foreach (var coord in newCoords) {
                print("num: "+ coord.Key + " Val: " + coord.Value.ToString());
            }
            return new GameInfo();
        }
        */
    }
}
