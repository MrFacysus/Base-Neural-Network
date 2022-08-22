 public class Synapse
        {
            public Neuron Start;
            public Neuron End;

            public double weight;

            public Synapse(Neuron start, Neuron end)
            {
                weight = r.NextDouble() * 2 - 1;
                Start = start;
                End = end;

                end.Connected.Add(this);
            }
        }

        public class Neuron
        {
            public List<Synapse> Connected;
            public double value;
            public double bias;

            public Neuron()
            {
                Connected = new List<Synapse>();
                bias = r.NextDouble();
            }

            public void Activate()
            {
                value = 0;

                foreach (Synapse synapse in Connected)
                {
                    value += synapse.Start.value * synapse.weight;
                }

                value = value + bias;
            }
        }

        public class Brain
        {
            public List<List<Neuron>> Layers = new List<List<Neuron>>();

            public Brain(int Inputs, int HiddenLayers, int Outputs)
            {
                Create(Inputs, HiddenLayers, Outputs);
            }

            public void SynapseSetup()
            {
                for (int i = 0; i < Layers.Count - 1; i++)
                {
                    for (int j = 0; j < Layers[i + 1].Count; j++)
                    {
                        for (int k = 0; k < Layers[i].Count; k++)
                        {
                            new Synapse(Layers[i][k], Layers[i + 1][j]);
                        }
                    }
                }
            }

            public void NeuronSetup(int Inputs, int HiddenLayers, int Outputs)
            {
                for (int i = 0; i < HiddenLayers + 2; i++)
                {
                    List<Neuron> Temp = new List<Neuron>();
                    if (i == 0)
                    {
                        for (int j = 0; j < Inputs; j++)
                        {
                            Temp.Add(new Neuron());
                        }
                    }
                    else if (i < HiddenLayers + 1)
                    {
                        for (int j = 0; j < Inputs - 1; j++)
                        {
                            Temp.Add(new Neuron());
                        }
                    }
                    else
                    {
                        for (int j = 0; j < Outputs; j++)
                        {
                            Temp.Add(new Neuron());
                        }
                    }
                    Layers.Add(Temp);
                }
            }

            public void Save()
            {
                string SaveWeightsLog = "";
                string SaveBiasLog = "";

                for (int i = 0; i < Layers.Count; i++)
                {
                    for (int j = 0; j < Layers[i].Count; j++)
                    {
                        SaveBiasLog += Layers[i][j].bias + " ";

                        if (i > 0)
                        {
                            foreach (Synapse Con in Layers[i][j].Connected)
                            {
                                SaveWeightsLog += Con.weight + " ";
                            }
                        }
                    }
                }

                File.WriteAllText("SaveFile.bias", SaveBiasLog.Trim());
                File.WriteAllText("SaveFile.weight", SaveBiasLog.Trim());
            }

            public void Load(List<double> WeightContent, List<double> BiasContent, int Inputs, int HiddenLayers, int Outputs)
            {
                int SYNIDX = 0;
                int NEUIDX = 0;

                Layers = new List<List<Neuron>>();

                NeuronSetup(Inputs, HiddenLayers, Outputs);
                SynapseSetup();

                for (int i = 0; i < Layers.Count; i++)
                {
                    for (int j = 0; j < Layers[i].Count; j++)
                    {
                        Layers[i][j].bias = BiasContent[NEUIDX];
                        NEUIDX++;

                        if (i > 0)
                        {
                            foreach (Synapse Con in Layers[i][j].Connected)
                            {
                                Con.weight = WeightContent[SYNIDX];
                                SYNIDX++;
                            }
                        }
                    }
                }
            }

            public void Create(int Inputs, int HiddenLayers, int Outputs)
            {
                NeuronSetup(Inputs, HiddenLayers, Outputs);

                SynapseSetup();
            }

            public int GetOutput(List<int> InputVals)
            {
                List<double> OutputVals = new List<double>();

                for (int i = 0; i < Layers.Count; i++)
                {
                    for (int j = 0; j < Layers[i].Count; j++)
                    {
                        if (i == 0)
                        {
                            Layers[i][j].value = InputVals[i];
                        }
                        else if (i < Layers.Count - 1)
                        {
                            Layers[i][j].Activate();
                        }
                        else
                        {
                            Layers[i][j].Activate();
                            return (int)Layers[i][j].value;
                        }
                    }
                }

                return 0;
            }
        }
    }
