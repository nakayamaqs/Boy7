using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boy8.DAL;
using Boy8.Models;
using System.Threading.Tasks;

namespace Boy8.BLL
{
    public class UserLogic
    {
        /// <summary>
        /// Connects baby with parents.
        /// </summary>
        /// <param name="boy7DB"></param>
        /// <param name="userID"></param>
        /// <param name="baby"></param>
        /// <returns></returns>
        public static async Task<int> ConnectsBabyWithParent(Baby7DbContext boy7DB, string userID, Baby baby)
        {
            var theuser = boy7DB.Users.Single(u => u.Id == userID);
            theuser.Babies.Add(baby);
            var saveResult = await boy7DB.SaveChangesAsync();
            return saveResult;
        }
    }
}