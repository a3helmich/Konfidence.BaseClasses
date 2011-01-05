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

        public ProcessActionResult ExecuteEvent(ExecuteEventType eventType, ProcessActionType actionType)
        {
            switch (eventType)
            {
                case ExecuteEventType.ConvertToWebApplication:
                    {
                        Processor.ConvertToWebApplication();
                        break;
                    }
                case ExecuteEventType.ConvertToWebProject:
                    {
                        Processor.ConvertToWebProject(); 
                        break;
                    }
            }

            return ExecuteRefresh(actionType);
        }

        public ProcessActionResult ExecuteRefresh(ProcessActionType actionType)
        {
            switch (actionType)
            {
                case ProcessActionType.DesignerFileExists:
                    {
                        return Processor.ProcessDesignerFileValidation(actionType);
                    }
                case ProcessActionType.DesignerFileMissing:
                    {
                        return Processor.ProcessDesignerFileValidation(actionType);
                    }
                case ProcessActionType.DesignerFileAll:
                    {
                        return Processor.ProcessDesignerFileValidation(actionType);
                    }
                case ProcessActionType.WebApplication:
                    {
                        return Processor.ProcessProjectTypeValidation(actionType);
                    }
                case ProcessActionType.WebProject:
                    {
                        return Processor.ProcessProjectTypeValidation(actionType);
                    }
                case ProcessActionType.UserControlValid:
                    {
                        return Processor.ProcessUserControlValidation(actionType);
                    }
                case ProcessActionType.UserControlInvalid:
                    {
                        return Processor.ProcessUserControlValidation(actionType);
                    }
                case ProcessActionType.UserControlMissing:
                    {
                        return Processor.ProcessUserControlValidation(actionType);
                    }
                case ProcessActionType.UserControlUnused:
                    {
                        return Processor.ProcessUserControlValidation(actionType);
                    }
                case ProcessActionType.UserControlAll:
                    {
                        return Processor.ProcessUserControlValidation(actionType);
                    }
            }

            return new ProcessActionResult();
        }
    }
}
