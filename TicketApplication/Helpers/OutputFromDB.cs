using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace TicketApplication.Helpers
{
    public static class OutputFromDB
    {
        public static bool Bool(object valueObj)
        {
            return !Convert.IsDBNull(valueObj) ? Convert.ToBoolean(valueObj) : false;
        }

        public static object ComboBox(object valueObj)
        {
            return !Convert.IsDBNull(valueObj) ? valueObj : null;
        }

        public static string StringEmpty(object valueObj)
        {
            return !Convert.IsDBNull(valueObj) ? valueObj.ToString() : string.Empty;
        }

        internal static void BindGridView(ASPxGridView gv, DataTable dt)
        {
            gv.DataSource = dt;
            gv.DataBind();
        }

        internal static void BindRadioList(ASPxRadioButtonList rbl, DataTable dt)
        {
            rbl.DataSource = dt;
            rbl.DataBind();
        }

        internal static void BindRepeater(Repeater rpt, DataTable dt)
        {
            rpt.DataSource = dt;
            rpt.DataBind();
        }

        internal static DateTime Date(object valueObj)
        {
            return Convert.IsDBNull(valueObj) ? DateTime.MinValue : (DateTime)valueObj;
        }

        internal static object DateEdit(object valueObj)
        {
            if (Convert.IsDBNull(valueObj))
            {
                return null;
            }

            return Convert.ToDateTime(valueObj);
        }

        internal static decimal Decimal(object valueObj)
        {
            return !Convert.IsDBNull(valueObj) ? Convert.ToDecimal(valueObj) : 0.00M;
        }

        internal static string DecimalNull(decimal p)
        {
            return p == 0.0M ? string.Empty : p.ToString();
        }

        internal static int Int(object valueObj)
        {
            return !Convert.IsDBNull(valueObj) ? Convert.ToInt32(valueObj) : 0;
        }

        internal static object IntNull(int p)
        {
            return p != 0 ? (object)p : null;
        }

        internal static string IntString(int p)
        {
            return p == 0 ? string.Empty : p.ToString();
        }

        internal static void PopulateComboBox(ASPxComboBox cbo, DataTable dt)
        {
            cbo.DataSource = dt;
            cbo.DataBind();
        }
    }
}