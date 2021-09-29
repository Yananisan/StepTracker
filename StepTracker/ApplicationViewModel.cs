using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using System.Text.RegularExpressions;

namespace StepTracker
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private ViewInfoCard selectedCard;

        public ObservableRangeCollection<InfoCard> InfoCards { get; set; }
        public ObservableCollection<ViewInfoCard> ViewInfoCards { get; set; }
        public ViewInfoCard SelectedCard
        {
            get { return selectedCard; }
            set
            {
                selectedCard = value;
                OnPropertyChanged("SelectedCard");
            }
        }
        public ApplicationViewModel()
        {
            Regex regex = new Regex(@"\d{1,3}");
            InfoCards = new ObservableRangeCollection<InfoCard>();
            foreach (string fileName in Directory.GetFiles("../../DataSets", "*.json"))
            {
                Match match = regex.Match(fileName);
                int day = int.Parse(match.Value);
                using (StreamReader r = new StreamReader(fileName))
                {
                    string json = r.ReadToEnd();
                    ObservableCollection<InfoCard> ro = JsonConvert.DeserializeObject<ObservableCollection<InfoCard>>(json);
                    ro = new ObservableCollection<InfoCard>(ro.Select(d => { d.Day = day; return d; }));
                    InfoCards.AddRange(ro);
                }
            }


            List<string> User = InfoCards.Select(x => x.User).Distinct().ToList();

            ViewInfoCards = new ObservableCollection<ViewInfoCard>();
            foreach (var user in User)
            {
                List<InfoCard> userInfo = InfoCards.Where(u => u.User == user).OrderBy(d => d.Day).ToList();
                List<int> Day = userInfo.Select(i => i.Day).ToList();
                List<int> Rank = userInfo.Select(i => i.Rank).ToList();
                List<int> Steps = userInfo.Select(i => i.Steps).ToList();
                List<string> Status = userInfo.Select(i => i.Status).ToList();
                double AvgSteps = Math.Round(userInfo.Select(i => i.Steps).Average(), 2);
                int Best = userInfo.Select(i => i.Steps).Max();
                int Worst = userInfo.Select(i => i.Steps).Min();
                string Difference = (Best - AvgSteps) / Best * 100 >= 20 ? "More" : (AvgSteps - Worst) / Worst * 100 > 20 ? "Less" : "Average";

                ViewInfoCard viewInfoCard = new ViewInfoCard()
                {
                    Days = Day,
                    Rank = Rank,
                    User = user,
                    Steps = Steps.Zip(Day, (days, steps) => new { days, steps }).ToDictionary(item => item.steps, item => item.days),
                    Status = Status,
                    AvgSteps = AvgSteps,
                    Best = Best,
                    Worst = Worst,
                    Difference = Difference
                };

                ViewInfoCards.Add(viewInfoCard);
            }

        }

        public void ExportData (ViewInfoCard viewinfocard)
        {
            string filename = "../../UserInfo/" + viewinfocard.User + "_Info.json";
            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, viewinfocard);
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
