using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WizardOfVti
{
    public class VtiComplexAPIController : IWizard
    {
        public partial class VtiComplexApiUserInputForm : Form
        {
            private static string modelNamespace;
            private static string dataNamespace;
            private static string keyName;
            private static string keyType;
            private static string valueType;
            private static string routePrefix;
            private static string orderBy;
            private static string textSearchFilter;
            private static string dateTimeProperties;

            private static Label modelNamespaceLabel;
            private static Label dataNamespaceLabel;
            private static Label keyNameLabel;
            private static Label keyTypeLabel;
            private static Label valueTypeLabel;
            private static Label routePrefixLabel;
            private static Label orderByLabel;
            private static Label textSearchFilterLabel;
            private static Label dateTimePropertiesLabel;

            private TextBox modelNamespaceBox;
            private TextBox dataNamespaceBox;
            private TextBox keyNameBox;
            private TextBox keyTypeBox;
            private TextBox valueTypeBox;
            private TextBox routePrefixBox;
            private TextBox orderByBox;
            private TextBox textSearchFilterBox;
            private TextBox dateTimePropertiesBox;

            private Button submitButton;
            private Button selectFileButton;

            private OpenFileDialog fileDialog;

            public VtiComplexApiUserInputForm()
            {
                this.Size = new System.Drawing.Size(576, 325);

                modelNamespaceLabel = new Label();
                modelNamespaceLabel.Location = new System.Drawing.Point(10, 25);
                modelNamespaceLabel.Text = "Model Namespace: ";
                modelNamespaceLabel.AutoSize = true;
                modelNamespaceLabel.ForeColor = System.Drawing.Color.Black;
                this.Controls.Add(modelNamespaceLabel);

                modelNamespaceBox = new TextBox();
                modelNamespaceBox.Location = new System.Drawing.Point(200, 25);
                modelNamespaceBox.Size = new System.Drawing.Size(350, 20);
                this.Controls.Add(modelNamespaceBox);

                dataNamespaceLabel = new Label();
                dataNamespaceLabel.Location = new System.Drawing.Point(10, 50);
                dataNamespaceLabel.Text = "Data Namespace: ";
                dataNamespaceLabel.AutoSize = true;
                dataNamespaceLabel.ForeColor = System.Drawing.Color.Black;
                this.Controls.Add(dataNamespaceLabel);

                dataNamespaceBox = new TextBox();
                dataNamespaceBox.Location = new System.Drawing.Point(200, 50);
                dataNamespaceBox.Size = new System.Drawing.Size(350, 20);
                this.Controls.Add(dataNamespaceBox);

                keyNameLabel = new Label();
                keyNameLabel.Location = new System.Drawing.Point(10, 75);
                keyNameLabel.Text = "Name of Key: ";
                keyNameLabel.AutoSize = true;
                keyNameLabel.ForeColor = System.Drawing.Color.Black;
                this.Controls.Add(keyNameLabel);

                keyNameBox = new TextBox();
                keyNameBox.Location = new System.Drawing.Point(200, 75);
                keyNameBox.Size = new System.Drawing.Size(350, 20);
                this.Controls.Add(keyNameBox);

                keyTypeLabel = new Label();
                keyTypeLabel.Location = new System.Drawing.Point(10, 100);
                keyTypeLabel.Text = "Type of Key: ";
                keyTypeLabel.AutoSize = true;
                keyTypeLabel.ForeColor = System.Drawing.Color.Black;
                this.Controls.Add(keyTypeLabel);

                keyTypeBox = new TextBox();
                keyTypeBox.Location = new System.Drawing.Point(200, 100);
                keyTypeBox.Size = new System.Drawing.Size(350, 20);
                this.Controls.Add(keyTypeBox);

                valueTypeLabel = new Label();
                valueTypeLabel.Location = new System.Drawing.Point(10, 125);
                valueTypeLabel.Text = "Model Name: ";
                valueTypeLabel.AutoSize = true;
                valueTypeLabel.ForeColor = System.Drawing.Color.Black;
                this.Controls.Add(valueTypeLabel);

                valueTypeBox = new TextBox();
                valueTypeBox.Location = new System.Drawing.Point(200, 125);
                valueTypeBox.Size = new System.Drawing.Size(350, 20);
                this.Controls.Add(valueTypeBox);

                routePrefixLabel = new Label();
                routePrefixLabel.Location = new System.Drawing.Point(10, 150);
                routePrefixLabel.Text = "API Route Prefix: ";
                routePrefixLabel.AutoSize = true;
                routePrefixLabel.ForeColor = System.Drawing.Color.Black;
                this.Controls.Add(routePrefixLabel);

                routePrefixBox = new TextBox();
                routePrefixBox.Location = new System.Drawing.Point(200, 150);
                routePrefixBox.Size = new System.Drawing.Size(350, 20);
                this.Controls.Add(routePrefixBox);

                orderByLabel = new Label();
                orderByLabel.Location = new System.Drawing.Point(10, 175);
                orderByLabel.Text = "Order By: ";
                orderByLabel.AutoSize = true;
                orderByLabel.ForeColor = System.Drawing.Color.Black;
                this.Controls.Add(orderByLabel);

                orderByBox = new TextBox();
                orderByBox.Location = new System.Drawing.Point(200, 175);
                orderByBox.Size = new System.Drawing.Size(350, 20);
                this.Controls.Add(orderByBox);

                textSearchFilterLabel = new Label();
                textSearchFilterLabel.Location = new System.Drawing.Point(10, 200);
                textSearchFilterLabel.Text = "Text Search Filter: ";
                textSearchFilterLabel.AutoSize = true;
                textSearchFilterLabel.ForeColor = System.Drawing.Color.Black;
                this.Controls.Add(textSearchFilterLabel);

                textSearchFilterBox = new TextBox();
                textSearchFilterBox.Location = new System.Drawing.Point(200, 200);
                textSearchFilterBox.Size = new System.Drawing.Size(350, 20);
                this.Controls.Add(textSearchFilterBox);

                dateTimePropertiesLabel = new Label();
                dateTimePropertiesLabel.Location = new System.Drawing.Point(10, 225);
                dateTimePropertiesLabel.Text = "Model DateTimes: ";
                dateTimePropertiesLabel.AutoSize = true;
                dateTimePropertiesLabel.ForeColor = System.Drawing.Color.Black;
                this.Controls.Add(dateTimePropertiesLabel);

                dateTimePropertiesBox = new TextBox();
                dateTimePropertiesBox.Location = new System.Drawing.Point(200, 225);
                dateTimePropertiesBox.Size = new System.Drawing.Size(350, 20);
                this.Controls.Add(dateTimePropertiesBox);

                submitButton = new Button();
                submitButton.Location = new System.Drawing.Point(10, 250);
                submitButton.AutoSize = true;
                submitButton.Click += submitButton_Click;
                submitButton.Text = "Submit";
                this.Controls.Add(submitButton);

                selectFileButton = new Button();
                selectFileButton.Location = new System.Drawing.Point(100, 250);
                selectFileButton.AutoSize = true;
                selectFileButton.Click += selectFileButton_Click;
                selectFileButton.Text = "Attempt to populate from Model file";
                this.Controls.Add(selectFileButton);
            }

            public static string ModelNamespace => modelNamespace;
            public static string DataNamespace => dataNamespace;
            public static string KeyName => keyName;
            public static string KeyType => keyType;
            public static string ValueType => valueType;
            public static string RoutePrefix => routePrefix;
            public static string OrderBy => orderBy;
            public static string TextSearchFilter => textSearchFilter;
            public static string DateTimeProperties => dateTimeProperties;

            private void submitButton_Click(object sender, EventArgs e)
            {
                modelNamespace = modelNamespaceBox.Text;
                dataNamespace = dataNamespaceBox.Text;
                keyName = keyNameBox.Text;
                keyType = keyTypeBox.Text;
                valueType = valueTypeBox.Text;
                routePrefix = routePrefixBox.Text;
                orderBy = orderByBox.Text;
                textSearchFilter = textSearchFilterBox.Text;
                dateTimeProperties = dateTimePropertiesBox.Text;

                this.Close();
            }

            class ModelPropertyInfo
            {
                public string Type { get; set; }
                public string Name { get; set; }
                public ModelPropertyInfo(string type, string name)
                {
                    Type = type;
                    Name = name;
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
                                ModelPropertyInfo next = new ModelPropertyInfo(typeString, nameString);
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
                        orderByBox.Text = idColumn.Name;

                        List<string> strings = new List<string>();
                        List<string> dates = new List<string>();
                        foreach (ModelPropertyInfo property in properties)
                        {
                            if (property.Type == "string")
                                strings.Add(property.Name);
                            else if (property.Type == "DateTime")
                                dates.Add(property.Name);
                        }
                        if (strings.Count > 0)
                        {
                            textSearchFilterBox.Text = "(x.";
                            textSearchFilterBox.Text += string.Join(" LIKE '%{textSearch}%' OR x.", strings);
                            textSearchFilterBox.Text += " LIKE '%{textSearch}%')";
                        }
                        if (dates.Count > 0)
                        {
                            dateTimePropertiesBox.Text = "\"";
                            dateTimePropertiesBox.Text += string.Join("\", \"", dates);
                            dateTimePropertiesBox.Text += "\"";
                        }
                    }

                    if (modelNamespaceBox.Text.Replace("WebApplicationQOSI.API.Models", "").Length > 0)
                        routePrefixBox.Text = $"api/{modelNamespaceBox.Text.Replace("WebApplicationQOSI.API.Models.", "").Replace(".", "/")}/{valueTypeBox.Text}";
                    else
                        routePrefixBox.Text = $"api/{valueTypeBox.Text}";
                    dataNamespaceBox.Text = modelNamespaceBox.Text.Replace(".Models", ".Data");
                }
            }
        }
        void IWizard.BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        void IWizard.ProjectFinishedGenerating(Project project)
        {
        }

        void IWizard.ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        void IWizard.RunFinished()
        {
        }

        private VtiComplexApiUserInputForm inputForm;
        void IWizard.RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                inputForm = new VtiComplexApiUserInputForm();
                inputForm.ShowDialog();

                replacementsDictionary.Add("$modelnamespace$",
                    VtiComplexApiUserInputForm.ModelNamespace);
                replacementsDictionary.Add("$datanamespace$",
                    VtiComplexApiUserInputForm.DataNamespace);
                replacementsDictionary.Add("$keyname$",
                    VtiComplexApiUserInputForm.KeyName);
                replacementsDictionary.Add("$keytype$",
                    VtiComplexApiUserInputForm.KeyType);
                replacementsDictionary.Add("$valuetype$",
                    VtiComplexApiUserInputForm.ValueType);
                replacementsDictionary.Add("$routeprefix$",
                    VtiComplexApiUserInputForm.RoutePrefix);
                replacementsDictionary.Add("$orderby$",
                    VtiComplexApiUserInputForm.OrderBy);
                replacementsDictionary.Add("$textsearchfilter$",
                    VtiComplexApiUserInputForm.TextSearchFilter);
                replacementsDictionary.Add("$datetimeproperties$", 
                    VtiComplexApiUserInputForm.DateTimeProperties);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        bool IWizard.ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
