using corejobref.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace corejobref.Controllers
{
    public class HomeController : Controller
    {
        SocketConnection sc = new SocketConnection();
        List<DriverList> list1 = new List<DriverList>();


        public IActionResult Index()
        {
            try
            {
                sc.socket_connect();
            }
            catch (Exception)
            {

            }

            return View();
        }

        // public static string jobref;

        [HttpPost]
        public IActionResult Index([FromBody] Jobrefmodel job)
        {
            // double Originlat;
            // double Originlng;

            var resulta = (dynamic)null;
            // Booking b
            Booking.sharedInstance = null;

            try
            {
                Booking.sharedInstance = sc.GetData(job.jobref);
                resulta = new { companyname = Booking.sharedInstance.logc[0] ,olat = Booking.sharedInstance.fromtovia[0].lat, olon = Booking.sharedInstance.fromtovia[0].lon, date = Booking.sharedInstance.date, time = Booking.sharedInstance.time, custname = Booking.sharedInstance.custname, from = Booking.sharedInstance.from, to = Booking.sharedInstance.to, fare = Booking.sharedInstance.fare, oldfare = Booking.sharedInstance.oldfare, comment = Booking.sharedInstance.comment, bookedby = Booking.sharedInstance.bookedby, jstate = Booking.sharedInstance.jstate, cstate = Booking.sharedInstance.cstate, dstate = Booking.sharedInstance.dstate, flag = Booking.sharedInstance.flag };
            }
            catch (Exception)
            {

            }

            return Json(resulta);
         }

        public IActionResult Map()
        {
            return View();
        }


        [HttpPost]
        public JsonResult Map(string str)
        {
            double let = 0;
            double lng = 0;
            try
            {
                // changes here 1 and map
                if (Booking.sharedInstance.bookedby == "website" && Booking.sharedInstance.jstate == "allocated" && Booking.sharedInstance.cstate == "despatched" && Booking.sharedInstance.dstate == "Accepted" && Booking.sharedInstance.flag == 1)
                {

                    // sc.socket_connect();
                    list1 = sc.Getdriverslist();
                    foreach (var item in list1)
                    {

                        HttpContext.Session.SetString("callsign", item.callsign);
                        HttpContext.Session.SetString("lat", item.lat);
                        let = Convert.ToDouble(item.lat);
                        HttpContext.Session.SetString("lng", item.lng);
                        lng = Convert.ToDouble(item.lng);
                        HttpContext.Session.SetString("speed", item.speed);
                    }
                    return Json(new { let = let, lng = lng });
                }
                else
                {
                    return Json(new { let = "", lng = "" });
                }

            }
            catch (Exception)
            {

            }

            // DriverList
            return Json(new { let = "", lng = "" });

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
