// XmlTreeViewControl.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using TopCoder.Web.UI.WebControl.TreeView;
using TopCoder.Web.Controls.XmlViewer.Formatters;

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// <para>This class represents the control for insertion into an ASP.NET page.</para>
    /// <para>It includes a container panel, buttons for switching views and saving XML, and properties
    /// for customizing the display of the contained TreeViewControl, as well as a Literal for displaying the
    /// raw code view.</para>
    /// <para>This class isn't inherently thread safe, as its state changes after instantiation,
    /// but it will be used in a thread safe manner by the ASP.NET server.</para>
    /// <para>See Section 4 (Usage notes) of component specification to see how to instantiate
    /// and use this control at design time or programatically.</para>
    /// </summary>
    /// <author>Ghostar</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class XmlTreeViewControl : System.Web.UI.UserControl, INamingContainer
    {
        /// <summary>
        /// The name of the Viewstate item for holding the formatter instance.
        /// </summary>
        private const string ViewStateFormatterName = "XmlTreeViewControl.Formatter";

        /// <summary>
        /// The name of the Viewstate item for holding the raw xml to display using this control.
        /// </summary>
        private const string ViewStateXmlName = "XmlTreeViewControl.XML";

        /// <summary>
        /// The name of the Viewstate item for holding the xml file name when saving the xml.
        /// </summary>
        private const string ViewStateSaveFileName = "XmlTreeViewControl.SaveFileName";

        /// <summary>
        /// The name of the Viewstate item for holding the current view type.
        /// </summary>
        private const string ViewStateViewTypeName = "XmlTreeViewControl.ViewType";

        /// <summary>
        /// The name of the Viewstate item for holding the generic prefix string.
        /// </summary>
        private const string ViewStateGenericPrefix = "XmlTreeViewControl.GenericPrefix";

        /// <summary>
        /// The default file name when save button is clicked.
        /// </summary>
        private const string DefaultSaveFileName = "XmlTree.xml";

        /// <summary>
        /// The default number of elements in prefix collection
        /// </summary>
        private const int DefaultNumPrefixes = 10;

        /// <summary>
        /// Represents html markup for a line break.
        /// </summary>
        private const string HtmlLineBreak = "<br>";

        /// <summary>
        /// This button represents the button at the top of the control that switches the view from raw view to tree
        /// view and back again. This button is created and added to the control collection in the constructor.
        /// </summary>
        private readonly Button changeViewButton;

        /// <summary>
        /// This image button represents the graphical "Save" button.  This button is shown to the right of the change
        /// view button on the control. Clicking this button allows
        /// the user to save the raw XML of the control to their local computer.
        /// This value is created and added to the control collection in the constructor.
        /// </summary>
        private readonly ImageButton saveButton;

        /// <summary>
        /// This Literal control is the control that is used to display the raw, formatted XML of the control. The tree
        /// view is hidden, and this control is displayed, with its text set to the formatted XML string. This value is
        /// created and added to the control collection in the constructor.
        /// </summary>
        private readonly Literal codeLocation;

        /// <summary>
        /// This member variable is the TreeViewControl used to show the tree view of the XML. This control is
        /// initialized and added to the control collection in the constructor. It's display properties are modified
        /// and accessed through the properties in this class.
        /// This tree view is loaded through a GenericXmlTreeLoader using the Xml property value of this class.
        /// </summary>
        private readonly TreeViewControl treeView = new TreeViewControl();

        /// <summary>
        /// This property holds the XML displayed in the control, either in the tree or in the raw view.
        /// </summary>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given parameter is null</exception>
        /// <exception cref="ArgumentException">ArgumentException if the given parameter is an empty string</exception>
        /// <exception cref="InvalidXmlException">
        /// InvalidXmlException if the XML string given isn't well-formed
        /// </exception>
        /// <value>The XML displayed in the control</value>
        [Category("Behavior")]
        [Description("The xml string to be rendered by this control.")]
        public string Xml
        {
            get
            {
                return ViewState[ViewStateXmlName + ID] as string;
            }
            set
            {
                HelperClass.ValidateNotNullNotEmpty(value, "Xml property");
                HelperClass.ValidateWellFormedXml(value, "Xml property");

                ViewState[ViewStateXmlName + ID] = value;

                //Reload control after changing the xml of the control
                ReloadControl();
            }
        }

        /// <summary>
        /// This property holds the IXmlFormatter used to format the XML in the raw view.
        /// This property can be set to null, in which case the default CssXmlFormatter will be returned.
        /// </summary>
        /// <value>The IXmlFormatter used to format the XML in the raw view.</value>
        [Category("Behavior")]
        [Description("The IXmlFormatter interface implementation to be used for formatting the xml in RawXml view. " +
        "If left empty, the CssXmlFormatter class shall be used for formatting.")]
        public IXmlFormatter Formatter
        {
            get
            {
                IXmlFormatter formatter = ViewState[ViewStateFormatterName + ID] as IXmlFormatter;

                //If formatter has not been explicitly set by user, then use CssXmlFormatter
                if (formatter == null)
                {
                    formatter = new CssXmlFormatter();
                    ViewState[ViewStateFormatterName + ID] = formatter;
                }
                return formatter;
            }
            set
            {
                ViewState[ViewStateFormatterName + ID] = value;
            }
        }

        /// <summary>
        /// This property represents the path to the icon that is displayed next to a leaf in the TreeViewControl.
        /// This value can't be set to a null value or an empty string.
        /// </summary>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given value is null</exception>
        /// <exception cref="ArgumentException">ArgumentException if the given value is an empty string</exception>
        /// <value>The path to the icon that is displayed next to a leaf in the TreeViewControl.</value>
        [Category("Appearance")]
        [Description("Image displayed before the content of tree nodes which do not have children")]
        public string LeafIconUrl
        {
            get
            {
                return treeView.LeafIconUrl;
            }
            set
            {
                treeView.LeafIconUrl = value;
            }
        }

        /// <summary>
        /// This property represents the path to the icon that is displayed next to a folder in the TreeViewControl.
        /// This value can't be set to a null value or an empty string.
        /// </summary>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given value is null</exception>
        /// <exception cref="ArgumentException">ArgumentException if the given value is an empty string</exception>
        /// <value>The path to the icon that is displayed next to a folder in the TreeViewControl.</value>
        [Category("Appearance")]
        [Description("Image displayed before the content of nodes which have children")]
        public string FolderIconUrl
        {
            get
            {
                return treeView.FolderIconUrl;
            }
            set
            {
                treeView.FolderIconUrl = value;
            }
        }

        /// <summary>
        /// This property represents the prefixes displayed next to nodes in the TreeViewControl.
        /// This value can't be set to a null value or an empty array.
        /// </summary>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given value is null.</exception>
        /// <exception cref="ArgumentException">
        /// ArgumentException if the given array is empty, or if it contains null values.
        /// </exception>
        /// <value>The prefixes displayed next to nodes in the TreeViewControl.</value>
        [Category("Appearance"), TypeConverter(typeof(StringArrayConverter))]
        [Description("Prefixes displayed before the icons")]
        public string[] Prefixes
        {
            get
            {
                return treeView.Prefixes;
            }
            set
            {
                treeView.Prefixes = value;
            }
        }

        /// <summary>
        /// This property represents the CSS classes for nodes at specific levels in the TreeViewControl.
        /// This value can't be set to a null value or an empty array.
        /// </summary>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given parameter is null</exception>
        /// <exception cref="ArgumentException">
        /// ArgumentException if the given array is empty, or if it contains null values.
        /// </exception>
        /// <value>The CSS classes for nodes at specific levels in the TreeViewControl.</value>
        [Category("Appearance"), TypeConverter(typeof(StringArrayConverter))]
        [Description("CSS class used for each level of tree nodes")]
        public string[] CssClasses
        {
            get
            {
                return treeView.CssClasses;
            }
            set
            {
                treeView.CssClasses = value;
            }
        }

        /// <summary>
        /// This property represents the path to the icon that is displayed next to an expanding folder in the
        /// TreeViewControl. This value can't be set to a null value or an empty string.
        /// </summary>
        /// <exception cref="ArgumentException">ArgumentException if the given parameter is an empty string</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given parameter is null</exception>
        /// <value>The icon that is displayed next to an expanding folder in the TreeViewControl.</value>
        [Category("Appearance")]
        [Description("Url of icon a tree node shown in the waiting state.")]
        public string ExpandingIconUrl
        {
            get
            {
                return treeView.ExpandingIconUrl;
            }
            set
            {
                treeView.ExpandingIconUrl = value;
            }
        }

        /// <summary>
        /// This property represents the text that indicates that a node is expanding in the TreeViewControl.
        /// This value can't be set to a null value or an empty string
        /// </summary>
        /// <exception cref="ArgumentException">ArgumentException if the given parameter is an empty string</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given parameter is null</exception>
        /// <value>The text that indicates that a node is expanding in the TreeViewControl.</value>
        [Category("Appearance")]
        [Description("Text a tree node shown in the waiting state.")]
        public string ExpandingText
        {
            get
            {
                return treeView.ExpandingText;
            }
            set
            {
                treeView.ExpandingText = value;
            }
        }

        /// <summary>
        /// This property represents the CSS class of the text that is displayed when a folder is expanding in the
        /// TreeViewControl. This value can't be set to a null value or an empty string
        /// </summary>
        /// <exception cref="ArgumentException">ArgumentException if the given parameter is an empty string</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given parameter is null</exception>
        /// <value>The CSS class of the text displayed when a folder is expanding in the TreeViewControl.</value>
        [Category("Appearance")]
        [Description("CSS class name applied to a node in the waiting state.")]
        public string ExpandingCssClass
        {
            get
            {
                return treeView.ExpandingCssClass;
            }
            set
            {
                treeView.ExpandingCssClass = value;
            }
        }

        /// <summary>
        /// This property represents the text that is displayed when an expansion operation fails in the
        /// TreeViewControl. This value can't be set to a null value or an empty string.
        /// </summary>
        /// <exception cref="ArgumentException">ArgumentException if the given parameter is an empty string</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given parameter is null</exception>
        /// <value>The text that is displayed when an expansion operation fails in the TreeViewControl.</value>
        [Category("Appearance")]
        [Description("Text shown on the client side when a node failed to load child nodes from server.")]
        public string ExpandingFailureText
        {
            get
            {
                return treeView.ExpandingFailureText;
            }
            set
            {
                treeView.ExpandingFailureText = value;
            }
        }

        /// <summary>
        /// This property represents the path to the icon that is displayed in the Save Button image button.
        /// This value can't be set to a null value or an empty string.
        /// </summary>
        /// <exception cref="ArgumentException">ArgumentException if the given parameter is an empty string</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given parameter is null</exception>
        /// <value>The path to the icon that is displayed in the Save Button image button.</value>
        [Category("Appearance")]
        [Description("Url of icon used for the Save Xml Button.")]
        public string SaveButtonIconUrl
        {
            get
            {
                return saveButton.ImageUrl;
            }
            set
            {
                HelperClass.ValidateNotNullNotEmpty(value, "SaveButtonIconUrl");
                saveButton.ImageUrl = value;
            }
        }

        /// <summary>
        /// This property represents the name of the file that is displayed when user clicks the "save"
        /// button to save the file. If this value isn't set, the file name used by default is "XmlTree.xml".
        /// Cannot be set to null value or an empty string.
        /// </summary>
        /// <exception cref="ArgumentException">ArgumentException if the given parameter is an empty string</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given parameter is null</exception>
        /// <value>The name of the file that is displayed when save button is clicked.</value>
        [Category("Behavior")]
        [Description("The default file name to be used when saving the xml.")]
        public string SaveFileName
        {
            get
            {
                string saveFileName = ViewState[ViewStateSaveFileName + ID] as string;
                if (saveFileName == null)
                {
                    return DefaultSaveFileName;
                }
                return saveFileName;
            }
            set
            {
                HelperClass.ValidateNotNullNotEmpty(value, "SaveFileName");
                ViewState[ViewStateSaveFileName + ID] = value;
            }
        }

        /// <summary>
        /// This property indicates which view is to be shown in the XmlTreeViewControl.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// ArgumentException if a given value isn't a value in the XmlTreeViewType enumeration.
        /// </exception>
        /// <value>The view is to be shown in the XmlTreeViewControl.</value>
        [Category("Appearance")]
        [Description("Whether to render control using RawXml or TreeView format.")]
        public XmlTreeViewType ViewType
        {
            get
            {
                object viewType = ViewState[ViewStateViewTypeName + ID];

                //If viewstate has not been set so far then
                //set the viewstate property and return TreeView
                if (viewType == null)
                {
                    ViewState[ViewStateViewTypeName + ID] = XmlTreeViewType.TreeView;
                    return XmlTreeViewType.TreeView;
                }
                //otherwise return normally
                else
                {
                    return (XmlTreeViewType)viewType;
                }
            }
            set
            {
                //Validate enum value
                if (!Enum.IsDefined(typeof(XmlTreeViewType), value))
                {
                    throw new ArgumentException(value.ToString() + " is not a valid XmlTreeViewType value.");
                }

                ViewState[ViewStateViewTypeName + ID] = value;

                //Reload control after changing the view type.
                ReloadControl();
            }
        }

        /// <summary>
        /// This property is equivalent to setting the Prefixes property. The difference is that only one
        /// string is specified for this property and the Prefixes collection is generated automatically
        /// with the specified string.
        /// This value can be set to null after which the Prefixes property can be reset to any custom value.
        /// </summary>
        /// <value>This property holds the string used to generate the prefix.</value>
        [Category("Appearance")]
        [Description("The string to generate prefixes displayed before the icons")]
        public string GenericPrefix
        {
            get
            {
                return (string)ViewState[ViewStateGenericPrefix + ID];
            }
            set
            {
                if (value == null)
                {
                    ViewState.Remove(ViewStateGenericPrefix + ID);
                }
                else
                {
                    ViewState[ViewStateGenericPrefix + ID] = value;

                    //set the prefixes property
                    string[] prefixes = new string[DefaultNumPrefixes];
                    prefixes[0] = value;
                    for (int i = 1; i < DefaultNumPrefixes; i++)
                    {
                        prefixes[i] = prefixes[i - 1] + value;
                    }
                    Prefixes = prefixes;
                }
            }
        }

        /// <summary>
        /// Constructs an instance of XmlTreeViewControl.
        /// </summary>
        public XmlTreeViewControl()
        {
            //Register page_load event
            this.Load += new EventHandler(Page_Load);

            //Add Change view button to control's children
            changeViewButton = new Button();
            changeViewButton.Width = new Unit(200, UnitType.Pixel);
            Controls.Add(changeViewButton);

            //Add Save xml button to control's children
            saveButton = new ImageButton();
            saveButton.ImageAlign = ImageAlign.AbsMiddle;
            Controls.Add(saveButton);

            //Add a line break after the above 2 controls
            //so that the actual xml is rendered on the next line
            Literal lineBreak = new Literal();
            lineBreak.Text = HtmlLineBreak;
            Controls.Add(lineBreak);

            //Add the Literal control for viewing raw xml
            codeLocation = new Literal();
            Controls.Add(codeLocation);

            //Add the TreeViewControl for viewing xml in tree format
            Controls.Add(treeView);
        }

        /// <summary>
        /// This method is called when the "saveButton" is clicked and it allows the user to save the XML to their local
        /// machine. This method builds a file response and writes the XML to the response object.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event args</param>
        private void SaveButtonClick(object sender, EventArgs e)
        {
            //Set Response properties
            Response.ContentType = "application/xml";
            Response.RedirectLocation = SaveFileName;
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + SaveFileName + "\"");

            //Write the xml to the Http Response stream
            Response.Write(Xml);
            Response.End();
        }

        /// <summary>
        /// This event handler handles the click of the change view button, which switches the view from tree view to
        /// raw view and back again.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event args</param>
        private void ChangeViewButtonClick(object sender, EventArgs e)
        {
            //Interchange the view types.
            if (ViewType == XmlTreeViewType.RawXml)
            {
                ViewType = XmlTreeViewType.TreeView;
            }
            else
            {
                ViewType = XmlTreeViewType.RawXml;
            }
        }

        /// <summary>
        /// This method is called at the load of the control.
        /// It registers the saveButton and changeViewButton click events to their respective event handlers.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Register the click events of buttons
            saveButton.Click += new ImageClickEventHandler(SaveButtonClick);
            changeViewButton.Click += new EventHandler(ChangeViewButtonClick);
        }

        /// <summary>
        /// This method is called when either the ViewType or Xml properties are set. This method ensures the control
        /// is reloaded properly with the updated value in whatever view is showing at the time.
        /// </summary>
        private void ReloadControl()
        {
            if (ViewType == XmlTreeViewType.RawXml)
            {
                //Set the literal text to the formatted xml.
                codeLocation.Text = Formatter.Format(Xml);

                //Update visibility and text of child controls
                changeViewButton.Text = "View XML as tree";
                codeLocation.Visible = true;
                treeView.Visible = false;
            }
            else
            {
                //Initiate the TreeLoader of the TreeViewControl.
                treeView.TreeLoader = new GenericXmlTreeLoader(Xml);

                //Update visibility and text of child controls
                changeViewButton.Text = "View raw XML";
                treeView.Visible = true;
                codeLocation.Visible = false;

                //Bind the xml to the TreeViewControl
                DataBind();
            }
        }

    }
}
