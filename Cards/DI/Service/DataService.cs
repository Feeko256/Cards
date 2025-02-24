using Cards.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.DI.Service
{
    public class DataService : IDataService
    {
        public ObservableCollection<CollectionModel> Collection { get; set; }
        public ObservableCollection<CardModel> Cards { get; set; }
        public CollectionModel SelectedCollection { get; set; }
        public DataService()
        {
            Collection = new ObservableCollection<CollectionModel>();
            Cards = new ObservableCollection<CardModel>();
        }
    }
}
