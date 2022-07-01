// Wizard implementation tutorial: https://docs.microsoft.com/en-us/visualstudio/extensibility/how-to-use-wizards-with-project-templates?view=vs-2022
// template schema reference: https://docs.microsoft.com/en-us/visualstudio/extensibility/visual-studio-template-schema-reference?view=vs-2022

using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using EnvDTE;

namespace WizardOfVti
{
    public class VtiCacheableDB : IWizard
    {
        private UserInputForm userInputForm;
        private string modelNamespaceString;
        private string keyTypeString;
        private string valueTypeString;
        private string sqlString;

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
                userInputForm = new UserInputForm();
                userInputForm.ShowDialog();

                modelNamespaceString = UserInputForm.ModelNamespace;
                keyTypeString = UserInputForm.KeyType;
                valueTypeString = UserInputForm.ValueType;
                sqlString = UserInputForm.SqlString;

                // Add custom parameters.
                replacementsDictionary.Add("$modelnamespace$",
                    modelNamespaceString);
                replacementsDictionary.Add("$keytype$",
                    keyTypeString);
                replacementsDictionary.Add("$valuetype$",
                    valueTypeString);
                replacementsDictionary.Add("$sqlstring$",
                    sqlString);
                replacementsDictionary.Add("$databaseName$",
                    UserInputForm.DatabaseName);
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

    public partial class UserInputForm : Form
    {
        // A string (to accept the user-input value), label,
        // and textbox for each custom parameter
        private static string modelNamespace;
        private static string keyType;
        private static string valueType;
        private static string sqlString;
        private static string databaseName;

        private static Label modelNamespaceLabel;
        private static Label keyTypeLabel;
        private static Label valueTypeLabel;
        private static Label sqlStringLabel;
        private static Label databaseNameLabel;

        private TextBox modelNamespaceBox;
        private TextBox keyTypeBox;
        private TextBox valueTypeBox;
        private TextBox sqlStringBox;

        private ListBox databaseNameBox;

        private Button submitButton;
        
        // User has the option of selecting a Model file from
        // which the wizard will attempt to populate the custom 
        // parameter values (except sqlString)
        private Button selectFileButton;
        private OpenFileDialog fileDialog;

        public UserInputForm()
        {
            this.Size = new System.Drawing.Size(576, 225);

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

            keyTypeBox = new TextBox();
            keyTypeBox.Location = new System.Drawing.Point(200, 75);
            keyTypeBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(keyTypeBox);

            keyTypeLabel = new Label();
            keyTypeLabel.Location = new System.Drawing.Point(10, 75);
            keyTypeLabel.Text = "Database key data type: ";
            keyTypeLabel.AutoSize = true;
            keyTypeLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(keyTypeLabel);

            valueTypeBox = new TextBox();
            valueTypeBox.Location = new System.Drawing.Point(200, 100);
            valueTypeBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(valueTypeBox);

            valueTypeLabel = new Label();
            valueTypeLabel.Location = new System.Drawing.Point(10, 100);
            valueTypeLabel.Text = "Database value data type: ";
            valueTypeLabel.AutoSize = true;
            valueTypeLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(valueTypeLabel);

            sqlStringBox = new TextBox();
            sqlStringBox.Location = new System.Drawing.Point(200, 125);
            sqlStringBox.Size = new System.Drawing.Size(350, 20);
            this.Controls.Add(sqlStringBox);

            sqlStringLabel = new Label();
            sqlStringLabel.Location = new System.Drawing.Point(10, 125);
            sqlStringLabel.Text = "SQL DB Query string: ";
            sqlStringLabel.AutoSize = true;
            sqlStringLabel.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(sqlStringLabel);

            submitButton = new Button();
            submitButton.Location = new System.Drawing.Point(10, 150);
            submitButton.AutoSize = true;
            submitButton.Click += submitButton_Click;
            submitButton.Text = "Submit";
            this.Controls.Add(submitButton);

            selectFileButton = new Button();
            selectFileButton.Location = new System.Drawing.Point(100, 150);
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
        private void submitButton_Click(object sender, EventArgs e)
        {
            modelNamespace = modelNamespaceBox.Text;
            keyType = keyTypeBox.Text;
            valueType = valueTypeBox.Text;
            sqlString = sqlStringBox.Text;
            databaseName = databaseNameBox.Text;
            this.Close();
        }
        private void selectFileButton_Click(object sender, EventArgs e)
        {
            fileDialog = new OpenFileDialog();

            // Attempt to auto-navigate to the Models directory
            fileDialog.InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\Models"; 
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileStream = fileDialog.OpenFile();

                // Scan filestream line-by-line for namespace
                // and relevant data types; use these values to
                // populate Form textboxes. This gives the user
                // the chance to edit them afterwards if necessary.
                StreamReader reader = new StreamReader(fileStream);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("namespace"))
                        modelNamespaceBox.Text = line.Substring("namespace ".Length, line.Length - "namespace ".Length);
                    else if (line.Contains("public class "))
                        valueTypeBox.Text = line.Split(new string[] { "class ", " : " }, StringSplitOptions.None)[1];
                    else if (line.Contains(" ID { get; set; }") || line.Contains($" {valueTypeBox.Text}ID {{ get; set }}"))
                        keyTypeBox.Text = line.Split(new string[] { "public ", " ID", $" {valueTypeBox.Text}ID" }, StringSplitOptions.None)[1];
                }
            }
        }
    }
}
