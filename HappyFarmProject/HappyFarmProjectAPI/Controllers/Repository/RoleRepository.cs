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
        public List<Role> GetRoles(string role)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get employees
                var roles = db.Roles.ToList();

                // filter by role
                if (role == "Super Admin")
                    roles = roles.Where(x => x.Name != "Super Admin" && x.Name != "Customer").ToList();
                else if(role == "Manager")
                    roles = roles.Where(x => x.Name != "Super Admin" && x.Name != "Manager" && x.Name != "Customer").ToList();

                // return employees
                return roles;
            }
        }
    }
}