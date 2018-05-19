using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyLog.Models
{
    public class Object
    {
        private Int32 id;
        private String name;
        private String date;

        public Object(Int32 id, String name, String date)
        {
            this.id = id;
            this.name = name;
            this.date = date;
        }

        public int Id
        {
            set => id = value;
            get => id;
        }

        public String Name
        {
            set => name = value;
            get => name;
        }
        public String Date
        {
            set => date = value;
            get => date;
        }

    }
}