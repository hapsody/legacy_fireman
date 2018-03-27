using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TGAME
{
    public class TPrefabMgr
    {
        static private Dictionary<string, GameObject> _cache = new Dictionary<string, GameObject>();
        
        //폴더 로딩   
        static public void Load(string subfolder, string subName)
        {
            //string loadName = subfolder + subName;
            object[] t0 = Resources.LoadAll(subfolder);
            for (int i = 0; i < t0.Length; i++)
            {
                GameObject t1 = (GameObject)(t0[i]);//temp[i] as GameObject;
                _cache[t1.name] = t1;
            }
        }

        // R1을 반환. 
        //GameObject temp = Get("R1");
        static public GameObject Get(string key)
        {
            return _cache[key];
        }

        //R1으로부터 R1Clone을 생성 및 반환. 
        //GameObject temp = Instance("R1", "R1Clone", 0, 0, 0);
        static public GameObject Instance(string key, string name, float x, float y, float z)
        {
            GameObject t0 = (GameObject)(GameObject.Instantiate(_cache[key], new Vector3(x, y, z), _cache[key].transform.rotation));
            t0.name = name;
            return t0;
        }

        //R1~R3까지 삭제 
        //Remove( "R1", "R2", "R3" ); 
        static public void Remove(params string[] arg)
        {
            for (int i = 0; i < arg.Length; i++)
            {
                string key = arg[i];
                _cache.Remove(key);
            }
        }
    }
}
