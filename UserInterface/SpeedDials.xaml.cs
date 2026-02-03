using System.Drawing;
using System.Windows;
using SwyxSharp.Common;
using Brushes = System.Windows.Media.Brushes;

namespace SwyxSharp.UserInterface
{
    /// <summary>
    /// Interaktionslogik für SpeedDials.xaml
    /// </summary>
    public partial class SpeedDials
    {
        public class Card
        {
            public string Name { get; set; }
            public string Status { get; set; }
            public string Picture { get; set; }
            public SwyxEnums.SpeedDialState State { get; set; }
        }

        public List<Card>? Cards;

        public SpeedDials()
        {
            InitializeComponent();
            this.DataContext = this;

            InitCards();

            // Subscribe to state change events
            SwyxBridge.SwyxClient?.UserStateChanged += (s) =>
            {
                // ignore 's'
                Dispatcher.Invoke(InitCards);
            };
        }

        private void InitCards()
        {
            var data = SwyxBridge.SwyxClient!.GetSpeedDials();
            Cards = [];
            foreach (var item in data)
            {
                Cards.Add(new Card
                {
                    Name = item.Name,
                    Status = item.State,
                    Picture = item.Picture,
                    State = item.SpeedDialState
                });
            }

            CardsControl.ItemsSource = Cards;
        }

        private void DebugLoad(object sender, RoutedEventArgs e)
        {
            InitCards();
        }
    }
}
