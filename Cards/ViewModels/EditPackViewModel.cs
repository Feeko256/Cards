using Cards.DI.Loader;
using Cards.DI.Service;
using Cards.Models;
using Cards.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Cards.ViewModels
{
    public class EditPackViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly IDataLoader _dataLoader;
        public ObservableCollection<CollectionModel> Collection => _dataService.Collection;
        public ObservableCollection<CardModel> Cards => _dataService.Cards;

        public EditPackViewModel(IDataService dataService, IDataLoader dataLoader)
        {
            _dataService = dataService;
            _dataLoader = dataLoader;
            LoadCardsPacks();
            EditCollectionContainer = 0;
        }



        private RelayCommand flipCommand;
        private RelayCommand addCardCommand;
        private RelayCommand removeCardCommand;
        private RelayCommand addCardPackCommand;
        private RelayCommand savePackInfoCommand;
        private RelayCommand removePackCommand;
        private RelayCommand refreshCollectionCommand;
        private bool isClicked = false;
        private CollectionModel selectedPath;
        private string cardPackName;
        private string renamePack;
        private int editCollectionContainer;



        public string CardPackName
        {
            get => cardPackName;
            set
            {
                cardPackName = value;
                OnPropertyChanged();
            }
        }
        public int EditCollectionContainer
        {
            get => editCollectionContainer;
            set
            {
                editCollectionContainer = value;
                OnPropertyChanged();
            }
        }

        public string RenamePack
        {
            get => renamePack;
            set
            {
                renamePack = value;
                OnPropertyChanged();
            }
        }

        public CollectionModel SelectedPath
        {
            get => _dataService.SelectedCollection;
            set
            {
                _dataService.SelectedCollection = value;
                if (SelectedPath is not null)
                    RenamePack = SelectedPath.Name;
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

                if (!Collection.Any(i => i.Name == fileName))
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
                        Cards.Add(card);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error loading cards: {ex.Message}");
            }
        }        
        public RelayCommand AddCardCommand
        {
            get
            {
                return addCardCommand ??
                  (addCardCommand = new RelayCommand(obj =>
                  {
                      if (SelectedPath != null)
                      {
                          Cards.Add(new CardModel { MainText = "Front", SecondText = "Back" });
                          string json = JsonSerializer.Serialize(Cards);
                          File.WriteAllText(SelectedPath.Path, json);
                      }
                  }));
            }
        }
        public RelayCommand RemoveCardCommand
        {
            get
            {
                return removeCardCommand ??
                  (removeCardCommand = new RelayCommand(obj =>
                  {
                      
                      if (obj is CardModel card)
                      {
                          _dataService.Cards.Remove(card);
                          string json = JsonSerializer.Serialize(Cards);
                          File.WriteAllText(SelectedPath.Path, json);
                      }
                  }));
            }
        }
        public RelayCommand AddCardPackCommand
        {
            get
            {
                return addCardPackCommand ??
                  (addCardPackCommand = new RelayCommand(obj =>
                  {
                      string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Packs/{CardPackName}.json");

                      if (string.IsNullOrWhiteSpace(CardPackName))
                      {
                          MessageBox.Show("Введите имя коллекции.");
                          return;
                      }
                      if (Collection.Any(i => i.Name == CardPackName))
                      {
                          MessageBox.Show("коллекция с таким именем уже существует.");
                          return;
                      }
                      File.WriteAllText(folderPath, "");
                      LoadCardsPacks();
                      CardPackName = "";
                      SelectedPath = Collection.Last();
                  }));
            }
        }
        public RelayCommand SavePackInfoCommand
        {
            get
            {
                return savePackInfoCommand ??
                  (savePackInfoCommand = new RelayCommand(obj =>
                  {
                      if (SelectedPath is not null)
                      {

                          string oldPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Packs/{SelectedPath.Name}.json");
                          string jsonData = File.ReadAllText(oldPath);
                          File.Delete(oldPath);
                          string newPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Packs/{RenamePack}.json");
                          File.WriteAllText(newPath, jsonData);
                          SelectedPath.Name = RenamePack;
                          SelectedPath.Path = newPath;
                          string json = JsonSerializer.Serialize(Cards);
                          File.WriteAllText(SelectedPath.Path, json);
                          LoadCardsPacks();
                      }
                  }));
            }
        }
        public RelayCommand RemovePackCommand
        {
            get
            {
                return removePackCommand ??
                  (removePackCommand = new RelayCommand(obj =>
                  {
                      if (SelectedPath is not null)
                      {
                          string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Packs\\{SelectedPath.Name}.json");

                          CollectionModel collection = Collection.FirstOrDefault(i => i.Path == path);
                          if (collection != null)
                          {
                              _dataService.Collection.Remove(collection);
                          }
                          File.Delete(path);
                          LoadCardsPacks();
                          if (Collection.Count > 0)
                              SelectedPath = Collection.Last();
                      }
                  }));
            }
        }
        public RelayCommand RefreshCollectionCommand
        {
            get
            {
                return refreshCollectionCommand ??
                  (refreshCollectionCommand = new RelayCommand(obj =>
                  {
                      if (SelectedPath is not null)
                          SelectedPath = null;
                      if (Collection.Count > 0 && (Collection != null))
                          Collection.Clear();
                      LoadCardsPacks();
                  }));
            }
        }
    }
}