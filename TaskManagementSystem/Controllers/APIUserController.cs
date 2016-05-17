﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace TaskManagementSystem.Controllers
{
    public class APIUserController : ApiController
    {
        private Data.TMSdbmlDataContext db = new Data.TMSdbmlDataContext();

        // ===========
        // Get Item
        // =========== 

        [Route("api/user/search/{id}")]
        public List<Models.MstUser> GetItem(String id)
        {
            var isLocked = true;

            var ID = Convert.ToInt32(id);
            var user = from d in db.mstUsers
                       where d.StaffId == ID
                       select new Models.MstUser
                       {
                           Id = d.StaffId,
                           StaffId = d.StaffId,
                           Username = d.Username,
                           Password = d.Password,
                           Designation = d.Designation,
                           IsLocked = isLocked
                       };

            return user.ToList();
        }

        // ===========
        // LIST Item
        // =========== 
        [Route("api/user/list")]
        public List<Models.MstUser> Get()
        {
            var isLocked = true;

            var user = from d in db.mstUsers
                          select new Models.MstUser
                          {
                              Id = d.StaffId,
                              StaffId = d.StaffId,
                              StaffName = d.mstStaff.StaffName,
                              Password = d.Password,
                              Username = d.Username,
                              IsLocked = isLocked
                          };

            return user.ToList();
        }

        // ===========
        // ADD Item
        // ===========
        [Route("api/user/add")]
        public HttpResponseMessage Post(Models.MstUser item)
        {
            try
            {
                var isLocked = true;
                var identityUserId = User.Identity.GetUserId();
                var date = DateTime.Now;

                Data.mstUser newItem = new Data.mstUser();

                newItem.Username = item.Username != null ? item.Username : "00000";
                newItem.Password = item.Password != null ? item.Password : "00000";
                //newItem.StaffId = item.StaffId != null ? item.StaffId : "00000";
                newItem.IsLocked = isLocked != null ? isLocked : false;

                //ALLOW NULL

                db.mstUsers.InsertOnSubmit(newItem);
                db.SubmitChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }


    }
}
