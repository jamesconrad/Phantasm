using System.Collections;

public class NeuralLayer
{
    private int neurons;
    private int numIn;
    System.Collections.Generic.List<NeuralNode> layer;
    
    public NeuralLayer(int numInputs, int numNodes)
    {
        numIn = numInputs;
        layer = new System.Collections.Generic.List<NeuralNode>(numInputs);
        for (int i = 0; i < numNodes; i++)
            layer[i] = new NeuralNode(numInputs);
    }

    public void CalculateLayer()
    {

    }
}
