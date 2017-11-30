using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer
{

    //test class
    class ReadData_Station_from_XML
    {
        Organizer_MainView Organizer_MainView;

        public ReadData_Station_from_XML(Organizer_MainView Data_View)
        {
            Organizer_MainView = Data_View;
        }

        public void Get_Data()
        {
            Get_station_name_from_Id();
            Get_link_from_Id();
            Get_info_by_Id();
            Get_Standard_by_Id();
        }

        public void Get_station_name_from_Id()
        {
            Organizer_MainView.Station_Name = Organizer_MainView.XDoc.Descendants("station")                                       //find out standard radio station in options.xml
                                        .Where(e => (string)e.Attribute("id") == Organizer_MainView.Id)
                                            .Select(e => (string)e.Element("station_name"))
                                            .Single();
        }

        public void Get_link_from_Id()
        {
            Organizer_MainView.Url = Organizer_MainView.XDoc.Descendants("station")
                                        .Where(a => (string)a.Attribute("id") == Organizer_MainView.Id)                  //select link for standard radio station
                                            .Select(a => (string)a.Element("link"))
                                            .Single();
        }


        public void Get_info_by_Id()
        {
            Organizer_MainView.Comment = Organizer_MainView.XDoc.Descendants("station")
                                        .Where(e => (string)e.Attribute("id") == Organizer_MainView.Id)      //select comment for standard radio station
                                            .Select(e => (string)e.Element("info"))
                                            .Single();
        }

        public void Get_Standard_by_Id()
        {
            Organizer_MainView.Standard = Organizer_MainView.XDoc.Descendants("station")                        //if set standard
                                        .Where(e => (string)e.Attribute("id") == Organizer_MainView.Id)     
                                            .Select(e => (string)e.Attribute("standard"))
                                            .Single();
        }
    }
}
