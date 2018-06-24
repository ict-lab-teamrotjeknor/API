using System.Collections.Generic;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process.Model;

namespace UnitTest
{
    public class DbManageFake : IDbManage
    {
        public List<Classroom> GetAllClassrooms()
        {
            List<Classroom> classrooms = new List<Classroom>();
            var makeClassroom = new Classroom();
            makeClassroom.Id = "10";
            makeClassroom.Name = "H.4.103";
            classrooms.Add(makeClassroom);
            return classrooms;
        }

        public bool SaveRoom()
        {
            var doNothing = string.Empty;
            return true;
        }

        public string GetRoomId(string roomName)
        {
            return "13";
        }

        public PI GetPi(NewPI searchPi)
        {
            if (searchPi.ClassroomName.Equals("H.4.103"))
            {
                return new PI();
            }

            return null;
        }

        public bool SavePi(NewPI newPi, string roomId)
        {
            return true;
        }

        public List<User> GetUsers()
        {
            var Users = new List<User>();
            var newUser = new User();
            newUser.Id = "13";
            newUser.Email = "test@test.nl";
            Users.Add(newUser);
            return Users;
        }

        public bool DeleteUser(Account deleteAccount)
        {
            return true;
        }

        public bool SetNotifications(NotificationMessage notificationMessage, NotificationUser makeNotification, string username)
        {
            throw new System.NotImplementedException();
        }

        public Notifications GetNotifications(string userId)
        {
            throw new System.NotImplementedException();
        }

        public bool SetReadNotificatie(string notificatieId)
        {
            throw new System.NotImplementedException();
        }
    }
}