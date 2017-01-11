using System.Collections.Generic;

public class NeuralNode
{

    public float bias;
    List<float> weights;
    List<float> inputs;
    private int numin;

    private float output;

    public NeuralNode(int numinputs)
    {
        numin = numinputs;
        bias = 0;
        weights = new List<float>(numinputs);
        inputs  = new List<float>(numinputs);
        for (int i = 0; i < numinputs; i++)
            weights[i] = 1.0f;
    }
    
    public void ApplyInput(float[] input)
    {
        for (int i = 0; i < numin; i++)
            inputs[i] = input[i];
    }

    public float CalculateOutput()
    {
        //summation of x*w + b
        float summation = 0;
        for (int i = 0; i < numin; i++)
            summation += inputs[i] * weights[i];
        float perceptron = summation + bias;
        //sigmoid(z) = 1/(1+e^-z)
        float sigmoid = 1 / (1 + (float)System.Math.Exp(-perceptron));
        return sigmoid;
    }

    public float[] GetWeights()
    {
        return weights.ToArray();
    }

    public void SetWeights(float[] w)
    {
        for (int i = 0; i < numin; i++)
            weights[i] = w[i];
    }

    public void AdjustWeights(float[] w)
    {
        for (int i = 0; i < numin; i++)
            weights[i] += w[i];
    }

    public int GetNumInputs()
    {
        return numin;
    }

    public float GetOutput()
    {
        return output;
    }
}
