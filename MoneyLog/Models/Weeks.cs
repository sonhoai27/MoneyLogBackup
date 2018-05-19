using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyLog.Models
{
    public class Weeks
    {
        private Object weekObj;
        private List<MoneyLogItem> moneyLogList;

        public Weeks()
        {

        }
        public Weeks(Object weekObj, List<MoneyLogItem> moneyLogs)
        {
            this.weekObj = weekObj;
            this.moneyLogList = moneyLogs;
        }

        public Object WeekObj
        {
            set => weekObj = value;
            get => weekObj;
        }
        public List<MoneyLogItem> MoneyLogList
        {
            set => moneyLogList = value;
            get => moneyLogList;
        }
    }
}