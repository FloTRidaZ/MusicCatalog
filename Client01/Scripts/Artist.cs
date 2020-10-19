using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Client01.Scripts
{
    public class Artist 
    {
        public int Id { get; }
        public string Name { get; }
        public string Cover { get; }
        public string TextData { get; }

        public Artist(int id, string name, string cover, string textData)
        {
            Id = id;
            Name = name;
            Cover = cover;
            TextData = textData;
            
        }

        public async void demo()
        {
           
        }

       
    }
}
