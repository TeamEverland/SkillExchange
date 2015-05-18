namespace SkillExchange.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Data.Data;
    using SkillExchange.Models;

    public class BaseController : Controller
    {
        private ISkillExchangeData data;

        protected BaseController(ISkillExchangeData data)
        {
            this.data = data;
        }

        protected ISkillExchangeData Data
        {
            get
            {
                return this.data;
            }
        }

        public User UserProfile { get; private set; }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var username = requestContext.HttpContext.User.Identity.Name;
                var user = this.data.Users.All().FirstOrDefault(u => u.UserName == username);
                this.UserProfile = user;
            }

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}