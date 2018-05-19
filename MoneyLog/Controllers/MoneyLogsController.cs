using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MoneyLog.Models;
using System.Text;

namespace MoneyLog.Controllers
{
    public class MoneyLogsController : ApiController
    {
        private Models.Object week, quart, monthObj, yearObj;
        private List<Weeks> listWeeks = new List<Weeks>();
        private NotesEntities db = new NotesEntities();

        // GET: api/MoneyLogs
        public IEnumerable<MoneyLog.Models.MoneyLog> GetMoneyLogs()
        {
            return db.MoneyLogs.SqlQuery("select * from MoneyLog Order By Id DESC").ToList();
        }

        // GET: api/MoneyLogs/5
        [ResponseType(typeof(MoneyLog.Models.MoneyLog))]
        public IHttpActionResult GetMoneyLog(int id)
        {
            MoneyLog.Models.MoneyLog moneyLog = db.MoneyLogs.Find(id);
            if (moneyLog == null)
            {
                return NotFound();
            }

            return Ok(moneyLog);
        }

        [Route("api/MoneyLogs/Week/")]//custom dang filter and sort
        [HttpGet]
        public List<Weeks> GetFilter(String type) {
            IEnumerable<MoneyLog.Models.MoneyLog> MoneyLogsController = db.MoneyLogs.SqlQuery("select * from MoneyLog where Type = "+type+" Order By Date DESC");

            String tempMonth = MoneyLogsController.ElementAt(0).Date.Split('-')[1];
            int tempDay = checkWeek(int.Parse(MoneyLogsController.ElementAt(0).Date.Split('-')[2]));
            String month = null;
            int thu;

            List<Models.Object> weeks = generateWeek();

            List<MoneyLogItem> moneyLogs = new List<MoneyLogItem>(); ;
            for (int i = 0; i < MoneyLogsController.Count();i++)
            {
                thu = checkWeek(int.Parse(MoneyLogsController.ElementAt(i).Date.Split('-')[2]));
                month = MoneyLogsController.ElementAt(i).Date.Split('-')[1];
                if (i == 0)
                {
                    moneyLogs.Add(new MoneyLogItem(
                            MoneyLogsController.ElementAt(i).Id,
                            MoneyLogsController.ElementAt(i).Name,
                            MoneyLogsController.ElementAt(i).Amount,
                            MoneyLogsController.ElementAt(i).Note,
                            MoneyLogsController.ElementAt(i).Type,
                            MoneyLogsController.ElementAt(i).Date
                    ));
                    week = new Models.Object(weeks.ElementAt(tempDay - 1).Id, weeks.ElementAt(tempDay - 1).Name, MoneyLogsController.ElementAt(i).Date);
                    listWeeks.Add(new Weeks(
                        week,
                        moneyLogs
                    ));
                }
                if (tempDay != thu || !tempMonth.Equals(month))
                {
                    moneyLogs = new List<MoneyLogItem>();
                    moneyLogs.Add(new MoneyLogItem(
                           MoneyLogsController.ElementAt(i).Id,
                           MoneyLogsController.ElementAt(i).Name,
                           MoneyLogsController.ElementAt(i).Amount,
                           MoneyLogsController.ElementAt(i).Note,
                           MoneyLogsController.ElementAt(i).Type,
                           MoneyLogsController.ElementAt(i).Date
                   ));
                    week = new Models.Object(weeks.ElementAt(thu - 1).Id, weeks.ElementAt(thu - 1).Name, MoneyLogsController.ElementAt(i).Date);
                    listWeeks.Add(new Weeks(
                        week,
                        moneyLogs
                    ));
                    tempDay = thu;
                    tempMonth = month;
                }else if(tempDay == thu && tempMonth.Equals(month))
                {
                   if(i != 0)
                    {
                        moneyLogs.Add(new MoneyLogItem(
                           MoneyLogsController.ElementAt(i).Id,
                           MoneyLogsController.ElementAt(i).Name,
                           MoneyLogsController.ElementAt(i).Amount,
                           MoneyLogsController.ElementAt(i).Note,
                           MoneyLogsController.ElementAt(i).Type,
                           MoneyLogsController.ElementAt(i).Date
                        ));
                    }
                }
            }
            return listWeeks.ToList();
        }

        [Route("api/MoneyLogs/Quart/")]//custom dang filter and sort
        [HttpGet]
        public List<Weeks> GetQuart(String type)
        {
            IEnumerable<MoneyLog.Models.MoneyLog> MoneyLogsController = db.MoneyLogs.SqlQuery("select * from MoneyLog where Type = " + type + " Order By Date DESC");

            int tempYear =int.Parse(MoneyLogsController.ElementAt(0).Date.Split('-')[0]);
            int tempMonth = checkQuart(int.Parse(MoneyLogsController.ElementAt(0).Date.Split('-')[1]));
            int year, month;

            List<Models.Object> quarts = generateQuart();

            List<MoneyLogItem> moneyLogs = new List<MoneyLogItem>(); ;
            for (int i = 0; i < MoneyLogsController.Count(); i++)
            {
                month = checkQuart(int.Parse(MoneyLogsController.ElementAt(i).Date.Split('-')[1]));
                year = int.Parse(MoneyLogsController.ElementAt(i).Date.Split('-')[0]);
                if (i == 0)
                {
                    moneyLogs.Add(new MoneyLogItem(
                            MoneyLogsController.ElementAt(i).Id,
                            MoneyLogsController.ElementAt(i).Name,
                            MoneyLogsController.ElementAt(i).Amount,
                            MoneyLogsController.ElementAt(i).Note,
                            MoneyLogsController.ElementAt(i).Type,
                            MoneyLogsController.ElementAt(i).Date
                    ));
                    quart = new Models.Object(quarts.ElementAt(tempMonth - 1).Id, quarts.ElementAt(tempMonth -1).Name, MoneyLogsController.ElementAt(i).Date);
                    listWeeks.Add(new Weeks(
                        quart,
                        moneyLogs
                    ));
                }
                if (tempMonth != month || !tempYear.Equals(year))
                {
                    moneyLogs = new List<MoneyLogItem>();
                    moneyLogs.Add(new MoneyLogItem(
                           MoneyLogsController.ElementAt(i).Id,
                           MoneyLogsController.ElementAt(i).Name,
                           MoneyLogsController.ElementAt(i).Amount,
                           MoneyLogsController.ElementAt(i).Note,
                           MoneyLogsController.ElementAt(i).Type,
                           MoneyLogsController.ElementAt(i).Date
                   ));
                    quart = new Models.Object(quarts.ElementAt(month - 1).Id, quarts.ElementAt(month -1).Name, MoneyLogsController.ElementAt(i).Date);
                    listWeeks.Add(new Weeks(
                        quart,
                        moneyLogs
                    ));
                    tempMonth = month;
                    tempYear = year;
                }
                else if (tempMonth == month && tempYear.Equals(year))
                {
                    if (i != 0)
                    {
                        moneyLogs.Add(new MoneyLogItem(
                           MoneyLogsController.ElementAt(i).Id,
                           MoneyLogsController.ElementAt(i).Name,
                           MoneyLogsController.ElementAt(i).Amount,
                           MoneyLogsController.ElementAt(i).Note,
                           MoneyLogsController.ElementAt(i).Type,
                           MoneyLogsController.ElementAt(i).Date
                        ));
                    }
                }
            }
            return listWeeks.ToList();
        }
        [Route("api/MoneyLogs/Month/")]//custom dang filter and sort
        [HttpGet]
        public List<Weeks> GetMonth(String type)
        {
            IEnumerable<MoneyLog.Models.MoneyLog> MoneyLogsController = db.MoneyLogs.SqlQuery("select * from MoneyLog where Type = " + type + " Order By Date DESC");

            int tempYear = int.Parse(MoneyLogsController.ElementAt(0).Date.Split('-')[0]);
            int tempMonth = int.Parse(MoneyLogsController.ElementAt(0).Date.Split('-')[1]);
            int year, month;

            List<Models.Object> months = generateMonth();

            List<MoneyLogItem> moneyLogs = new List<MoneyLogItem>(); ;
            for (int i = 0; i < MoneyLogsController.Count(); i++)
            {
                month = int.Parse(MoneyLogsController.ElementAt(i).Date.Split('-')[1]);
                year = int.Parse(MoneyLogsController.ElementAt(i).Date.Split('-')[0]);
                if (i == 0)
                {
                    moneyLogs.Add(new MoneyLogItem(
                            MoneyLogsController.ElementAt(i).Id,
                            MoneyLogsController.ElementAt(i).Name,
                            MoneyLogsController.ElementAt(i).Amount,
                            MoneyLogsController.ElementAt(i).Note,
                            MoneyLogsController.ElementAt(i).Type,
                            MoneyLogsController.ElementAt(i).Date
                    ));
                    monthObj = new Models.Object(months.ElementAt(tempMonth - 1).Id, months.ElementAt(tempMonth - 1).Name, MoneyLogsController.ElementAt(i).Date);
                    listWeeks.Add(new Weeks(
                        monthObj,
                        moneyLogs
                    ));
                }
                if (tempMonth != month || !tempYear.Equals(year))
                {
                    moneyLogs = new List<MoneyLogItem>();
                    moneyLogs.Add(new MoneyLogItem(
                           MoneyLogsController.ElementAt(i).Id,
                           MoneyLogsController.ElementAt(i).Name,
                           MoneyLogsController.ElementAt(i).Amount,
                           MoneyLogsController.ElementAt(i).Note,
                           MoneyLogsController.ElementAt(i).Type,
                           MoneyLogsController.ElementAt(i).Date
                   ));
                    monthObj = new Models.Object(months.ElementAt(month - 1).Id, months.ElementAt(month - 1).Name, MoneyLogsController.ElementAt(i).Date);
                    listWeeks.Add(new Weeks(
                        monthObj,
                        moneyLogs
                    ));
                    tempMonth = month;
                    tempYear = year;
                }
                else if (tempMonth == month && tempYear.Equals(year))
                {
                    if (i != 0)
                    {
                        moneyLogs.Add(new MoneyLogItem(
                           MoneyLogsController.ElementAt(i).Id,
                           MoneyLogsController.ElementAt(i).Name,
                           MoneyLogsController.ElementAt(i).Amount,
                           MoneyLogsController.ElementAt(i).Note,
                           MoneyLogsController.ElementAt(i).Type,
                           MoneyLogsController.ElementAt(i).Date
                        ));
                    }
                }
            }
            return listWeeks.ToList();
        }

        [Route("api/MoneyLogs/Year/")]//custom dang filter and sort
        [HttpGet]
        public List<Weeks> GetYear(String type)
        {
            IEnumerable<MoneyLog.Models.MoneyLog> MoneyLogsController = db.MoneyLogs.SqlQuery("select * from MoneyLog where Type = " + type + " Order By Date DESC");

            int tempYear = int.Parse(MoneyLogsController.ElementAt(0).Date.Split('-')[0]);
            int year;

            List<Models.Object> months = generateMonth();

            List<MoneyLogItem> moneyLogs = new List<MoneyLogItem>(); ;
            for (int i = 0; i < MoneyLogsController.Count(); i++)
            {
                year = int.Parse(MoneyLogsController.ElementAt(i).Date.Split('-')[0]);
                if (i == 0)
                {
                    moneyLogs.Add(new MoneyLogItem(
                            MoneyLogsController.ElementAt(i).Id,
                            MoneyLogsController.ElementAt(i).Name,
                            MoneyLogsController.ElementAt(i).Amount,
                            MoneyLogsController.ElementAt(i).Note,
                            MoneyLogsController.ElementAt(i).Type,
                            MoneyLogsController.ElementAt(i).Date
                    ));
                    yearObj = new Models.Object(MoneyLogsController.ElementAt(i).Id, "Nam "+ year, "");
                    listWeeks.Add(new Weeks(
                        yearObj,
                        moneyLogs
                    ));
                }
                if (!tempYear.Equals(year))
                {
                    moneyLogs = new List<MoneyLogItem>();
                    moneyLogs.Add(new MoneyLogItem(
                           MoneyLogsController.ElementAt(i).Id,
                           MoneyLogsController.ElementAt(i).Name,
                           MoneyLogsController.ElementAt(i).Amount,
                           MoneyLogsController.ElementAt(i).Note,
                           MoneyLogsController.ElementAt(i).Type,
                           MoneyLogsController.ElementAt(i).Date
                   ));
                    yearObj = new Models.Object(MoneyLogsController.ElementAt(i).Id, "Nam " + year, "");
                    listWeeks.Add(new Weeks(
                        yearObj,
                        moneyLogs
                    ));
                    tempYear = year;
                }
                else if (tempYear.Equals(year))
                {
                    if (i != 0)
                    {
                        moneyLogs.Add(new MoneyLogItem(
                           MoneyLogsController.ElementAt(i).Id,
                           MoneyLogsController.ElementAt(i).Name,
                           MoneyLogsController.ElementAt(i).Amount,
                           MoneyLogsController.ElementAt(i).Note,
                           MoneyLogsController.ElementAt(i).Type,
                           MoneyLogsController.ElementAt(i).Date
                        ));
                    }
                }
            }
            return listWeeks.ToList();
        }
        // PUT: api/MoneyLogs/5
        [ResponseType(typeof(MoneyLog.Models.MoneyLog))]
        public IHttpActionResult PutMoneyLog(int id, [FromBody]MoneyLog.Models.MoneyLog moneyLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != moneyLog.Id && moneyLog == null)
            {
                return BadRequest();
            }

            db.Entry(moneyLog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoneyLogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok("200");
            
        }

        // POST: api/MoneyLogs
        [ResponseType(typeof(MoneyLog.Models.MoneyLog))]
        public IHttpActionResult PostMoneyLog([FromBody]MoneyLog.Models.MoneyLog moneyLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DateTime userLocalNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, System.TimeZoneInfo.Local);
            DateTime dateTime = DateTime.UtcNow.Date;
            moneyLog.Date = userLocalNow.ToString("yyyy-MM-dd");
            db.MoneyLogs.Add(moneyLog);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = moneyLog.Id }, moneyLog);
        }

        // DELETE: api/MoneyLogs/5
        [ResponseType(typeof(MoneyLog.Models.MoneyLog))]
        public IHttpActionResult DeleteMoneyLog(int id)
        {
            MoneyLog.Models.MoneyLog moneyLog = db.MoneyLogs.Find(id);
            if (moneyLog == null)
            {
                return NotFound();
            }

            db.MoneyLogs.Remove(moneyLog);
            db.SaveChanges();

            return Ok("200");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MoneyLogExists(int id)
        {
            return db.MoneyLogs.Count(e => e.Id == id) > 0;
        }

        private List<Models.Object> generateWeek()
        {
            List<Models.Object> weeks = new List<Models.Object>();
            for (int i = 1; i < 5; i++)
            {
                weeks.Add(new Models.Object(
                        i,
                        "Tuan: " + i,
                        ""
                   ));
            }

            return weeks;
        }

        private List<Models.Object> generateQuart()
        {
            List<Models.Object> weeks = new List<Models.Object>();
            for (int i = 1; i < 5; i++)
            {
                weeks.Add(new Models.Object(
                        i,
                        "Quy: " + i,
                        ""
                   ));
            }

            return weeks;
        }

        private List<Models.Object> generateMonth()
        {
            List<Models.Object> weeks = new List<Models.Object>();
            for (int i = 1; i < 13; i++)
            {
                weeks.Add(new Models.Object(
                        i,
                        "Thang: " + i,
                        ""
                   ));
            }

            return weeks;
        }

        private int checkWeek(int day)
        {
           if(day > 0 && day < 8)
            {
                return 1;
            }else if(day > 7 && day < 15)
            {
                return 2;
            }else if(day > 14 && day < 22)
            {
                return 3;
            }else if(day > 21 && day < 32)
            {
                return 4;
            }
            return 0;
        }

        private int checkQuart(int month)
        {
            if (month > 0 && month < 4)
            {
                return 1;
            }
            else if (month > 3 && month < 7)
            {
                return 2;
            }
            else if (month > 6 && month < 10)
            {
                return 3;
            }
            else if (month > 9 && month < 13)
            {
                return 4;
            }
            return 0;
        }
    }
}