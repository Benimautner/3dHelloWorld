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
    private Vector2 _target;
    private int _currentMission;
    public GameObject beacon;

    public List<Vector2> missions;

    private void Start()
    {
        missions = LoadJson();
        if (missions.Count == 0) {
            gameObject.SetActive(false);
        }
        _target = missions[0];
        beacon.transform.position = new Vector3(_target.x, 0, _target.y);
    }

    private void Update()
    {
        Vector3 playerPos = player.transform.position;
        if (Vector2.Distance(_target, new Vector2(playerPos.x, playerPos.z)) < 5.0f) {
            _currentMission++;
            if (_currentMission > missions.Count - 1)
                _currentMission = 0;
            _target = missions[_currentMission];
            beacon.transform.position = new Vector3(_target.x, 0, _target.y);
        }
        Vector3 dir = (new Vector3(_target.x, 0, _target.y)) - (playerPos);

        float x = Vector2.SignedAngle(Vector3ToVector2Horizontal(dir), Vector3ToVector2Horizontal(head.transform.forward));
        
        arrow.transform.eulerAngles = new Vector3(0, 0, -x + 180);
    }

    Vector2 Vector3ToVector2Horizontal(Vector3 vin)
    {
        return new Vector2(vin.x, vin.z);
    }

    public List<Vector2> LoadJson()
    {
        List<Vector2> returnVecs = new List<Vector2>();
        
        print(Application.streamingAssetsPath);

        var filePath = Path.Combine(Application.streamingAssetsPath + "/missions.save");

        if (!File.Exists(filePath)) {
            FileStream fs = File.Create(filePath);
            StreamWriter streamWriter = new StreamWriter(fs);
            GameInfo gameInfo = new GameInfo();
            gameInfo.missions = new Dictionary<string, Tuple<float, float>>();
            gameInfo.missions.Add("first", new Tuple<float, float>(50, 50));
            gameInfo.missions.Add("second", new Tuple<float, float>(70, 50));
            gameInfo.missions.Add("third", new Tuple<float, float>(0, 0));
            gameInfo.missions.Add("fourth", new Tuple<float, float>(-30, -40));
            
            string json = Json.Serialize(gameInfo.missions);
            streamWriter.Write(json);
            streamWriter.Close();
            fs.Close();
        }
        returnVecs = SplitJsonString(filePath);
        print("count: " + returnVecs.Count);
        return returnVecs;
    }

    private List<Vector2> SplitJsonString(string filePath)
    {
        var returnVectors = new List<Vector2>();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.OpenRead(filePath);
        StreamReader streamReader = new StreamReader(file);
        var newGameInfo = Json.Deserialize(streamReader.ReadToEnd()) as Dictionary<string, object>;
        if (newGameInfo == null) return new List<Vector2>();
        foreach (var obj in newGameInfo) {
            Tuple<float, float> tuple = ParseStringToTuple(obj.Value.ToString());
            Vector2 newVec = new Vector2(tuple.Item1, tuple.Item2);
            returnVectors.Add(newVec);
            print("Tuple: " + tuple.ToString());
        }

        return returnVectors;
    }
    
    
    private Tuple<float, float> ParseStringToTuple(string inString)
    {
        var charList = new List<char>(inString.ToCharArray());
        int start = charList.FindIndex(u => u == "(".ToCharArray()[0]);
        int endFirst = charList.FindIndex(u => u == ",".ToCharArray()[0]);
        int endLast = charList.FindIndex(u => u == ")".ToCharArray()[0]);
        
        string val1 = inString.Substring(start+1,endFirst-start-1);
        string val2 = inString.Substring(endFirst+1, endLast-endFirst-1);
        
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
