using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mslugx_form
{
    public class ReplayMemory
    {
        public int capacity { get; set; }
        public List<int> memory { get; set; }
        
        public ReplayMemory(int cap)
        {
            capacity = cap;
            //memory = mem;
        }

        public void Push(int e)
        {
            memory.Add(e);
            if(memory.Count>capacity)
            {
                memory.RemoveAt(0);
            }
        }

        //public void Sample(int batch_size)
        //{
        //    Random rnd = new Random();
        //    var temp = memory.OrderBy(item => rnd.Next()).ToList();
        //    List<int> samples = new List<int>();
        //    for (int i = 0; i < batch_size; i++)
        //    {
        //        samples.Add(temp[i]);
        //    }


        //}


    }
}
