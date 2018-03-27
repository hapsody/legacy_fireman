using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TGAME;

public class Loading : MonoBehaviour {
    
    string m_strPath = "Assets/Resources/";

    public void Parse()

    {
        int iY = 0;
        int iZ = 0;
        int iX = 0;

        TextAsset data = Resources.Load("stage1", typeof(TextAsset)) as TextAsset;

        StringReader sr = new StringReader(data.text);

        // 먼저 한줄을 읽는다. 
        string source = sr.ReadLine();

        string[] values;                // 쉼표로 구분된 데이터들을 저장할 배열 (values[0]이면 첫번째 데이터 )

        while (source != null)

        {
            values = source.Split(',');  // 쉼표로 구분한다. 저장시에 쉼표로 구분하여 저장하였다.

            if (values.Length == 0)

            {

                sr.Close();

                return;

            }

            for(int i = 0; i < values.Length; i++)
            {
                GameObject obj = TPrefabMgr.Instance("tile_" + values[i], "tile_" + values[i], iX + i, iY, iZ);
                obj.transform.localScale.Set(2.0f, 2.0f, 2.0f);
            }
            
            source = sr.ReadLine();    // 한줄 읽는다.
            iY++;
        }

    }

	// Use this for initialization
	void Start () {
        TPrefabMgr.Load("Prefabs", null);
        Parse();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
