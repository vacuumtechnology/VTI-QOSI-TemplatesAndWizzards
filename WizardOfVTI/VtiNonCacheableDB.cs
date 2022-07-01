using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using EnvDTE;
using System.Text.RegularExpressions;
using System.Linq;

namespace WizardOfVti
{
    public class VtiNonCacheableDB : IWizard
    {
        private NonCacheableUserInputForm userInputForm;
        private string modelNamespaceString;
        private string keyNameString;
        private string keyTypeString;
        private string keyDbType;
        private string valueTypeString;
        private string databaseName;
        private string tableName;
        private string orderByVar;
        private string idSorterString;
        private string selectSqlString;
        private string filteredSqlString;
        private string totalSqlString;
        private string commandBindingsString;

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                // Display a form to the user. The form collects
                // input for the custom message.
                userInputForm = new NonCacheableUserInputForm();
                userInputForm.ShowDialog();

                modelNamespaceString = NonCacheableUserInputForm.ModelNamespace;
                keyNameString = NonCacheableUserInputForm.KeyName;
                keyTypeString = NonCacheableUserInputForm.KeyType;
                keyDbType = NonCacheableUserInputForm.KeyDbType;
                valueTypeString = NonCacheableUserInputForm.ValueType;
                databaseName = NonCacheableUserInputForm.DatabaseName;
                tableName = NonCacheableUserInputForm.TableName;
                orderByVar = NonCacheableUserInputForm.OrderByVar;
                idSorterString = NonCacheableUserInputForm.IdSorterString;
                selectSqlString = NonCacheableUserInputForm.SelectSqlString;
                filteredSqlString = NonCacheableUserInputForm.FilteredSqlString;
                totalSqlString = NonCacheableUserInputForm.TotalSqlString;
                commandBindingsString = NonCacheableUserInputForm.Bindings;

                // Add custom parameters.
                replacementsDictionary.Add("$modelnamespace$",
                    modelNamespaceString);
                replacementsDictionary.Add("$keyName$",
                    keyNameString);
                replacementsDictionary.Add("$keytype$",
                    keyTypeString);
                replacementsDictionary.Add("$keydbtype$",
                    keyDbType);
                replacementsDictionary.Add("$valuetype$",
                    valueTypeString);
                replacementsDictionary.Add("$selectsqlstring$",
                    selectSqlString);
                replacementsDictionary.Add("$tablename$",
                    tableName);
                replacementsDictionary.Add("$databasename$",
                    databaseName);
                replacementsDictionary.Add("$commandbindings$",
                    commandBindingsString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }

    public partial class NonCacheableUserInputForm : Form
    {
        private static string modelNamespace;
        private static string keyName;
        private static string keyType;
        private static string keyDbType;
        private static string valueType;
        private static string databaseName;
        private static string tableName;
        private static string orderByVar;
        private static string idSorterString;
        private static string selectSqlString;
        private static string filteredSqlString;
        private static string totalSqlString;
        private static string bindings;

        public static string ModelNamespace
        {
            get
            {
                return modelNamespace;
            }
            set
            {
                modelNamespace = value;
            }
        }
        public static string KeyName
        {
            get
            {
                return keyName;
            }
            set
            {
                keyName = value;
            }
        }
        public static string KeyType
        {
            get
            {
                return keyType;
            }
            set
            {
                keyType = value;
            }
        }
        public static string KeyDbType
        {
            get
            {
                return keyDbType;
            }
            set
            {
                keyDbType = value;
            }
        }
        public static string ValueType
        {
            get
            {
                return valueType;
            }
            set
            {
                valueType = value;
            }
        }
        public static string DatabaseName 
        {
            get
            {
                return databaseName;
            }
            set
            {
                databaseName = value;
            }
        }
        public static string TableName 
        {
            get
            {
                return tableName;
            }
            set
            {
                tableName = value;
            }
        }
        public static string OrderByVar 
        {
            get
            {
                return orderByVar;
            }
            set
            {
                orderByVar = value;
            }
        }
        public static string IdSorterString 
        {
            get
            {
                return idSorterString;
            }
            set
            {
                idSorterString = value;
            }
        }
        public static string SelectSqlString 
        {
            get
            {
                return selectSqlString;
            }
            set
            {
                selectSqlString = value;
            }
        }
        public static string FilteredSqlString 
        {
            get
            {
                return filteredSqlString;
            }
            set
            {
                filteredSqlString = value;
            }
        }
        public static string TotalSqlString
        { 
            get
            {
                return totalSqlString;
            }
            set
            {
                totalSqlString = value;
            }
        }
        public static string Bindings
        { 
            get
            {
                return bindings;
            }
            set
            {
                bindings = value;
            }
        }
    }
}
