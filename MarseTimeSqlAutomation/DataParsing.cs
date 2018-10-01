using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarseTimeSqlAutomation
{
    class DataParsing
    {
        public static List<TimeTableModel> ttmList = new List<TimeTableModel>();
        public static List<TimeTableDetailModel> ttdmList = new List<TimeTableDetailModel>();

        public void ParseData(string json)
        {
            var rootObject = JObject.Parse(json);

            var weekObject = rootObject.GetValue("weekof");
            string weekOf = (string)weekObject.SelectToken("@week");

            JArray intakeArray = (JArray)weekObject.SelectToken("intake");

            for (int i = 0; i < intakeArray.Count; i++)
            {
                TimeTableModel ttm = new TimeTableModel();
                ttm.Name = (string)intakeArray[i].SelectToken("@name");
                ttm.Week = weekOf;
                ttm.Id = i + 1;
                JArray timetableArray = (JArray)intakeArray[i].SelectToken("timetable");
                for (int j = 0; j < timetableArray.Count; j++)
                {
                    TimeTableDetailModel ttdm = new TimeTableDetailModel();
                    ttdm.IntakeId = i + 1;
                    ttdm.Date = (string)timetableArray[j].SelectToken("date");
                    ttdm.Time = (string)timetableArray[j].SelectToken("time");
                    ttdm.Location = (string)timetableArray[j].SelectToken("location");
                    ttdm.Classroom = (string)timetableArray[j].SelectToken("classroom");
                    ttdm.Module = (string)timetableArray[j].SelectToken("module");
                    ttdm.Lecturer = (string)timetableArray[j].SelectToken("lecturer");
                    ttdmList.Add(ttdm);
                }
                ttmList.Add(ttm);
            }
        }
    }
}
