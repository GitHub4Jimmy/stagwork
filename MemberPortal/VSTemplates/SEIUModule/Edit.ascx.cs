using System;
using System.Diagnostics;

using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Exceptions;

using StagwellTech.SEIU.DNN.Modules.$safeprojectname$.Components;


namespace StagwellTech.SEIU.DNN.Modules.$safeprojectname$
{
    /// -----------------------------------------------------------------------------
    /// <summary>   
    /// The Edit class is used to manage content
    /// 
    /// Typically your edit control would be used to create new content, or edit existing content within your module.
    /// The ControlKey for this control is "Edit", and is defined in the manifest (.dnn) file.
    /// 
    /// Because the control inherits from $safeprojectname$ModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Edit : $safeprojectname$ModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Implement your edit logic for your module
                if (!Page.IsPostBack)
                {
                    FillData();
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        void FillData()
        {
            try
            {
                //Implement your edit logic for your module
                if (!Page.IsPostBack)
                {
                    //check if we have an ID passed in via a querystring parameter, if so, load that item to edit.
                    //ItemId is defined in the ItemModuleBase.cs file
                    if (ItemId > 0)
                    {
                        var tc = new ItemController();

                        var t = tc.GetItem(ItemId, ModuleId);
                        if (t != null)
                        {
                            // txtQuestion.Text = t.QuestionText;
                            // txtAnswer.Text = t.AnswerText;
                        }
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var tc = new ItemController();
                var t = tc.GetItem(ItemId, ModuleId);
                bool itemExists = t != null;

                if (!itemExists)
                {
                    t = new Item()
                    {
                        CreatedByUserId = UserId,
                        CreatedOnDate = DateTime.Now
                    };
                }

                //Data

                //End of data

                //Meta data
                t.LastModifiedOnDate = DateTime.Now;
                t.LastModifiedByUserId = UserId;
                t.ModuleId = ModuleId;
                //End of meta

                if (itemExists)
                {
                    tc.UpdateItem(t);
                }
                else
                {
                    tc.CreateItem(t);
                }
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }
    }
}