using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Organizer
{
    public partial class MainWindow : Window
    {
        Organizer_MainView Organizer_MainView;
        List_Stations List_Stations;
        Streaming_http Streaming_http;
        private bool clicked = false;
        public int count_stations = 0;

        public MainWindow()
        {
            InitializeComponent();

            Organizer_MainView = new Organizer_MainView();
            List_Stations = new List_Stations();

            this.DataContext = List_Stations;

            Initialize_Data();
        }


        public void create_Stations_data_List()
        {
            ObservableCollection<Organizer_MainView> li = new ObservableCollection<Organizer_MainView>();

            XmlDocument xdoc = new XmlDocument();
            List_Stations.XML_Document_Name = "Stations_Stream_http.xml";
            xdoc.Load("Stations_Stream_http.xml");

            foreach (XmlNode station in xdoc.SelectNodes("/Stations_Table/*"))
            {
                count_stations++;

                li.Add(new Organizer_MainView
                {
                    Station_Name = station["station_name"].InnerText,
                    Station_homepage = station["station_homepage"].InnerText,
                    Url = station["link"].InnerText,
                    Comment = station["comment"].InnerText,
                    Genre = station["genre"].InnerText,
                    Id = station.Attributes["id"].Value,
                    Standard = station.Attributes["standard"].Value,
                    Number_of_stations = count_stations,
                    Close_stream = true
                });
            }

            List_Stations.Stations_data_List = li;
        }

        public void Initialize_Data()
        {
            create_Stations_data_List();

            for (int i = 0; i < List_Stations.Stations_data_List.Count; i++)
            {
                if (List_Stations.Stations_data_List[i].Standard.ToString() == "true")
                {
                    Organizer_MainView = List_Stations.Stations_data_List[i];
                    List_Stations.Standard_station = Organizer_MainView;
                }
            }

            ObservableCollection<Get_Stream_Information> list_data = new ObservableCollection<Get_Stream_Information>();

            List_Stations.End_get_stream_data = false;

            foreach (Organizer_MainView omv in List_Stations.Stations_data_List)
            {
                list_data.Add(new Get_Stream_Information(omv, List_Stations, 0));
            }

            List_Stations.Stream_data_list = list_data;
            List_Stations.ImageSource = Parse_Imagepath_to_ImageSource("/Resources/Images/play.png");
        }

        private void btn_play_Click(object sender, RoutedEventArgs e)
        {
            bool clicking = true;

            if (clicked != clicking)
            {
                clicked = true;
                Organizer_MainView.Close_stream = false;
                List_Stations.ImageSource = Parse_Imagepath_to_ImageSource("/Resources/Images/stop2.png");


                Streaming_http = new Streaming_http(Organizer_MainView);
                Streaming_http.Run();
            }
            else
            {
                clicked = false;
                Organizer_MainView.Close_stream = true;
                Organizer_MainView.State = "Last played: ";
                List_Stations.ImageSource = Parse_Imagepath_to_ImageSource("/Resources/Images/play.png");
            }
        }

        private ImageSource Parse_Imagepath_to_ImageSource(string path)
        {
            ImageSource ImageSource;

            Uri ImageUri = new Uri(path, UriKind.RelativeOrAbsolute);
            ImageSource = new BitmapImage(ImageUri);

            return ImageSource;
        }

        private void LV_stations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Organizer_MainView.Close_stream == false)
            {
                //binding_controls();
            }
            if (Organizer_MainView.Close_stream == true)
            {
                binding_controls();
            }
        }

        private void LV_stations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //object obj = ((GridView)sender).Content as object;

            if (LV_stations.SelectedItem == null)
            {
                return;
            }
            if(LV_stations.SelectedItem == Organizer_MainView)
            {
                return;
            }
            else
            {


                if (Organizer_MainView.Close_stream != false)
                {
                    binding_controls();

                    clicked = true;
                    Organizer_MainView.Close_stream = false;
                    List_Stations.ImageSource = Parse_Imagepath_to_ImageSource("/Resources/Images/stop2.png");

                    Streaming_http = new Streaming_http(Organizer_MainView);
                    Streaming_http.Run();
                }
                else
                {
                    Organizer_MainView.Close_stream = true;

                    Organizer_MainView = (Organizer_MainView)LV_stations.SelectedItem;
                    List_Stations.Selected_Station = Organizer_MainView;

                    binding_controls();

                    clicked = true;
                    Organizer_MainView.Close_stream = false;
                    List_Stations.ImageSource = Parse_Imagepath_to_ImageSource("/Resources/Images/stop2.png");

                    Streaming_http = new Streaming_http(Organizer_MainView);
                    Streaming_http.Run();
                }
            }
        }

        private void binding_controls()
        {
            Binding binding_new_station = new Binding();
            binding_new_station.Path = new PropertyPath("Station_Name");
            binding_new_station.Source = List_Stations.Selected_Station;
            BindingOperations.SetBinding(lbl_show_actual_station, ContentProperty, binding_new_station);


            Binding binding_new_state = new Binding();
            binding_new_state.Path = new PropertyPath("State");
            binding_new_state.Source = List_Stations.Selected_Station;
            BindingOperations.SetBinding(lbl_show_actual_stream_state, ContentProperty, binding_new_state);

            Binding binding_new_stream = new Binding();
            binding_new_stream.Path = new PropertyPath("Actual_stream_play_info");
            binding_new_stream.Source = List_Stations.Selected_Station;
            BindingOperations.SetBinding(lbl_show_actual_stream_play_info, ContentProperty, binding_new_stream);
        }

        private void MItem_add_station_Click(object sender, RoutedEventArgs e)
        {
            Window_save_station ws = new Window_save_station(this, List_Stations);
            ws.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Environment.Exit(0);
        }
    }
}
