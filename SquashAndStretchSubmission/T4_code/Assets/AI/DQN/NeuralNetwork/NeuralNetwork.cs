

public class NeuralNetwork
{
    int layers;
    int inputs;
    int outputs;
    System.Collections.Generic.List<NeuralLayer> network;

    public NeuralNetwork(int initialInputs, int _outputs, int[] layerNeurons, int _layers)
    {
        layers = _layers;
        inputs = initialInputs;
        outputs = _outputs;
        network.Add(new NeuralLayer(initialInputs, layerNeurons[0]));
        for (int i = 1; i < layers - 1; i++)
            network.Add(new NeuralLayer(layerNeurons[i - 1], layerNeurons[i]));
        network.Add(new NeuralLayer(layerNeurons[layers - 1], outputs));
    }

    public float[] CalculateNetwork(float[] input)
    {
        float[] output = new float[outputs];
        float[] linkData = network[1].CalculateLayer(input);
        for (int i = 0; i < inputs; i++)
            linkData = network[i].CalculateLayer(linkData);
        output = linkData;

        return output;
    }
}
