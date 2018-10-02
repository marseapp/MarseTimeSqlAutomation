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
            ttmList.Clear();
            ttdmList.Clear();

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

                var token = intakeArray[i].SelectToken("timetable");

                if (token is JArray)
                {
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
                }
                else if (token is JObject)
                {
                    JObject timetableObject = (JObject)intakeArray[i].SelectToken("timetable");

                    TimeTableDetailModel ttdm = new TimeTableDetailModel();
                    ttdm.IntakeId = i + 1;
                    ttdm.Date = (string)timetableObject.SelectToken("date");
                    ttdm.Time = (string)timetableObject.SelectToken("time");
                    ttdm.Location = (string)timetableObject.SelectToken("location");
                    ttdm.Classroom = (string)timetableObject.SelectToken("classroom");
                    ttdm.Module = (string)timetableObject.SelectToken("module");
                    ttdm.Lecturer = (string)timetableObject.SelectToken("lecturer");
                    ttdmList.Add(ttdm);
                }
                ttmList.Add(ttm);

            }
        }
    }
}
