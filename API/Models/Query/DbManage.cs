using System;
using System.Collections.Generic;
using System.Linq;
using API.Process.Model;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;


namespace API.Models.Data.Query
{
    //Database class for the manage all components 
    public class DbManage : IDbManage
    {
        private ApplicationDbContext _dbContext;
        private ILogger _logger;
        
        public DbManage(ApplicationDbContext newDbContext, ILogger logger)
        {
            _dbContext = newDbContext;
            _logger = logger;
        }

        public List<Classroom> GetAllClassrooms()
        {
            var classrooms = _dbContext.Classrooms.ToList();
            return classrooms;
        }

        public bool SaveRoom()
        {
            return true;
        }

        public string GetRoomId(string roomName)
        {
            var room = _dbContext.Classrooms
                .SingleOrDefault(c => c.Name.Equals(roomName));
            if (room != null) return room.Id;
            return string.Empty;
        }

        public PI GetPi(NewPI searchPi)
        {
            var pi = _dbContext.PI
                .SingleOrDefault(p => p.MacAdress.Equals(searchPi.MacAdress));
            return pi;
        }

        public bool SavePi(NewPI newPi, string roomId)
        {
            var room = _dbContext.Classrooms
                .SingleOrDefault(c => c.Id.Equals(roomId));
            
            
            var pi = new PI();
            pi.Id = Guid.NewGuid().ToString();
            pi.MacAdress = newPi.MacAdress;

            room.PiID = pi.Id;

            _dbContext.Add(pi);
            _dbContext.Update(room);
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                    
            }

            return true;
        }

        public List<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }

        public bool DeleteUser(Account deleteAccount)
        {
            var user = _dbContext.Users
                .SingleOrDefault(u => u.Email.Equals(deleteAccount.Email));

            user.Delete = true;

            _dbContext.Update(user);
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return true;
        }

        public bool SetNotifications(NotificationMessage notificationMessage, NotificationUser makeNotification,
            string username)
        {
            var user = _dbContext.Users
                .SingleOrDefault(u => u.Email.Equals(username));

            if (user == null) return false;
            makeNotification.UserId = user.Id;

            try
            {
                _dbContext.Add(notificationMessage);
                _dbContext.Add(makeNotification);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return false;
        }

        public Notifications GetNotifications(string userId)
        {
            var notifications = _dbContext.NotificationUser
                .Where(n => n.UserId.Equals(userId))
                .Join(_dbContext.Notification,
                    n => n.NotificationId,
                    nu => nu.Id,
                    (n, nu) => new {NotificationMessage = nu, NotificationUser = n}).ToList();

            var allNotifications = new Notifications();
            foreach (var currentNotification in notifications)
            {
                var niceNotification = new Notification();
                niceNotification.ID = currentNotification.NotificationMessage.Id;
                niceNotification.Message = currentNotification.NotificationMessage.Message;
                niceNotification.New = currentNotification.NotificationUser.New;
                allNotifications.Messages.Add(niceNotification);
            }

            return allNotifications;
        }

        public bool SetReadNotificatie(string notificatieId)
        {
            var notification = _dbContext.NotificationUser
                .Single(n => n.NotificationId.Equals(notificatieId));

            if (notification != null)
            {
                notification.New = false;

                _dbContext.Update(notification);
                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}