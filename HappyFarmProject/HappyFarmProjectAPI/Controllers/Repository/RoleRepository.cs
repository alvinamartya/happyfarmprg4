using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class RoleRepository
    {
        /// <summary>
        /// Get role with paging
        /// </summary>
        /// <returns></returns>
        public List<Role> GetRoles()
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get employees
                var roles = db.Roles.ToList();

                // return employees
                return roles;
            }
        }
    }
}