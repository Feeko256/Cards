using Cards.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.DI.Loader
{
    public interface IDataLoader
    {
        Task<ObservableCollection<CardModel>> ReadCardsAsync(CollectionModel selectedPath);
    }
}
