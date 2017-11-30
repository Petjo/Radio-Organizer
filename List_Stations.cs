using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;

namespace Organizer
{
    public class List_Stations : BindableBase
    {

        private ObservableCollection<Organizer_MainView> stations_data_List;
        private ObservableCollection<Get_Stream_Information> stream_data_list;
        private Organizer_MainView selected_Station;
        private Organizer_MainView standard_station;


        public ObservableCollection<Organizer_MainView> Stations_data_List
        {
            get { return stations_data_List; }
            set
            {
                stations_data_List = value; OnPropertyChanged("Stations_data_List");
            }
        }
        public ObservableCollection<Get_Stream_Information> Stream_data_list
        {
            get { return stream_data_list; }
            set
            {
                stream_data_list = value; OnPropertyChanged("Stream_data_list");
            }
        }

        public Organizer_MainView Standard_station
        {
            get { return standard_station; }
            set
            {
                if (standard_station == value)
                    return;
                standard_station = value;

                OnPropertyChanged("Standard_station");
            }
        }
        public Organizer_MainView Selected_Station
        {
            get { return selected_Station; }
            set
            {
                if (selected_Station == value)
                    return;
                selected_Station = value;

                OnPropertyChanged("Selected_Station");
            }
        }

        ImageSource _ImageSource;

        public ImageSource ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; OnPropertyChanged("ImageSource"); }
        }

        //Declaration
        private string _XML_Document_Name;
        private XmlDocument _XML_Document;
        private XDocument _X_Doc;

        public string XML_Document_Name
        {
            get { return _XML_Document_Name; }
            set { _XML_Document_Name = value; OnPropertyChanged("XML_Document_Name"); }
        }
        public XmlDocument XML_Document
        {
            get { return _XML_Document; }
            set
            {
                _XML_Document = value; OnPropertyChanged("XML_Document");
            }
        }
        public XDocument XDoc
        {
            get { return _X_Doc; }
            set { _X_Doc = value; OnPropertyChanged("XDoc"); }
        }

        private bool if_valid;
        private string request_answer;
        private bool end_get_stream_data;

        public bool If_valid
        {
            get { return if_valid; }
            set { if_valid = value; OnPropertyChanged("If_valid"); }
        }
        public string Request_answer
        {
            get { return request_answer; }
            set { request_answer = value; OnPropertyChanged("Request_answer"); }
        }
        public bool End_get_stream_data
        {
            get { return end_get_stream_data; }
            set { end_get_stream_data = value; OnPropertyChanged("End_get_stream_data"); }
        }

    }
}
