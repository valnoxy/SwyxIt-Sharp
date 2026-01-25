using System.Windows;
using SwyxSharp.Common;

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
            public string Picutre { get; set; }
        }

        public static List<Card>? Cards;
        public List<Card>? CardList => Cards;

        public SpeedDials()
        {
            InitializeComponent();
            this.DataContext = this;

            InitCards();
        }

        private void InitCards()
        {
            var data = SwyxBridge._swyxClient.GetSpeedDials();
            Cards = new List<Card>();
            foreach (var item in data)
            {
                Cards.Add(new Card
                {
                    Name = item.Name,
                    Status = "item.Status",
                    Picutre = item.Picture
                });
            }
        }

        private void DebugLoad(object sender, RoutedEventArgs e)
        {
            InitCards();
        }
    }
}
