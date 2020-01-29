using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketApplication.Helpers
{
    public class Variables
    {
        public Variables()
        {

        }

        public Variables(int id)
        {
            UserId = id;
        }

        public int UserId { get; set; }

        public static Variables current
        {
            get
            {
                var cur = HttpContext.Current.Session["SessionVars"] as Variables;

                if (cur == null)
                {
                    cur = new Variables();
                    HttpContext.Current.Session["SessionVars"] = cur;
                }

                return cur;
            }
            //set
            //{
            //    var cur = (Variables)value;
            //    HttpContext.Current.Session["SessionVars"] = cur;
            //}
        }
    }
}