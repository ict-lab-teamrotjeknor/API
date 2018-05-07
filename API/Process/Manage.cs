﻿using System;
using System.Net.Mime;
using API.Models.Data;
using API.Models.Data.Query;
using API.Process.Model;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace API.Process
{
    public class Manage
    {
        private DbManage _dbManage;
        private JsonEditor _jsonEditor;

        public Manage(ApplicationDbContext dbContext)
        {
            _dbManage = new DbManage(dbContext);
            _jsonEditor = new JsonEditor();
        } 
        
        public JObject FindAllClassrooms()
        {
            var classrooms = _dbManage.GetAllClassrooms();
            return _jsonEditor.MakeClassrooms(classrooms);
        }

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
                    return _jsonEditor.GetError("Room doesn't exists");
                }
            }
            else
            {
                return _jsonEditor.GetError("Pi already exists");
            }
        }

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
    }
}