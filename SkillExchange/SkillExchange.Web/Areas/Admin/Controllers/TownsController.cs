namespace SkillExchange.Web.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using Data.Data;
    using Web.Controllers;

    public class TownsController : BaseController
    {
        public TownsController(ISkillExchangeData data)
            :base(data)
        {
        }

        //
        // GET: Admin/Towns/Index
        public ActionResult Index()
        {
            return this.View();
        }
    }
}