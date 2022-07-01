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
    public class VtiCRUDCacheableDB : IWizard
    {
        private CRUDCacheableUserInputForm userInputForm;
        private string modelNamespaceString;
        private string keyNameString;
        private string keyTypeString;
        private string keydbtype;
        private string valueTypeString;
        private string selectsqlString;
        private string insertsqlstring;
        private string updatesqlstring;
        private string deletesqlstring;
        private string tableName;
        private string databaseName;
        private string commandbindingsString;

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
                selectsqlString = CRUDCacheableUserInputForm.SelectSqlString;
                insertsqlstring = CRUDCacheableUserInputForm.InsertSqlString;
                updatesqlstring = CRUDCacheableUserInputForm.UpdateSqlString;
                deletesqlstring = CRUDCacheableUserInputForm.DeleteSqlString;
                tableName = CRUDCacheableUserInputForm.TableName;
                databaseName = CRUDCacheableUserInputForm.DatabaseName;
                commandbindingsString = CRUDCacheableUserInputForm.Bindings;


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
                replacementsDictionary.Add("$selectsqlString$",
                    selectsqlString);
                replacementsDictionary.Add("$insertsqlstring$",
                    insertsqlstring);
                replacementsDictionary.Add("$updatesqlstring$",
                    updatesqlstring);
                replacementsDictionary.Add("$deletesqlstring$",
                    deletesqlstring);
                replacementsDictionary.Add("$tableName$",
                    tableName);
                replacementsDictionary.Add("$databaseName$",
                    databaseName);
                replacementsDictionary.Add("$commandbindings$",
                    commandbindingsString);
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
        private static string selectsqlString;
        private static string insertsqlString;
        private static string updatesqlString;
        private static string deletesqlString;
        private static string tableName;
        private static string databaseName;
        private static string bindings;


        private static Label modelNamespaceLabel;
        private static Label keyNameLabel;
        private static Label keyTypeLabel;
        private static Label keyDbTypeLabel;
        private static Label valueTypeLabel;
        private static Label selectsqlStringLabel;
        private static Label insertsqlStringLabel;
        private static Label updatesqlStringLabel;
        private static Label deletesqlStringLabel;
        private static Label tableNameLabel;
        private static Label databaseNameLabel;
        private static Label bindingsLabel;


        private TextBox modelNamespaceBox;
        private TextBox keyNameBox;
        private TextBox keyTypeBox;
        private TextBox keyDbTypeBox;
        private TextBox valueTypeBox;
        private TextBox selectsqlStringBox;
        private TextBox insertsqlStringBox;
        private TextBox updatesqlStringBox;
        private TextBox deletesqlStringBox;
        private TextBox tableNameBox;

        private ListBox databaseNameBox;

        private TextBox bindingsBox;

        private Button submitButton;

        // User has the option of selecting a Model file from
        // which the wizard will attempt to populate the custom 
        // parameter values (except selectsqlString)
        private Button selectFileButton;
        private OpenFileDialog fileDialog;

        public CRUDCacheableUserInputForm()
        {
            this.Size = new System.Drawing.Size(576, 500);

            tableNameBox = new TextBox();
            tableNameBox.Location = new System.Drawing.Point(200, 25);
            tableNameBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(tableNameBox);

            tableNameLabel = new Label();
            tableNameLabel.Location = new System.Drawing.Point(10, 25);
            tableNameLabel.Text = "Table Name (default tbl_{ClassName}): ";
            tableNameLabel.AutoSize = true;
            tableNameLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(tableNameLabel);

            databaseNameBox = new ListBox();
            databaseNameBox.Location = new System.Drawing.Point(200, 50);
            databaseNameBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(databaseNameBox);

            foreach (string s in StaticSettings.DatabaseNames)
            {
                databaseNameBox.Items.Add(s);
            }

            databaseNameBox.SelectedIndex = 0;

            databaseNameLabel = new Label();
            databaseNameLabel.Location = new System.Drawing.Point(10, 50);
            databaseNameLabel.Text = "Database: ";
            databaseNameLabel.AutoSize = true;
            databaseNameLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(databaseNameLabel);

            modelNamespaceBox = new TextBox();
            modelNamespaceBox.Location = new System.Drawing.Point(200, 75);
            modelNamespaceBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(modelNamespaceBox);

            modelNamespaceLabel = new Label();
            modelNamespaceLabel.Location = new System.Drawing.Point(10, 75);
            modelNamespaceLabel.Text = "Model namespace: ";
            modelNamespaceLabel.AutoSize = true;
            modelNamespaceLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(modelNamespaceLabel);

            keyNameBox = new TextBox();
            keyNameBox.Location = new System.Drawing.Point(200, 100);
            keyNameBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(keyNameBox);

            keyNameLabel = new Label();
            keyNameLabel.Location = new System.Drawing.Point(10, 100);
            keyNameLabel.Text = "Key's data name: ";
            keyNameLabel.AutoSize = true;
            keyNameLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(keyNameLabel);

            keyTypeBox = new TextBox();
            keyTypeBox.Location = new System.Drawing.Point(200, 125);
            keyTypeBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(keyTypeBox);

            keyTypeLabel = new Label();
            keyTypeLabel.Location = new System.Drawing.Point(10, 125);
            keyTypeLabel.Text = "Key's data type: ";
            keyTypeLabel.AutoSize = true;
            keyTypeLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(keyTypeLabel);


            keyDbTypeBox = new TextBox();
            keyDbTypeBox.Location = new System.Drawing.Point(200, 150);
            keyDbTypeBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(keyDbTypeBox);

            keyDbTypeLabel = new Label();
            keyDbTypeLabel.Location = new System.Drawing.Point(10, 150);
            keyDbTypeLabel.Text = "Key's SQL Datatype: ";
            keyDbTypeLabel.AutoSize = true;
            keyDbTypeLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(keyDbTypeLabel);

            valueTypeBox = new TextBox();
            valueTypeBox.Location = new System.Drawing.Point(200, 175);
            valueTypeBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(valueTypeBox);

            valueTypeLabel = new Label();
            valueTypeLabel.Location = new System.Drawing.Point(10, 175);
            valueTypeLabel.Text = "Database value data type: ";
            valueTypeLabel.AutoSize = true;
            valueTypeLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(valueTypeLabel);

            selectsqlStringBox = new TextBox();
            selectsqlStringBox.Location = new System.Drawing.Point(200, 200);
            selectsqlStringBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(selectsqlStringBox);

            selectsqlStringLabel = new Label();
            selectsqlStringLabel.Location = new System.Drawing.Point(10, 200);
            selectsqlStringLabel.Text = "SQL DB Query string: ";
            selectsqlStringLabel.AutoSize = true;
            selectsqlStringLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(selectsqlStringLabel);

            insertsqlStringBox = new TextBox();
            insertsqlStringBox.Location = new System.Drawing.Point(200, 225);
            insertsqlStringBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(insertsqlStringBox);

            insertsqlStringLabel = new Label();
            insertsqlStringLabel.Location = new System.Drawing.Point(10, 225);
            insertsqlStringLabel.Text = "SQL DB Insert string: ";
            insertsqlStringLabel.AutoSize = true;
            insertsqlStringLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(insertsqlStringLabel);

            updatesqlStringBox = new TextBox();
            updatesqlStringBox.Location = new System.Drawing.Point(200, 250);
            updatesqlStringBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(updatesqlStringBox);

            updatesqlStringLabel = new Label();
            updatesqlStringLabel.Location = new System.Drawing.Point(10, 250);
            updatesqlStringLabel.Text = "SQL DB Update string: ";
            updatesqlStringLabel.AutoSize = true;
            updatesqlStringLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(updatesqlStringLabel);

            deletesqlStringBox = new TextBox();
            deletesqlStringBox.Location = new System.Drawing.Point(200, 275);
            deletesqlStringBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(deletesqlStringBox);

            deletesqlStringLabel = new Label();
            deletesqlStringLabel.Location = new System.Drawing.Point(10, 275);
            deletesqlStringLabel.Text = "SQL DB Delete string: ";
            deletesqlStringLabel.AutoSize = true;
            deletesqlStringLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(deletesqlStringLabel);

            bindingsBox = new TextBox();
            bindingsBox.Location = new System.Drawing.Point(200, 300);
            bindingsBox.Size = new System.Drawing.Size(350, 120);
            bindingsBox.Multiline = true;
            this.Controls.Add(bindingsBox);

            bindingsLabel = new Label();
            bindingsLabel.Location = new System.Drawing.Point(10, 300);
            bindingsLabel.Text = "Bindings: ";
            bindingsLabel.AutoSize = true;
            bindingsLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(bindingsLabel);

            submitButton = new Button();
            submitButton.Location = new System.Drawing.Point(10, 425);
            submitButton.AutoSize = true;
            submitButton.Click += submitButton_Click;
            submitButton.Text = "Submit";
            this.Controls.Add(submitButton);

            selectFileButton = new Button();
            selectFileButton.Location = new System.Drawing.Point(100, 425);
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
        public static string SelectSqlString
        {
            get
            {
                return selectsqlString;
            }
            set
            {
                selectsqlString = value;
            }
        }
        public static string InsertSqlString
        {
            get
            {
                return insertsqlString;
            }
            set
            {
                insertsqlString = value;
            }
        }
        public static string UpdateSqlString
        {
            get
            {
                return updatesqlString;
            }
            set
            {
                updatesqlString = value;
            }
        }
        public static string DeleteSqlString
        {
            get
            {
                return deletesqlString;
            }
            set
            {
                deletesqlString = value;
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

        private void submitButton_Click(object sender, EventArgs e)
        {
            modelNamespace = modelNamespaceBox.Text;
            keyName = keyNameBox.Text;
            keyType = keyTypeBox.Text;
            keyDbType = keyDbTypeBox.Text;
            valueType = valueTypeBox.Text;
            selectsqlString = selectsqlStringBox.Text;
            insertsqlString = insertsqlStringBox.Text;
            updatesqlString = updatesqlStringBox.Text;
            deletesqlString = deletesqlStringBox.Text;
            tableName = tableNameBox.Text;
            databaseName = databaseNameBox.Text;
            bindings = bindingsBox.Text;

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

                    string databaseTableName = $"tbl_{valueTypeBox.Text}";
                    if (!string.IsNullOrEmpty(tableNameBox.Text))
                        databaseTableName = tableNameBox.Text;
                    else
                        tableNameBox.Text = databaseTableName;

                    string selectionQuery = $"SELECT {idColumn.DbName}{(properties.Count > 0 ? (", " + string.Join(", ", properties.Select(p => p.DbName))) : "")} FROM {databaseTableName};";
                    selectsqlStringBox.Text = selectionQuery;

                    string insertQuery = $"INSERT INTO {databaseTableName} (" +
                        (properties.Count > 0 ? $" {string.Join(", ", properties.Select(p => $"{p.DbName}"))}" : "") +
                        $") VALUES (" +
                        (properties.Count > 0 ? $" {string.Join(", ", properties.Select(p => $"@{p.Name}"))}" : "") +
                        $")";
                    insertsqlStringBox.Text = insertQuery;

                    string updateQuery = $"UPADTE {databaseTableName}" +
                        (properties.Count > 0 ? $" SET {string.Join(", ", properties.Select(p => $"{p.DbName} = @{p.Name}"))}" : "") +
                        $" WHERE {idColumn.DbName} = @{idColumn.Name}";
                    updatesqlStringBox.Text = updateQuery;

                    string deleteQuery = $"DELETE FROM {databaseTableName} WHERE {idColumn.DbName} = @{idColumn.Name};";
                    deletesqlStringBox.Text = deleteQuery;

                    List<string> bindingLines = new List<string>();

                    foreach (ModelPropertyInfo next in properties)
                    {
                        string defaultValue = "";
                        if (!string.IsNullOrEmpty(next.DefaultValue))
                            defaultValue = $" ?? {next.DefaultValue}";
                        if (next.Nullable)
                        {
                            bindingLines.Add($"if (input.{next.Name}.HasValue)");
                            bindingLines.Add("{");
                            bindingLines.Add($"    command.Parameters.Add(\"@{next.Name}\", System.Data.SqlDbType.{next.DbType}).Value = input.{next.Name}{defaultValue};");
                            bindingLines.Add("}");
                            bindingLines.Add("else");
                            bindingLines.Add("{");
                            bindingLines.Add($"    command.Parameters.Add(\"@{next.Name}\", System.Data.SqlDbType.{next.DbType}).Value = DBNull.Value;");
                            bindingLines.Add("}");
                        }
                        else
                            bindingLines.Add($"command.Parameters.Add(\"@{next.Name}\", System.Data.SqlDbType.{next.DbType}).Value = input.{next.Name}{defaultValue};");
                    }

                    string bindingLineText = string.Join(Environment.NewLine, bindingLines);
                    bindingsBox.Text = bindingLineText;
                }
            }
        }
    }
}
