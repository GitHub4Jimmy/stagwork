/*
' Copyright (c) 2019 Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Entities.Modules;

namespace Stagwell.Horiondigital.Modules.$safeprojectname$.Components
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for $safeprojectname$
    /// 
    /// The FeatureController class is defined as the BusinessController in the manifest file (.dnn)
    /// DotNetNuke will poll this class to find out which Interfaces the class implements. 
    /// 
    /// The IPortable interface is used to import/export content from a DNN module
    /// 
    /// The ISearchable interface is used by DNN to index the content of a module
    /// 
    /// The IUpgradeable interface allows module developers to execute code during the upgrade 
    /// process for a module.
    /// 
    /// Below you will find stubbed out implementations of each, uncomment and populate with your own data
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class FeatureController : IPortable, IUpgradeable
    {
        #region Optional Interfaces

        public string ExportModule(int moduleId)
        {
            return "";
        }

        public void ImportModule(int moduleId, string content, string version, int userId)
        {}

        public string UpgradeModule(string version)
        {
            try
            {
                switch (version)
                {
                    case "00.00.01":
                        // run your custom code here
                        return "success";
                    default:
                        return "success";
                }
            }
            catch
            {
                return "failure";
            }
        }

        #endregion
    }
}