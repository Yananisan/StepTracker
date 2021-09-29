using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StepTracker
{
    public class ViewInfoCard : INotifyPropertyChanged
    {       
        private string user;        
        private double avgsteps;
        private int best;
        private int worst;
        private string difference;

        public List<int> Days { get; set; }
        public List<int> Rank { get; set; }
        public Dictionary<int, int> Steps { get; set; }
        public List<string> Status { get; set; }

        public string User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }
        public double AvgSteps
        {
            get { return avgsteps; }
            set
            {
                avgsteps = value;
                OnPropertyChanged("AvgSteps");
            }
        }
        public int Best
        {
            get { return best; }
            set
            {
                best = value;
                OnPropertyChanged("Best");
            }
        }

        public int Worst
        {
            get { return worst; }
            set
            {
                worst = value;
                OnPropertyChanged("Worst");
            }
        }

        [DefaultValue("Average")]
        public string Difference
        {
            get { return difference; }
            set
            {
                difference = value;
                OnPropertyChanged("Difference");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
