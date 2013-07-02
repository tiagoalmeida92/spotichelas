using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class PlayController : Controller
    {
        //
        // GET: /Play/

        public ActionResult Track(string id)
        {
            return PartialView((object)id);
        }

    }
}
