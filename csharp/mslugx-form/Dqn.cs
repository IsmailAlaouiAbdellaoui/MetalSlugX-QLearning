using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslugx_form
{
    public class Dqn
    {
        public int InputSize { get; set; }
        public int NumberActions { get; set; }
        public double Gamma { get; set; }
        public List<double> RewardWindow { get; set; }
        public ReplayMemory Memory { get; set; }
        //public NetWork model { get; set; }
        public string Optimizer { get; set; }


        public Dqn(int inp_size,int nb_actions, double gamma)
        {
            InputSize = inp_size;
            NumberActions = nb_actions;
            Gamma = gamma;
            Memory = new ReplayMemory(100000);
            //model = new Network(InputSize,NumberActions);


        }
    }
}
