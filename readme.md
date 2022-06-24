# Templates and wizards for faster item creation in Visual Studio

[Templates](https://docs.microsoft.com/en-us/visualstudio/ide/creating-project-and-item-templates?view=vs-2022) can be created to streamline item creation where large amounts of code will be reused. Parts of the code that need to be changed can be replaced by parameters, written as `$paramname$`. Microsoft provides a list of [reserved parameters](https://docs.microsoft.com/en-us/visualstudio/ide/template-parameters?view=vs-2022) that can be used to insert data such as the namespace of the current project and the name of the current file. In addition, custom parameters can be created. These must have their value set either in the template's [.vstemplate](https://docs.microsoft.com/en-us/visualstudio/extensibility/customparameters-element-visual-studio-templates?view=vs-2022) file or by a [wizard](https://docs.microsoft.com/en-us/visualstudio/extensibility/how-to-use-wizards-with-project-templates?view=vs-2022)[^1], which allows values to be set for each item at the time of the item's creation.

[^1]: Despite the article's title, the process of creating a wizard is the same for both project and item templates.

The wizard implementation in the tutorial above uses the [System.Windows.Forms.Form](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form?view=windowsdesktop-6.0) class and can utilize all the functionality for that class. For example, it can include a file dialog to autopopulate the template's custom parameters from the contents of an existing file.

### Wizard installation
A finished template wizard must be installed on the user's machine to be usable in Visual Studio. This can be done by executing the .vsix file in the wizard project's bin folder.