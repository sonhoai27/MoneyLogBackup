using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyLog.Models
{
    public class MoneyLogItem
    {
        public int id;
        public string name;
        public Int32 amount;
        public string note;
        public byte type;
        public string date;
        public MoneyLogItem(int Id, string Name, Int32 Amount, string Note, byte Type, string Date)
        {
            this.id = Id;
            this.name = Name;
            this.amount = Amount;
            this.note = Note;
            this.type = Type;
            this.date = Date;
        }

        //public int Id
        //{
        //    set => id = value;
        //    get => id;
        //}
        //public double Amount
        //{
        //    set => amount = value;
        //    get => amount;
        //}
        //public string Name
        //{
        //    set => name = value;
        //    get => name;
        //}
        //public string Note
        //{
        //    set => note = value;
        //    get => note;
        //}
        //public byte Type
        //{
        //    set => type = value;
        //    get => type;
        //}
        //public string Date
        //{
        //    set => date = value;
        //    get => date;
        //}
    }
}