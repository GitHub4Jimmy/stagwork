using System;
using System.Reflection;
using DotNetNuke.Entities.Modules;
using DotNetNuke.ComponentModel.DataAnnotations;
using System.Diagnostics;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.Translation;

namespace StagwellTech.SEIU.CommonDNNEntities.DNNContentHelper
{
    public static class TranslateHelper
    {
        public static T CopyAndTranslateValues<T>(T sObj, int targetModuleId) where T : class, new()
        {
            try
            {
                if (sObj == null) return null;

                var moduleIdName = GetScopedPropertyName<T>();
                var sourceModuleId = GetSourceModuleId(sObj, moduleIdName);

                var mc = new ModuleController();
                var sourceModule = mc.GetModule(sourceModuleId);
                var targetModule = mc.GetModule(targetModuleId);

                var tObj = new T();

                if (sourceModule != null && targetModule != null) //Check if translation is required
                {
                    string sLangCode = sourceModule.CultureCode.Split('-')[0].ToLower();
                    string tLangCode = targetModule.CultureCode.Split('-')[0].ToLower();

                    foreach (PropertyInfo propertyInfo in tObj.GetType().GetProperties())
                    {
                        bool doModuleId = propertyInfo.Name == moduleIdName;
                        bool doTabId = propertyInfo.Name.ToLower() == "TabId".ToLower();
                        bool doCultureCode = propertyInfo.Name.ToLower() == "CultureCode".ToLower();
                        bool doCopy = (CopyOnlyAttribute)propertyInfo.GetCustomAttribute(typeof(CopyOnlyAttribute)) != null;
                        bool doCopyAndTranslate = (CopyAndTranslateAttribute)propertyInfo.GetCustomAttribute(typeof(CopyAndTranslateAttribute)) != null;

                        object sVal = typeof(T).GetProperty(propertyInfo.Name).GetValue(sObj);

                        if (doModuleId) //Resolve target value
                        {
                            propertyInfo.SetValue(tObj, targetModule.ModuleID);
                        }
                        else if (doTabId)
                        {
                            propertyInfo.SetValue(tObj, targetModule.TabID);
                        }
                        else if (doCultureCode)
                        {
                            propertyInfo.SetValue(tObj, targetModule.CultureCode);
                        }
                        else if (doCopy)
                        {
                            propertyInfo.SetValue(tObj, sVal);
                        }
                        else if (doCopyAndTranslate)
                        {
                            if (sLangCode != tLangCode)
                            {
                                try
                                {
                                    var tVal = TranslationHelper.FromString(sVal.ToString(), sLangCode, tLangCode);
                                    propertyInfo.SetValue(tObj, tVal);
                                }
                                catch (Exception e)
                                {
                                    Debug.WriteLine(e.Message);
                                    Debug.WriteLine(e.InnerException);
                                    propertyInfo.SetValue(tObj, sVal);
                                }
                            }

                        }
                    }
                }

                return tObj;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                return sObj;
            }
        }

        private static string GetScopedPropertyName<T>()
        {
            ScopeAttribute scope = (ScopeAttribute)typeof(T).GetCustomAttribute(typeof(ScopeAttribute));
            var moduleIdName = "ModuleId";
            if (scope != null && scope.Scope != null)
            {
                moduleIdName = scope.Scope;
            }
            return moduleIdName;
        }

        private static int GetSourceModuleId<T>(T obj, string moduleIdName)
        {
            try
            {
                return (int)typeof(T).GetProperty(moduleIdName).GetValue(obj);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                throw new Exception("Cannot resolve SourceModule for translation.");
            }

        }
    }
}
