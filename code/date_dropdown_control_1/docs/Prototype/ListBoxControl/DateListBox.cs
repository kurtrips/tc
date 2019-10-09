using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Design;
using System.Web.UI.Design.WebControls;

namespace ListBoxControl
{
    [DefaultProperty("InitialSelection")]
    [ToolboxData("<{0}:DateListBox runat=server></{0}:DateListBox>")]
    [System.Drawing.ToolboxBitmap("DateListBox.ico")]
    public class DateListBox : System.Web.UI.WebControls.ListBox
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

        public DateTime SelectedDate
        {
            get
            {
                return new DateTime();
            }
        }

        /*
        protected override void RenderContents(HtmlTextWriter output)
        {
            output.Write(AnotherProp);
        }
        */

        public DateListBox()
        {
        }

        protected override void PerformDataBinding(System.Collections.IEnumerable dataSource)
        {
            base.PerformDataBinding(dataSource);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.SelectionMode = ListSelectionMode.Multiple;
            this.Items[1].Selected = true;
        }

    }
}
