using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Judje : MonoBehaviour
{
    private int generation = 0;
    private bool coroutineGoing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!coroutineGoing) StartCoroutine(Coroutine());

        if (Input.GetKeyDown(KeyCode.Q)) {
            List<Player> players = new List<Player>();
            players = new List<Player>(FindObjectsOfType<Player>());
            List<float> dists = new List<float>(new float[players.Count]);

            for (int i = 0; i < dists.Count; i++)
            {
                dists[i] = players[i].body_position().x;
            }
            int best = 0;
            for (int i = 0; i < dists.Count; i++){
                if (dists[i] == Max(dists)) {
                    best = i;
                    break;
                }
            }
            //int best = dists.IndexOf(Max(dists));

            Save(players[best].GetWeights());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            List<Player> players = new List<Player>();
            players = new List<Player>(FindObjectsOfType<Player>());

            

            List<float> weights_not_ready = Load();
            List<List<float>> weights = new List<List<float>>(new List<float>[3]);

            weights[0] = new List<float>(new float [3]);
            weights[1] = new List<float>(new float[3]);
            weights[2] = new List<float>(new float[3]);

            weights[0][0] = weights_not_ready[0];
            weights[0][1] = weights_not_ready[1];
            weights[0][2] = weights_not_ready[2];
            weights[1][0] = weights_not_ready[3];
            weights[1][1] = weights_not_ready[4];
            weights[1][2] = weights_not_ready[5];
            weights[2][0] = weights_not_ready[6];
            weights[2][1] = weights_not_ready[7];
            weights[2][2] = weights_not_ready[8];


            for (int i = 0; i < players.Count; i++) players[i].gameObject.BroadcastMessage("SetWeights", weights);
        }
    }   

    IEnumerator Coroutine() {
        coroutineGoing = true;
        yield return new WaitForSeconds(5f);
        List<Player> players = new List<Player>();
        players = new List<Player>(FindObjectsOfType<Player>());
        List<float> dists = new List<float>(new float[players.Count]);

        for (int i = 0;i < dists.Count;i++) {
            dists[i] = players[i].body_position().x;
            //Debug.Log(players[i].transform.position);
        }
        int best = 0;
            for (int i = 0; i < dists.Capacity; i++){
                if (dists[i] == Max(dists)) {
                    best = i;
                    break;
                }
            }

        for (int i = 0; i < dists.Capacity; i++)
        {
            players[i].ResetPosition();
            if (i != best)
            {
                List<float> weights = players[best].GetWeights();
                for (int j = 0; j < 9; j++)
                {
                    weights[j] += Random.Range(-0.1f, 0.1f);
                }

                GameObject go = players[i].gameObject;

                List<float> arr1 = new List<float>(new float[3]);
                List<float> arr2 = new List<float>(new float[3]);
                List<float> arr3 = new List<float>(new float[3]);

                arr1[0] = weights[0];
                arr1[1] = weights[1];
                arr1[2] = weights[2];
                arr2[0] = weights[3];
                arr2[1] = weights[4];
                arr2[2] = weights[5];
                arr3[0] = weights[6];
                arr3[1] = weights[7];
                arr3[2] = weights[8];

                List<List<float>> mutated = new List<List<float>>(new List<float>[3]);
                mutated[0] = arr1;
                mutated[1] = arr2;
                mutated[2] = arr3;
                go.BroadcastMessage("SetWeights", mutated);
                go.transform.position = new Vector3(0f, go.transform.position.y, 0f);
                players[i].GetBody().transform.position = go.transform.position;

            }
            else { GameObject.Find("Main Camera").transform.position = new Vector3(0f, players[best].gameObject.transform.position.y + 5f, -10f); }


            players[i].ResetPosition();
        }
Debug.Log(best);

        coroutineGoing = false;
        generation++;
        //Debug.Log(generation);
    }

    float Max(List<float> list) {
        float ans = float.MaxValue;
        foreach (float f in list) {
            if (f < ans) ans = f;
        }
        return ans;
    }

    void Save(List<float> weights) {
        BinaryFormatter form = new BinaryFormatter();
        string path = Application.persistentDataPath + "weights.data";
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        form.Serialize(stream, weights);

        stream.Close();
    }

    List<float> Load() {
        string path = Application.persistentDataPath + "weights.data";
        if (File.Exists(path))
        {
            BinaryFormatter form = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            List<float> ans = form.Deserialize(stream) as List<float>;

            stream.Close();

            return ans;
        }
        else {
            List<float> ans = new List<float>(new float [9]);
            for (int i = 0; i < 9; i++) {
                ans[i] = Random.Range(-1f, 1f);
            }
            return ans;
        }
    }
}
