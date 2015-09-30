using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;

namespace DeadManSwitch.Service.WebApi.Host.Controllers
{
    public class UserPreferencesController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IAccountService AccountSvc;

        public UserPreferencesController(IAccountService accountService)
        {
            AccountSvc = accountService;
        }

        // GET: Preferences
        public async Task<IHttpActionResult> Get(string id)
        {
            string userName = id;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");

            try
            {
                var preferences = await AccountSvc.FindUserPreferencesAsync(userName);
                if (preferences != null)
                {
                    return Ok(preferences.ToWebApiEntity());
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // POST: Preferences
        public async Task<IHttpActionResult> Post(UserPreferencesModel userPreferences)
        {
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await AccountSvc.UpdatePreferencesAsync(userName, userPreferences.ToServiceEntity());

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

    }
}
