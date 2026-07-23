using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Konfidence.UtilHelper
{
    internal class Dll
    {
        /// <summary>
        /// To load the dll - dllFilePath doesn't have to be const - so I can read path from registry
        /// </summary>
        /// <param name="dllFilePath">file path with file name</param>
        /// <param name="hFile">use IntPtr.Zero</param>
        /// <param name="dwFlags">What will happen during loading dll
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
        /// This will to load concrete dll file
        /// </summary>
        /// <param name="dllFilePath">Dll file path</param>
        /// <returns>Pointer to loaded dll</returns>
        /// <exception cref="ApplicationException">
        /// when loading dll will failure
        /// </exception>
        public static IntPtr LoadWin32Library(string dllFilePath)
        {
            IntPtr moduleHandle = LoadLibraryEx(dllFilePath, IntPtr.Zero, LOAD_WITH_ALTERED_SEARCH_PATH);

            if (moduleHandle != IntPtr.Zero)
            {
                return moduleHandle;
            }

            // I'm getting last dll error
            int errorCode = Marshal.GetLastWin32Error();

            throw new ApplicationException(
                $"There was an error during dll loading : {dllFilePath}, error - {errorCode}"
            );
        }
    }
}
