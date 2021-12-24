using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetworkZero
{
    private List<float> weights = new List<float>();//synaptic weights

    //constructor
    public NeuralNetworkZero(List<float> weights)
    {
        this.weights = weights;
    }

    // normalizing function
    public float sigmoid(float x) {
        return 1f / (1f + Mathf.Exp(-x));
    }

    // main function
    public float Main(List<float> values) {
        float ans = 0f;
        for (int i = 0; i < weights.Count; i++) {
            ans += weights[i] * values[i];
        }

        return sigmoid(ans);
    }


    // get and set synaptic weights
    public List<float> GetWeights() {
        return weights;
    }

    public void SetWeights(List<float> weights) {
        this.weights = weights;
    }
}
