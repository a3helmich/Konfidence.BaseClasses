using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace Konfidence.UtilHelper
{
    class Dll
    {
        /// <summary>
        /// To load the dll - dllFilePath dosen't have to be const - so I can read path from registry
        /// </summary>
        /// <param name="dllFilePath">file path with file name</param>
        /// <param name="hFile">use IntPtr.Zero</param>
        /// <param name="dwFlags">What will happend during loading dll
        /// <para>LOAD_LIBRARY_AS_DATAFILE</para>
        /// <para>DONT_RESOLVE_DLL_REFERENCES</para>
        /// <para>LOAD_WITH_ALTERED_SEARCH_PATH</para>
        /// <para>LOAD_IGNORE_CODE_AUTHZ_LEVEL</para>
        /// </param>
        /// <returns>Pointer to loaded Dll</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibraryEx(string dllFilePath, IntPtr hFile, uint dwFlags);

        /// <summary>
        /// To unload library 
        /// </summary>
        /// <param name="dllPointer">Pointer to Dll witch was returned from LoadLibraryEx</param>
        /// <returns>If unloaded library was correct then true, else false</returns>
        [DllImport("kernel32.dll")]
        [UsedImplicitly]
        public static extern bool FreeLibrary(IntPtr dllPointer);
 
        /// <summary>
        /// To get function pointer from loaded dll 
        /// </summary>
        /// <param name="dllPointer">Pointer to Dll witch was returned from LoadLibraryEx</param>
        /// <param name="functionName">Function name with you want to call</param>
        /// <returns>Pointer to function</returns>



        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr GetProcAddress(IntPtr dllPointer, string functionName);

        private const uint LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008;

        /// <summary>
        /// This will to load concret dll file
        /// </summary>
        /// <param name="dllFilePath">Dll file path</param>
        /// <returns>Pointer to loaded dll</returns>
        /// <exception cref="ApplicationException">
        /// when loading dll will failure
        /// </exception>
        public static IntPtr LoadWin32Library(string dllFilePath)
        {
            var moduleHandle = LoadLibraryEx(dllFilePath, IntPtr.Zero, LOAD_WITH_ALTERED_SEARCH_PATH);
            if (moduleHandle == IntPtr.Zero)
            {
                // I'm gettin last dll error
                var errorCode = Marshal.GetLastWin32Error();
                throw new ApplicationException(
                    $"There was an error during dll loading : {dllFilePath}, error - {errorCode}"
                );
            }
            return moduleHandle;
        }
    }

    // **************************************************************************************************************************
    // That is all, now how to use this functions
    // **************************************************************************************************************************
    [UsedImplicitly]
    public class TestDll
    {
        private static IntPtr _myDll ;
 
        public  static void InitializeMyDll()
        {
            try
            {
                _myDll = Dll.LoadWin32Library("path to my dll with file path name");
                // here you can add, dl version check
            }
            catch (ApplicationException exc)
            {
                MessageBox.Show(exc.Message, "There was an error during dll loading",MessageBoxButtons.OK,MessageBoxIcon.Error);

                throw;
            }
        }
 
// The last thing is to create delegate to calling function
 
// delegate must to have the same parameter then calling function (from dll)
        public delegate int DllFunctionDelegate(int a, string b);
// public delegate RETURN_TYPE DllFunctionDelegate(int a, string b);
        public static int FunctionName(int a, string b)
//public static RETURN_TYPE functionName(int a, string b)
        {
            if (_myDll == IntPtr.Zero)
                InitializeMyDll();
            var pProc = Dll.GetProcAddress(_myDll , "CallingFunctionNameFromCallingDllFile");
            var cpv = (DllFunctionDelegate)Marshal.GetDelegateForFunctionPointer(pProc, typeof(DllFunctionDelegate));
            // Now i'm calling delegate, with is calling function from dll file 
            return cpv(1, "Test");
        }

        // Now if you want to call dll function from program code use this
        // for ex. you want to call c++ function RETURN_TYPE CallingFunctionNameFromCallingDllFile(int nID, LPSTR lpstrName);


        [UsedImplicitly]
        private void Test()
        {
            FunctionName(2, "name");
        }
    }
}