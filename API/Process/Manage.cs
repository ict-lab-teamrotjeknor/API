using System;
using System.Net.Mime;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace API.Process
{
    //This class handles every componates
    public class Manage
    {
        private IDbManage _dbManage;
        private IJsonEditor _jsonEditor;
        private ILogger _logger;
        private bool Deployment;
        private UserManager<User> _userManager;

        public Manage(IDbManage newDbManage, IJsonEditor newJsonEditor, ILogger logger, UserManager<User> newManager, bool newDeployemnt = true)
        {
            _dbManage = newDbManage;
            _jsonEditor = newJsonEditor;
            _logger = logger;
            Deployment = newDeployemnt;
            _userManager = newManager;
        } 
        
        //Get all classrooms
        public JObject FindAllClassrooms()
        {
            var classrooms = _dbManage.GetAllClassrooms();
            return _jsonEditor.MakeClassrooms(classrooms);
        }
        
        //Register a PI
        public JObject AddPi(NewPI newPi)
        {
            var pi = _dbManage.GetPi(newPi);

            if (pi == null)
            {
                var roomId = _dbManage.GetRoomId(newPi.ClassroomName);
                if (roomId != string.Empty)
                {
                    var done = _dbManage.SavePi(newPi, roomId);
                    return _jsonEditor.GetSucced();
                }
                else
                {
                    if (Deployment) _logger.LogInformation("Room doesn;t exists");
                    return _jsonEditor.GetError("Room doesn't exists");
                }
            }
            else
            {
                if (Deployment) _logger.LogInformation("Pi already exists");
                return _jsonEditor.GetError("Pi already exists");
            }
        }

        //Get Users
        public JObject GetUsers()
        {
            var users = _dbManage.GetUsers();

            var allUsers = new UserView();
            foreach (var currentUser in users)
            {
                var getUser = new UserModel();
                getUser.Id = currentUser.Id;
                getUser.Email = currentUser.Email;
                allUsers.Users.Add(getUser);
            }

            return _jsonEditor.SerilizeJObject(allUsers);
        }

        //Send Notifications
        public JObject SendNotification(Notification newNotification)
        {
            var notificationMessage = new NotificationMessage();
            notificationMessage.Id = Guid.NewGuid().ToString();
            notificationMessage.Message = newNotification.Message;
            var makeNotification = new NotificationUser();
            makeNotification.Id = Guid.NewGuid().ToString();
            makeNotification.New = true;
            makeNotification.NotificationId = notificationMessage.Id;
            var succeed = _dbManage.SetNotifications(notificationMessage, makeNotification, newNotification.UserName);
            return succeed ? _jsonEditor.GetSucced() : _jsonEditor.GetError("User doesn't exists");
        }
        
        //Send Notifications group
        public JObject SendGroupNotification(Notification newNotification)
        {
            var usersOfRole = _userManager.GetUsersInRoleAsync(newNotification.Role);

            foreach (var currentUser in usersOfRole.Result)
            {
                newNotification.UserName = currentUser.Email;
                SendNotification(newNotification);
            }
            
            return _jsonEditor.GetSucced();
        }

        //Get all Notifications
        public JObject GetNotifications(string userId)
        {
           var notifications =  _dbManage.GetNotifications(userId);
            return _jsonEditor.SerilizeJObject(notifications);
        }

        public JObject SetRead(string notificatieId)
        {
            var succeed = _dbManage.SetReadNotificatie(notificatieId);
            return succeed ? _jsonEditor.GetSucced() : _jsonEditor.GetError("Notification doesn't exists");
        }
    }
}