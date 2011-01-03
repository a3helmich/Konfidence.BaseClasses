using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.Base;
using WebProjectValidator.EnumTypes;
using WebProjectValidator.HelperClasses;

namespace WebProjectValidator
{
    public class MainFormController : BaseItem
    {
        private string _ProjectName;
        private string _ProjectFolder;
        private string _ProjectFile;
        private LanguageType _LanguageType;

        internal ListProcessor Processor
        {
            get { return ListProcessor.GetProcessor(_ProjectName, _ProjectFolder, _ProjectFile, _LanguageType);  }
        }

        public MainFormController(string projectName, string projectFolder, string projectFile, LanguageType languageType)
        {
            _ProjectName = projectName;
            _ProjectFolder = projectFolder;
            _ProjectFile = projectFile;
            _LanguageType = languageType;
        }

        public ProcessActionResult Execute(ProcessActionType actionType)
        {
            switch (actionType)
            {
                case ProcessActionType.DesignerFileExists:
                    return DesignerFileValidationExecute(actionType);
                case ProcessActionType.DesignerFileMissing:
                    return DesignerFileValidationExecute(actionType);
                case ProcessActionType.DesignerFileAll:
                    return DesignerFileValidationExecute(actionType);
                case ProcessActionType.WebApplication:
                    return ProjectTypeValidationExecute(actionType);
                case ProcessActionType.WebProject:
                    return ProjectTypeValidationExecute(actionType);
                case ProcessActionType.UserControlValid:
                    return UserControlValidationExecute(actionType);
                case ProcessActionType.UserControlInvalid:
                    return UserControlValidationExecute(actionType);
                case ProcessActionType.UserControlMissing:
                    return UserControlValidationExecute(actionType);
                case ProcessActionType.UserControlUnused:
                    return UserControlValidationExecute(actionType);
                case ProcessActionType.UserControlAll:
                    return UserControlValidationExecute(actionType);
                case ProcessActionType.ConvertToWebApplication:
                    ConvertToWebApplicationExecute();
                    break;
                case ProcessActionType.ConvertToWebProject:
                    ConvertToWebProjectExecute();
                    break;
            }

            return new ProcessActionResult();
        }

        private ProcessActionResult DesignerFileValidationExecute(ProcessActionType actionType)
        {
            return Processor.ProcessDesignerFileValidation(actionType);
        }

        private ProcessActionResult ProjectTypeValidationExecute(ProcessActionType actionType)
        {
            return Processor.ProcessProjectTypeValidation(actionType);
        }

        private ProcessActionResult UserControlValidationExecute(ProcessActionType actionType)
        {
            return Processor.ProcessUserControlValidation(actionType);
        }

        private void ConvertToWebProjectExecute()
        {
            // TODO : just get a list of all projectFiles without any processing
            ProcessActionResult projectTypeValidationResult = Processor.ProcessProjectTypeValidation(ProcessActionType.WebProject);

            // web project uses a projectfile -> only files included in the project file must be converted
            Processor.ConvertToWebProject(projectTypeValidationResult.DesignerFileItemList);
        }

        private void ConvertToWebApplicationExecute()
        {
            Processor.ConvertToWebApplication();
        }
    }
}
