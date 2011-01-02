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
            }

            return new ProcessActionResult();
        }

        private ProcessActionResult ProjectTypeValidationExecute(ProcessActionType actionType)
        {
            ListProcessor processor = ListProcessor.GetProcessor(_ProjectName, _ProjectFolder, _ProjectFile, _LanguageType);

            return processor.ProcessProjectTypeValidation(actionType);
        }

        private ProcessActionResult UserControlValidationExecute(ProcessActionType actionType)
        {

            ListProcessor processor = ListProcessor.GetProcessor(_ProjectName, _ProjectFolder, _ProjectFile, _LanguageType);

            return processor.ProcessUserControlValidation(actionType);
        }
    }
}
