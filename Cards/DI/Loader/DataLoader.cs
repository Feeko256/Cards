using Cards.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cards.DI.Loader
{
    public class DataLoader : IDataLoader
    {
        public async Task<ObservableCollection<CardModel>> ReadCardsAsync(CollectionModel selectedPath)
        {
            if (selectedPath == null)
            {
                return new ObservableCollection<CardModel>();
            }
            using (FileStream fs = new FileStream(selectedPath.Path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {

                return await JsonSerializer.DeserializeAsync<ObservableCollection<CardModel>>(fs)
                       ?? new ObservableCollection<CardModel>();
            }
        }
    }
}
