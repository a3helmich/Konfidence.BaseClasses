using System;
using System.Collections.Generic;
using System.Text;

namespace Konfidence.Security.ContainerSecurity
{
    internal static class ContainerPermission
    {
        //internal static bool TryKeyContainerPermissionCheck(string secretKeyName) {

        //    bool returnValue = false;

        //    WindowsIdentity current = WindowsIdentity.GetCurrent();

        //    WindowsPrincipal currentPrincipal = new WindowsPrincipal(current);

        //    if (currentPrincipal.IsInRole(WindowsBuiltInRole.Administrator)) {
        //        try {
        //            foreach (string fileName in Directory.GetFiles(
        //                @"C:\Documents and Settings\All Users\" +
        //                @"Application Data\Microsoft\Crypto\RSA\MachineKeys")) {
        //                FileInfo fi = new FileInfo(fileName);

        //                if (fi.Length <= 1024 * 5) { // no key file should be greater then 5KB
        //                    using (StreamReader sr = fi.OpenText()) {
        //                        string fileData = sr.ReadToEnd();
        //                        if (fileData.Contains(secretKeyName)) { // this is our file

        //                            FileSecurity fileSecurity = fi.GetAccessControl();

        //                            bool currentIdentityFoundInACL = false;
        //                            foreach (FileSystemAccessRule rule in fileSecurity
        //                                .GetAccessRules(
        //                                    true,
        //                                    true,
        //                                    typeof(NTAccount)
        //                                )
        //                            ) {
        //                                if (rule.IdentityReference.Value.ToLower() ==
        //                                    current.Name.ToLower()
        //                                ) {
        //                                    returnValue = true;
        //                                    currentIdentityFoundInACL = true;
        //                                    break;
        //                                }
        //                            }

        //                            if (!currentIdentityFoundInACL) {
        //                                fileSecurity.AddAccessRule(
        //                                    new FileSystemAccessRule(
        //                                        current.Name,
        //                                        FileSystemRights.FullControl,
        //                                        AccessControlType.Allow
        //                                    )
        //                                );

        //                                fi.SetAccessControl(fileSecurity);

        //                                returnValue = true;
        //                            }

        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        } catch (UnauthorizedAccessException) {
        //            throw;
        //        } catch { }
        //    }

        //    return returnValue;
        //}

    }
}
