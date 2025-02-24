using Cards.DI.Loader;
using Cards.DI.Service;
using Cards.Models;
using Cards.Resources;
using Cards.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Cards.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;

        public ObservableCollection<CollectionModel> Collection => _dataService.Collection;
        public ObservableCollection<CardModel> Cards => _dataService.Cards;

        public RelayCommand mainPageCommand;
        public RelayCommand editPackPageCommand;

        private MainPageViewModel mainPageViewModel;
        private EditPackViewModel editPackViewModel;
        private SettingsPageViewModel settingsPageViewModel;


        private RelayCommand settingsPageCommand;

        public ContentControl currentPage;
        public MainWindowViewModel(MainPageViewModel cardsVM, EditPackViewModel editPackViewModel, SettingsPageViewModel settingsPageViewModel, IDataService dataService)
        {
            mainPageViewModel = cardsVM;
            this.editPackViewModel = editPackViewModel;
            this.settingsPageViewModel = settingsPageViewModel;
            CurrentPage = new MainPage { DataContext = cardsVM };

            _dataService = dataService;
        }

        public ContentControl CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                OnPropertyChanged();
            }
        }

       
        public RelayCommand MainPageCommand
        {
            get
            {
                return mainPageCommand ??
                  (mainPageCommand = new RelayCommand(obj =>
                  {;
                      CurrentPage = new MainPage { DataContext = mainPageViewModel };
                  }));
            }
        }  
        public RelayCommand EditPackPageCommand
        {
            get
            {
                return editPackPageCommand ??
                  (editPackPageCommand = new RelayCommand(obj =>
                  {
                      CurrentPage = new EditPackPage { DataContext = editPackViewModel };
                  }));
            }
        }
        public RelayCommand SettingsPageCommand
        {
            get
            {
                return settingsPageCommand ??
                  (settingsPageCommand = new RelayCommand(obj =>
                  {
                      CurrentPage = new SettingsPage { DataContext = settingsPageViewModel };
                  }));
            }
        }
    }
}
