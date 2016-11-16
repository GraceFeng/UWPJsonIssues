using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPJsonIssues
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private string JsonOutstring;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".json");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                using (var streamIn = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    DataReader reader = new DataReader(streamIn);
                    await reader.LoadAsync((uint)streamIn.Size);
                    var jsonInstring = reader.ReadString((uint)streamIn.Size);
                    var JobjList = JsonConvert.DeserializeObject<List<JsonColor>>(jsonInstring);
                    reader.Dispose();
                    JobjList.Add(new JsonColor() { color = "pink", value = "#c0c" });
                    JsonOutstring = JsonConvert.SerializeObject(JobjList);
                }
                using (var streamOut = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    DataWriter writer = new DataWriter(streamOut);
                    writer.WriteString(JsonOutstring);
                    await writer.StoreAsync();
                    writer.DetachStream();
                    writer.Dispose();
                }
            }
            else
            {
            }
        }

        public class JsonColor
        {
            public string color { get; set; }
            public string value { get; set; }
        }
    }
}