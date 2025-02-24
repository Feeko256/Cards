using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using System.IO;
using System.Windows.Media;
using Cards.Models;
using System.Windows.Data;
using Cards.Resources;
using Cards.DI.Service;
using Cards.DI.Loader;

namespace Cards.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly IDataLoader _dataLoader;
        public ObservableCollection<CardModel> Cards => _dataService.Cards;
        public ObservableCollection<CollectionModel> Collection => _dataService.Collection;

        private RelayCommand flipCommand;
        private bool isClicked = false;
        private CollectionModel selectedPath;
        private string cardPackName;

        public MainPageViewModel(IDataService dataService, IDataLoader dataLoader)
        {
            _dataService = dataService;
            _dataLoader = dataLoader;
            LoadCardsPacks();
        }
        public string CardPackName
        {
            get => cardPackName;
            set
            {
                cardPackName = value;
                OnPropertyChanged();
            }
        }
        public CollectionModel SelectedPath
        {
            get => _dataService.SelectedCollection;
            set
            {
               // selectedPath = value;
                _dataService.SelectedCollection = value;
                OnPropertyChanged();
                LoadCards();
            }
        }
        private void LoadCardsPacks()
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Packs");
            string[] jsonFiles = Directory.GetFiles(folderPath, "*.json", SearchOption.AllDirectories);

            foreach (var a in jsonFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(a);

                if (!_dataService.Collection.Any(i => i.Name == fileName))
                {
                    _dataService.Collection.Add(new CollectionModel { Name = fileName, Path = a });
                }
            }
        }
        private async void LoadCards()
        {
            _dataService.Cards.Clear();
            try
            {
                var cards = await _dataLoader.ReadCardsAsync(SelectedPath);
                if (cards != null)
                {
                    foreach (var card in cards)
                    {
                        _dataService.Cards.Add(card);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error loading cards: {ex.Message}");
            }
        }

        public RelayCommand FlipCommand
        {
            get
            {
                return flipCommand ??
                  (flipCommand = new RelayCommand(obj =>
                  {

                      if (obj is CardModel card)
                      {
                          card.ChangeText();
                      }
                  }));
            }
        }
    }
}
