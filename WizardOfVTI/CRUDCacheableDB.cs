// Wizard implementation tutorial: https://docs.microsoft.com/en-us/visualstudio/extensibility/how-to-use-wizards-with-project-templates?view=vs-2022
// template schema reference: https://docs.microsoft.com/en-us/visualstudio/extensibility/visual-studio-template-schema-reference?view=vs-2022
// reference template data at: C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\ItemTemplates\Web\CSharp\1033\WebApiControllerItemTemplateWebv5.0.cs

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
    public class CRUDCacheableDB : IWizard
    {
        private CRUDCacheableUserInputForm userInputForm;
        private string modelNamespaceString;
        private string keyNameString;
        private string keyTypeString;
        public string keydbtype;
        private string valueTypeString;
        private string sqlString;
        public string insertsqlstring;
        public string updatesqlstring;
        public string deletesqlstring;
        public string databaseName;

        // This method is called before opening any item that
        // has the OpenInEditor attribute.
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        // This method is only called for item templates,
        // not for project templates.
        public void ProjectItemFinishedGenerating(ProjectItem
            projectItem)
        {
        }

        // This method is called after the project is created.
        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject,
            Dictionary<string, string> replacementsDictionary,
            WizardRunKind runKind, object[] customParams)
        {
            try
            {
                // Display a form to the user. The form collects
                // input for the custom message.
                userInputForm = new CRUDCacheableUserInputForm();
                userInputForm.ShowDialog();

                modelNamespaceString = CRUDCacheableUserInputForm.ModelNamespace;
                keyNameString = CRUDCacheableUserInputForm.KeyName;
                keyTypeString = CRUDCacheableUserInputForm.KeyType;
                keydbtype = CRUDCacheableUserInputForm.KeyDbType;
                valueTypeString = CRUDCacheableUserInputForm.ValueType;
                sqlString = CRUDCacheableUserInputForm.SqlString;

                // Add custom parameters.
                replacementsDictionary.Add("$modelnamespace$",
                    modelNamespaceString);
                replacementsDictionary.Add("$keyName$",
                    keyNameString);
                replacementsDictionary.Add("$keytype$",
                    keyTypeString);
                replacementsDictionary.Add("$keydbtype$",
                    keydbtype);
                replacementsDictionary.Add("$valuetype$",
                    valueTypeString);
                replacementsDictionary.Add("$sqlstring$",
                    sqlString);
                replacementsDictionary.Add("$insertsqlstring$",
                    insertsqlstring);
                replacementsDictionary.Add("$updatesqlstring$",
                    updatesqlstring);
                replacementsDictionary.Add("$deletesqlstring$",
                    deletesqlstring);
                replacementsDictionary.Add("$databaseName$",
                    databaseName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // This method is only called for item templates,
        // not for project templates.
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }

    public partial class CRUDCacheableUserInputForm : Form
    {
        // A string (to accept the user-input value), label,
        // and textbox for each custom parameter
        private static string modelNamespace;
        private static string keyName;
        private static string keyType;
        private static string keyDbType;
        private static string valueType;
        private static string sqlString;

        private static Label modelNamespaceLabel;
        private static Label keyNameLabel;
        private static Label keyTypeLabel;
        private static Label keyDbTypeLabel;
        private static Label valueTypeLabel;
        private static Label sqlStringLabel;

        private TextBox modelNamespaceBox;
        private TextBox keyNameBox;
        private TextBox keyTypeBox;
        private TextBox keyDbTypeBox;
        private TextBox valueTypeBox;
        private TextBox sqlStringBox;

        private Button submitButton;

        // User has the option of selecting a Model file from
        // which the wizard will attempt to populate the custom 
        // parameter values (except sqlString)
        private Button selectFileButton;
        private OpenFileDialog fileDialog;

        public CRUDCacheableUserInputForm()
        {
            this.Size = new System.Drawing.Size(576, 300);

            modelNamespaceBox = new TextBox();
            modelNamespaceBox.Location = new System.Drawing.Point(200, 25);
            modelNamespaceBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(modelNamespaceBox);

            modelNamespaceLabel = new Label();
            modelNamespaceLabel.Location = new System.Drawing.Point(10, 25);
            modelNamespaceLabel.Text = "Model namespace: ";
            modelNamespaceLabel.AutoSize = true;
            modelNamespaceLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(modelNamespaceLabel);

            keyNameBox = new TextBox();
            keyNameBox.Location = new System.Drawing.Point(200, 50);
            keyNameBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(keyNameBox);

            keyNameLabel = new Label();
            keyNameLabel.Location = new System.Drawing.Point(10, 50);
            keyNameLabel.Text = "Key's data type: ";
            keyNameLabel.AutoSize = true;
            keyNameLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(keyNameLabel);

            keyTypeBox = new TextBox();
            keyTypeBox.Location = new System.Drawing.Point(200, 75);
            keyTypeBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(keyTypeBox);

            keyTypeLabel = new Label();
            keyTypeLabel.Location = new System.Drawing.Point(10, 75);
            keyTypeLabel.Text = "Key's data type: ";
            keyTypeLabel.AutoSize = true;
            keyTypeLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(keyTypeLabel);


            keyDbTypeBox = new TextBox();
            keyDbTypeBox.Location = new System.Drawing.Point(200, 100);
            keyDbTypeBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(keyDbTypeBox);

            keyDbTypeLabel = new Label();
            keyDbTypeLabel.Location = new System.Drawing.Point(10, 100);
            keyDbTypeLabel.Text = "Key's SQL Datatype: ";
            keyDbTypeLabel.AutoSize = true;
            keyDbTypeLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(keyDbTypeLabel);

            valueTypeBox = new TextBox();
            valueTypeBox.Location = new System.Drawing.Point(200, 120);
            valueTypeBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(valueTypeBox);

            valueTypeLabel = new Label();
            valueTypeLabel.Location = new System.Drawing.Point(10, 120);
            valueTypeLabel.Text = "Database value data type: ";
            valueTypeLabel.AutoSize = true;
            valueTypeLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(valueTypeLabel);

            sqlStringBox = new TextBox();
            sqlStringBox.Location = new System.Drawing.Point(200, 150);
            sqlStringBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(sqlStringBox);

            sqlStringLabel = new Label();
            sqlStringLabel.Location = new System.Drawing.Point(10, 150);
            sqlStringLabel.Text = "SQL DB Query string: ";
            sqlStringLabel.AutoSize = true;
            sqlStringLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(sqlStringLabel);

            submitButton = new Button();
            submitButton.Location = new System.Drawing.Point(10, 175);
            submitButton.AutoSize = true;
            submitButton.Click += submitButton_Click;
            submitButton.Text = "Submit";
            this.Controls.Add(submitButton);

            selectFileButton = new Button();
            selectFileButton.Location = new System.Drawing.Point(100, 175);
            selectFileButton.AutoSize = true;
            selectFileButton.Click += selectFileButton_Click;
            selectFileButton.Text = "Attempt to populate from Model file";
            this.Controls.Add(selectFileButton);
        }
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
        public static string SqlString
        {
            get
            {
                return sqlString;
            }
            set
            {
                sqlString = value;
            }
        }
        private void submitButton_Click(object sender, EventArgs e)
        {
            modelNamespace = modelNamespaceBox.Text;
            keyName = keyNameBox.Text;
            keyType = keyTypeBox.Text;
            keyDbType = keyTypeBox.Text;
            valueType = valueTypeBox.Text;
            sqlString = sqlStringBox.Text;
            this.Close();
        }

        class ModelPropertyInfo
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string DbType { get; set; }
            public string DbName { get; set; }
            public string DefaultValue { get; set; }
            public bool Nullable { get; set; }
            public ModelPropertyInfo(string type, string name, string dbName, string defaultValue, bool nullable)
            {
                Type = type;
                Name = name;
                DbName = dbName;
                DefaultValue = defaultValue;
                Nullable = nullable;
                switch (type)
                {
                    case "int":
                        DbType = "Int";
                        break;
                    case "long":
                        DbType = "BigInt";
                        break;
                    case "string":
                        DbType = "NVarChar";
                        break;
                    case "bool":
                        DbType = "Bit";
                        break;
                    case "double":
                        DbType = "Float";
                        break;
                    case "DateTime":
                        DbType = "DateTime";
                        break;
                }
            }
        }

        private void selectFileButton_Click(object sender, EventArgs e)
        {
            fileDialog = new OpenFileDialog();

            // Attempt to auto-navigate to the Models directory
            fileDialog.InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\Models";
            List<ModelPropertyInfo> properties = new List<ModelPropertyInfo>();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileStream = fileDialog.OpenFile();

                // Scan filestream line-by-line for namespace
                // and relevant data types; use these values to
                // populate Form textboxes. This gives the user
                // the chance to edit them afterwards if necessary.
                StreamReader reader = new StreamReader(fileStream);
                string line;

                string previousAutoPopulate = "";

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("namespace"))
                        modelNamespaceBox.Text = line.Substring("namespace ".Length, line.Length - "namespace ".Length);
                    else if (line.Contains("public class "))
                        valueTypeBox.Text = line.Split(new string[] { "class ", " : " }, StringSplitOptions.None)[1];
                    else if (previousAutoPopulate == "false")
                    {
                        previousAutoPopulate = "";
                        continue;
                    }
                    else
                    {
                        Regex r = new Regex(@"public (?<type>[^c]\w+)(?<nullable>\?)?\s+(?<name>\w+)[^=\n]*(?:= (?<default>[""\w]+);)?");
                        Match m = r.Match(line);
                        if (m.Success)
                        {
                            string typeString = m.Groups["type"].Value ?? "";
                            string nameString = m.Groups["name"].Value ?? "";
                            string defaultString = m.Groups["default"].Value ?? "";
                            string nullable = m.Groups["nullable"].Value ?? "";
                            string dbNameString = nameString;
                            if (!string.IsNullOrWhiteSpace(previousAutoPopulate))
                            {
                                dbNameString = previousAutoPopulate.Replace("\"", "");
                            }
                            ModelPropertyInfo next = new ModelPropertyInfo(typeString, nameString, dbNameString, defaultString, nullable == "?");
                            properties.Add(next);
                        }
                        else
                        {
                            Regex r2 = new Regex(@"\[AutoPopulate\((?<populate>(?:false)|(?:[""\w]+))\)\]");
                            Match m2 = r2.Match(line);
                            if (m2.Success)
                            {
                                previousAutoPopulate = m2.Groups["populate"].Value;
                            }
                        }
                    }
                }

                if (properties.Count > 0)
                {
                    ModelPropertyInfo idColumn = properties[0];
                    properties.RemoveAt(0);

                    keyNameBox.Text = idColumn.Name;
                    keyTypeBox.Text = idColumn.Type;
                    keyDbTypeBox.Text = idColumn.DbType;

                    string selectionQuery = $"SELECT {idColumn.DbName}{(properties.Count > 0 ? (", " + string.Join(", ", properties.Select(p => p.DbName))) : "")} FROM tbl_{valueTypeBox.Text};";
                    sqlStringBox.Text = selectionQuery;
                }
            }
        }
    }
}
