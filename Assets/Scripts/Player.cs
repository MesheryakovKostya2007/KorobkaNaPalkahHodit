using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private NeuralNetworkZero leg_0; // neural network for the first leg
    private NeuralNetworkZero leg_1; // neural network for the second leg
    private NeuralNetworkZero body; // neural network for body

    //references for legs and body, i set them in unity, imagine they are not null
    public Rigidbody2D leg_0_rb;
    public Rigidbody2D leg_1_rb;
    public Rigidbody2D body_rb;

    //rotation speed, set in unity
    public float speed;

    private float bodyrot = 0f;
    // Start is called before the first frame update
    void Start()
    {
        // generate random weights for neural networks
        List<float> w_0 = new List<float>(new float[3]);
        List<float> w_1 = new List<float>(new float[3]);
        List<float> w_2 = new List<float>(new float[3]);

        

        for (int i = 0; i < 3; i++) {
            w_0[i] = Random.Range(-1f, 1f);
            w_1[i] = Random.Range(-1f, 1f);
            w_2[i] = Random.Range(-1f, 1f);
        }

        //initialize neural networks
        leg_0 = new NeuralNetworkZero(w_0);
        leg_1 = new NeuralNetworkZero(w_1);
        body = new NeuralNetworkZero(w_2);

    }

    public Vector3 body_position() {
        return body_rb.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //get rotation of body and legs;
        float leg_0_rot = leg_0_rb.rotation;
        float leg_1_rot = leg_1_rb.rotation;
        float body_rot = body_rb.rotation;

        float[] args = new float[3];
        args[0] = leg_0_rot;
        args[1] = leg_1_rot;
        args[2] = body_rot;

        //get info from neural networks
        
        float ans_0 = leg_0.Main(new List<float>(args));
        float ans_1 = leg_1.Main(new List<float>(args));
        float ans_2 = body.Main(new List<float>(args));

        // rotate stuff
        if (ans_0 > 0.5f)
        {
            leg_0_rb.MoveRotation(leg_0_rb.rotation + 13);
        }
        else {
            leg_0_rb.MoveRotation(leg_0_rb.rotation - 13);
        }

        if (ans_1 > 0.5f)
        {
            leg_1_rb.MoveRotation(leg_1_rb.rotation + 13);
        }
        else
        {
            leg_1_rb.MoveRotation(leg_1_rb.rotation - 13);
        }

        if (ans_2 > 0.5f)
        {
            bodyrot--;
            body_rb.MoveRotation(body_rb.rotation + 3);
        }
        else 
        {
            bodyrot--;
            body_rb.MoveRotation(body_rb.rotation - 3);
        }
    }

    public List<float> GetWeights() {
        List<float> ans = new List<float>();
        ans.AddRange(leg_0.GetWeights());
        ans.AddRange(leg_1.GetWeights());
        ans.AddRange(body.GetWeights());
        return ans;
    }

    public void SetWeights(List<List<float>> weights) {
        leg_0.SetWeights(weights[0]);
        leg_1.SetWeights(weights[1]);
        body.SetWeights(weights[2]);
    }

    public GameObject GetBody() {
        return body_rb.gameObject;
    }

    public void ResetPosition()
    {

        leg_1_rb.MoveRotation(0f);
        leg_0_rb.MoveRotation(0f);
        body_rb.MoveRotation(0f);
        body_rb.position = new Vector3(0f, body_rb.position.y, 0f);
    }
}
