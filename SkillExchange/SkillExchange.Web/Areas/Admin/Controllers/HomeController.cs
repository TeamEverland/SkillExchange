﻿namespace SkillExchange.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using Data.Data;
    using Web.Controllers;

    [Authorize(Roles = "Administrator")]
    public class HomeController : BaseController
    {
        public HomeController(ISkillExchangeData data)
            : base(data)
        {
        }

        //
        // GET: Admin/Home/Index
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }
    }
}