using System;
using System.Collections.Generic;
using API.Process.Model;

namespace API.Models.Data.Query
{
    public interface IDbManage
    {
        List<Classroom> GetAllClassrooms();
        bool SaveRoom();
        string GetRoomId(string roomName);
        PI GetPi(NewPI searchPi);
        bool SavePi(NewPI newPi, string roomId);
        List<User> GetUsers();
        bool DeleteUser(Account deleteAccount);
        bool SetNotifications(NotificationMessage notificationMessage, NotificationUser makeNotification,
            string username);
        Notifications GetNotifications(string userId);
        bool SetReadNotificatie(string notificatieId);
    }
}