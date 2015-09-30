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
    public class UsersController : ApiController
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IAccountService AccountSvc;

        public UsersController(IAccountService accountService)
        {
            AccountSvc = accountService;
        }

        // GET: Users/userName
        [HttpGet]
        public async Task<IHttpActionResult> Get(string id)
        {
            string userName = id;
            if (string.IsNullOrWhiteSpace(userName)) return BadRequest(nameof(userName) + " is required.");

            try
            {
                var user = await AccountSvc.FindUserAsync(userName);
                if (user == null) return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // POST: Users
        public async Task<IHttpActionResult> Post(UserRegistrationModel userRegistration)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var validationErrorMsgs = await AccountSvc.RegisterUserAsync(userRegistration.ToServiceEntity(), userRegistration.Password);
                if (validationErrorMsgs.Any()) return StatusCode((HttpStatusCode)HttpStatusCodeCustom.UnprocessableRequest);

                return Ok();    //TODO: Change to created with a route to the user
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // PUT: Users
        public async Task<IHttpActionResult> Put(UserProfileModel profile)
        {
            string userName;
            if (!CurrentAppState.IsUserAuthenticated(Request, out userName)) throw new Exception("Not authorized");
            if (profile == null) return BadRequest(nameof(profile) + " is required.");

            try
            {
                await AccountSvc.UpdateProfileAsync(userName, profile.ToServiceEntity());

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
