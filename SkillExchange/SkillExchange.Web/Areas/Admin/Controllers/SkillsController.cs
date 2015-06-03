namespace SkillExchange.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using Data.Data;
    using Web.Controllers;

    public class SkillsController : BaseController
    {
        public SkillsController(ISkillExchangeData data)
            :base(data)
        { 
        }

        //
        // GET: Admin/Skills/Index
        public ActionResult Index()
        {
            return this.View();
        }
    }
}