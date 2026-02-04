using SwyxSharp.Common;
using System.Windows;
using System.Windows.Input;
using SwyxSharp.Common.Debugging;

namespace SwyxSharp.UserInterface
{
    /// <summary>
    /// Interaktionslogik für SpeedDials.xaml
    /// </summary>
    public partial class SpeedDials
    {
        public class Card
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public string Picture { get; set; }
            public string Number { get; set; }
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
                    Id = item.Id,
                    Name = item.Name,
                    Status = item.State,
                    Picture = item.Picture,
                    State = item.SpeedDialState,
                    Number = item.Number
                });
            }

            CardsControl.ItemsSource = Cards;
        }

        private void DebugLoad(object sender, RoutedEventArgs e)
        {
            InitCards();
        }

        private void SpeedDial_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Wpf.Ui.Controls.Card cardControl) return;
            if (cardControl.DataContext is not Card clickedCard) return;

            SwyxBridge.SwyxClient?.InitiateCall(clickedCard.Number);
        }

        private void EditSpeedDial_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Wpf.Ui.Controls.Button cardControl) return;
            if (cardControl.DataContext is not Card clickedCard) return;
            
            Logging.Log($"Editing SpeedDial {clickedCard.Id}");
            SwyxBridge.SwyxClient?.OpenDialog(SwyxEnums.DialogId.SpeedDials + (uint)clickedCard.Id + 1);
        }

        private void AddSpeedDial_Click(object sender, RoutedEventArgs e)
        {
            Logging.Log("Adding new SpeedDial");
            var currentCountCards = Cards!.Count;
            SwyxBridge.SwyxClient?.OpenDialog(SwyxEnums.DialogId.SpeedDials + (uint)currentCountCards + 1);
        }
    }
}
