using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Cards.ViewModels;

namespace Cards.Models
{
    public class CardModel : BaseViewModel
    {
        private string mainText;
        private string secondText;
        private Brush brush = Brushes.LightGray;
        private bool _isFlipped = false;
        private string currentText;


        [JsonIgnore]
        public Brush Brush
        {
            get => brush;
            set { brush = value; OnPropertyChanged(); }
        }
        [JsonIgnore]
        public bool IsFlipped
        {
            get => _isFlipped;
            set { _isFlipped = value; OnPropertyChanged(); }
        }
        [JsonIgnore]
        public string CurrentText
        {
            get { return currentText; }
            set { currentText = value; OnPropertyChanged(); }
        }
        public string MainText
        {
            get { return mainText; }
            set { mainText = value; SetStartText();  OnPropertyChanged(); }
        }
        public string SecondText
        {
            get { return secondText; }
            set { secondText = value; SetSecondText(); OnPropertyChanged(); }
        }
        public void ChangeText()
        {
            if (IsFlipped)
            {
                CurrentText = MainText;
                Brush = Brushes.LightGray;
                IsFlipped = false;
            }
            else if (!IsFlipped)
            {
                CurrentText = SecondText;
                Brush = Brushes.LightCoral;
                IsFlipped = true;
            }
        }
        private void SetStartText()
        {
            if(!IsFlipped)
                CurrentText = MainText;
        }
        private void SetSecondText()
        {
            if (IsFlipped)
                CurrentText = SecondText;
        }
    }
}
