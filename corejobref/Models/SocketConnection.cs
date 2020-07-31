using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace corejobref.Models
{
    public class SocketConnection
    {
        public static string data_Retreived = "";
        public List<DriverList> driverloc = new List<DriverList>();

        //Booking b;

        public static Socket socket;
        public List<Booking> bookinglist = new List<Booking>();
        public string socket_connect()
        {
            try
            {
                //changes 2

                string url = new WebClient().DownloadString();
                string ip = "http://" + url + "";

                //string url = new WebClient().DownloadString("");
                //string ip = "http://" + url + "";

                socket = IO.Socket(ip);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "successful";
        }

       // string data_fetched = "";
        public Booking GetData(string jobref)
        {
            try
            {
                // changes here 3
                Dictionary<string, string> dic_list = new Dictionary<string, string>();
                dic_list.Add("jobref", jobref);
                dic_list.Add("bookedby", "website");
                JObject filter = JObject.FromObject(dic_list);
                JArray data = new JArray();
                data.Add("Booking");
                data.Add(filter);

                //if (socket == null)
                //{
                //    socket_connect();
                //}

                socket.Emit("getdata", new AckImpl((abc) =>
                {
                    try
                    {
                        JObject fetchdata = (JObject)(abc);

                        //JArray fetch = (JArray)JsonConvert.DeserializeObject(fetchdata.ToString());

                        for (int i = 0; i < fetchdata.Count; i++)
                        {
                            Booking.sharedInstance = fetchdata.ToObject<Booking>();
                        }
                    }
                    catch (Exception)
                    {

                    }
                  
                    //  var fetch = (JObject)JsonConvert.DeserializeObject(data_fetched);

                    //b = JsonConvert.DeserializeObject<Booking>(data_Retreived);
                    //for (int i = 0; i < data_Retreived.Length; i++)
                    //{
                    //    b = data_fetched[i].ToObject<Booking>();
                    //    dic_fare.Add(farelist.vehsymbol, farelist);
                    //}

                }), data);
                Thread.Sleep(2000);
            }
            catch (Exception)
            {
            
            }

            // bookinglist.Add(b);

            return Booking.sharedInstance;
        }


        public List<DriverList> Getdriverslist()
        {
            try
            {
                string[] details = Booking.sharedInstance.drvrcallsign.Split('@');
                string callsign = details[0];
                string off = details[1];

                Dictionary<string, string> dic_list = new Dictionary<string, string>();
                dic_list.Add("office_id", off);
                dic_list.Add("callsign", callsign);

                JObject filter = JObject.FromObject(dic_list);
                JArray data = new JArray();
                data.Add("Driverloc");
                data.Add(filter);
                try
                {
                    socket.Emit("getdata", new AckImpl((abc) =>
                    {
                        try
                        {
                            JObject fetchdata = (JObject)(abc);

                            // driverloc.Add((DriverList)fetchdata);
                            // driverloc.Add((DriverList)fetchdata);
                            driverloc.Add(fetchdata.ToObject<DriverList>());
                        }
                        catch (Exception)
                        {

                        }

                    }), data);
                    Thread.Sleep(2000);
                }
                catch (Exception)
                {

                }

            }
            catch (Exception)
            {

            }
            return driverloc;

        }

    }
}
