using Cards.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.DI.Service
{
    public interface IDataService
    {
        ObservableCollection<CollectionModel> Collection { get; set; }
        ObservableCollection<CardModel> Cards { get; set; }
        CollectionModel SelectedCollection { get; set; }
    }
}
