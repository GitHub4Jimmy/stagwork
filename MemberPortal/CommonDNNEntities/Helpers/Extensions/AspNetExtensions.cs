using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Web;

namespace StagwellTech.SEIU.CommonDNNEntities.Helpers
{
    public static class AspNetExtensions
    {

        public static void SetTextSafe(this Literal literal, string text)
        {
            literal.Text = HttpUtility.HtmlEncode(text);
        }
    }
}
