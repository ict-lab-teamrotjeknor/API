using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ScheduleCrawler.Models;

namespace ScheduleCrawler
{
    public class Process
    {
        private Schedule _schedule;
        private int _dayLine;
        private int _totalDays;
        private int _hourLine;
        private bool _timeReady;
        private int _whichDay;
        private int _nextField;
        private int _rightField;
        private bool _readySubjects;
        private int _sizeCurrentHour;
        
        public Schedule GetModelSchedule(string HTMLSchedule)
        {
            _schedule = new Schedule();
            _dayLine = 15;
            _totalDays = 1;
            _hourLine = 0;
            _timeReady = false;
            _whichDay = 0;
            _nextField = 0;
            _rightField = 0;
            _readySubjects = false;
            _sizeCurrentHour = 0;
            
            using (var reader = new StringReader(HTMLSchedule))
            {
                var line = string.Empty;
                var lineId = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    ChooseRightFunction(line, lineId);
                    lineId++;
                }
            }

            return _schedule;
        }

        private void ChooseRightFunction(string line, int lineId)
        {
            switch (lineId)
            {
                case 7:
                    SetClassRoomName(line);
                    return;
                case 9:
                    SetClassroomType(line);
                    return;
            }

            if (lineId == _dayLine)
            {
                AddDay(line);
            }

            if (_timeReady)
            {
                var check = line.Remove(3, 1);
                
                if (check.Equals("<b></b>"))
                {
                    _hourLine += 1;
                    _nextField = lineId + 6;
                    _whichDay = 0;
                    _readySubjects = false;
                }

                if (_whichDay < 6)
                {

                    if (CheckEmptyHour(line) && _nextField == lineId)
                    {
                        _nextField += 1;
                        _whichDay += 1;
                    }
                    else
                    {
                        if (_nextField == lineId && !_readySubjects && line.Contains("colspan"))
                        {
                            _nextField += 1;
                            _readySubjects = true;
                            _sizeCurrentHour = CheckSize(line);
                            _whichDay += 1;
                            SkipExistsDays();
                        }
                        else
                        {
                            if (_nextField == lineId && _readySubjects)
                            {
                                if (_sizeCurrentHour == 15)
                                {
                                    _rightField = 3;
                                }
                                
                                ChooseRightField(line);
                            }
                        }
                    }
                }
            }
        }

        private void SetClassRoomName(string name)
        {
            _schedule.ClassroomName = name;
        }

        private void SetClassroomType(string type)
        {
            _schedule.ClassroomType = type;
        }

        private void AddDay(string day)
        {
            _schedule.AddDay(day);

            if (_totalDays < 5)
            {
                _dayLine += 4;
                _totalDays += 1;
            }
            else
            {
                _timeReady = true;
            }
        }

        private bool CheckEmptyHour(string hour)
        {
            return hour.Equals("<td colspan=\"12\" rowspan=\"2\" align=\"center\" nowrap=\"1\"><table><tr><td></td></tr></table></td>");
        }

        private int CheckSize(string line)
        {
            string totalRows = string.Empty;
            totalRows = line[26].ToString();

            if (totalRows.Equals("1") || totalRows.Equals("3"))
            {
                char second = line[27];
                totalRows = totalRows + second.ToString();
            }

            int totalIntRows = Int32.Parse(totalRows);
            return totalIntRows / 2;
        }

        private void ChooseRightField(string line)
        {

            for (int tempHour = _hourLine ;tempHour < _sizeCurrentHour + _hourLine; tempHour++) {

                switch (_rightField)
                {
                    case 0:
                        _schedule.SetClassName(tempHour - 1, _whichDay - 1, line);
                        break;
                    case 1:
                        _schedule.SetTeacherName(tempHour - 1, _whichDay - 1, line);
                        break;
                    case 2:
                        _schedule.SetCourse(tempHour - 1, _whichDay - 1, line);
                        break;
                    case 3:
                        _schedule.SetSpecialReason(tempHour - 1, _whichDay - 1, line);
                        break;
                    default:
                        break;
                }
            }
            _nextField += 3;
            if (_rightField >= 2)
            {
                _rightField = 0;
                _readySubjects = false;
                _sizeCurrentHour = 0;
            }
            else
            {
                _rightField++;
            }
        }

        private void SkipExistsDays()
        {
            while (true)
            {
                if (_schedule.CheckHourExists(_hourLine - 1, _whichDay -1))
                {
                    _whichDay++;
                }
                else
                {
                    break;
                }
            }
        }
    }
}