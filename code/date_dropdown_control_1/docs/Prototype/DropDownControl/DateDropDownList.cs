using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DropDownControl
{
    [DefaultProperty("InitialSelection")]
    [ToolboxData("<{0}:DateDropDownList runat=server></{0}:DateDropDownList>")]
    [System.Drawing.ToolboxBitmap("DateDropDownList.ico")]
    public class DateDropDownList : System.Web.UI.WebControls.DropDownList
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string InitialSelection
        {
            get
            {
                String s = (String)ViewState["InitialSelection"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["InitialSelection"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string InitialSelectionTimeStamp
        {
            get
            {
                String s = (String)ViewState["InitialSelectionTimeStamp"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["InitialSelectionTimeStamp"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string InitialSelectionDateFormat
        {
            get
            {
                String s = (String)ViewState["InitialSelectionDateFormat"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["InitialSelectionDateFormat"] = value;
            }
        }

        /*
        protected override void RenderContents(HtmlTextWriter output)
        {
            output.Write(MyProp);
        }
        */

        public DateDropDownList()
        {
            
        }

        protected override void PerformDataBinding(System.Collections.IEnumerable dataSource)
        {
            base.PerformDataBinding(dataSource);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            foreach (ListItem item in this.Items)
            {
                if (item.Selected == true) return;
            }
            this.Items[1].Selected = true;
        }
    }
}
