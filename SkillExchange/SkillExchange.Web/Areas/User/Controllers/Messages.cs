﻿namespace SkillExchange.Web.Areas.User.Controllers
{
    using System.Web.Mvc;
    using Data.Data;
    using Web.Controllers;

    public class Messages : BaseController
    {
        public Messages(ISkillExchangeData data) : base(data)
        {  
        }

        // GET: User/Messages/Index
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }
    }
}