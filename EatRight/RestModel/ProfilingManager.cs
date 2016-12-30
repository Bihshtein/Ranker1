using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestModel
{
    public class GlobalProfilingManger
    {
        protected static GlobalProfilingManger instance = null;
        protected static object locker = new object();
        protected ProfilingManager pManager = null;

        protected GlobalProfilingManger()
        {
            pManager = new ProfilingManager("Global");
        }

        public static GlobalProfilingManger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null) instance = new GlobalProfilingManger();
                    }
                    
                }
                return instance;
            }
            
        }

        public ProfilingManager Manager
        {
            get { return pManager; }
        }
    }

    public class ProfilingManager
    {
        public string Name { get; set; }

        protected List<Tuple<string, double>> timedActions;

        public ProfilingManager()
        {
            timedActions = new List<Tuple<string, double>>();
            Name = "Default";
        }

        public ProfilingManager(string name) : this()
        {
            this.Name = name;
        }

        public void Reset()
        {
            this.timedActions.Clear();
        }

        public void End()
        {
            this.TakeTime("ending time");
        }

        public double TotalTime()
        {
            if ((timedActions == null) || (timedActions.Count < 2)) return 0;

            return timedActions.Last().Item2 - timedActions.First().Item2;
        }

        public void TakeTime(string processName)
        {
            this.timedActions.Add(new Tuple<string, double>(processName, DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond));
        }

        public override string ToString()
        {
            if ((timedActions == null) || (timedActions.Count < 1)) return string.Empty;
            var toPrint = "Profiling Manger: " + this.Name + "\n";

            toPrint += timedActions[0].Item1 + " took 0 ms\n";

            for (int i = 1; i < timedActions.Count; i++)
            {
                toPrint += string.Format("{0} took {1} ms\n", timedActions[i].Item1, timedActions[i].Item2 - timedActions[i - 1].Item2);
            }

            toPrint += string.Format("Entire Process took: {0} ms", timedActions.Last().Item2 - timedActions.First().Item2);

            return toPrint;
        }
    }
}
