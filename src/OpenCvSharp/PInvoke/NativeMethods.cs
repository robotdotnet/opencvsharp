using OpenCvSharp.PInvoke.NativeLibraryUtilties;
using OpenCvSharp.PInvoke.NativeLibraryUtilties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using OpenCvSharp.Util;

// ReSharper disable InconsistentNaming
#pragma warning disable 1591

namespace OpenCvSharp
{
    /// <summary>
    /// P/Invoke methods of OpenCV 2.x C++ interface
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public partial class NativeMethods
    {
        /// <summary>
        /// Is tried P/Invoke once
        /// </summary>
        private static bool tried = false;

        public const string DllMsvcr = "msvcr120";
        public const string DllMsvcp = "msvcp120";

        public const string DllExtern = "OpenCvSharpExtern";

        public const string Version = "310";

        private static readonly string[] RuntimeDllNames =
        {
            DllMsvcr,
            DllMsvcp,
        };

        private static readonly string[] OpenCVDllNames =
        {
            "opencv_world",
        };

        public const string DllFfmpegX86 = "opencv_ffmpeg" + Version;
        public const string DllFfmpegX64 = DllFfmpegX86 + "_64";

        private static readonly bool s_libraryLoaded;
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
        internal static NativeLibraryLoader NativeLoader { get; }
        private static readonly string s_libraryLocation;
        private static readonly bool s_useCommandLineFile;
        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable
        private static readonly bool s_runFinalizer;

        // private constructor. Only used for our unload finalizer
        private NativeMethods() { }
        private void Ping() { } // Used to force compilation
        // static variable used only for interop purposes
        private static readonly NativeMethods finalizeInterop = new NativeMethods();
        ~NativeMethods()
        {
            // If we did not successfully get constructed, we don't need to destruct
            if (!s_runFinalizer) return;
            //Sets logger to null so no logger gets called back.

            NativeLoader.LibraryLoader.UnloadLibrary();

            try
            {
                //Don't delete file if we are using a specified file.
                if (!s_useCommandLineFile && File.Exists(s_libraryLocation))
                {
                    File.Delete(s_libraryLocation);
                }
            }
            catch
            {
                //Any errors just ignore.
            }
        }

        /// <summary>
        /// Static constructor
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        static NativeMethods()
        {
            Console.WriteLine("Using Native Thinggy");
            if (!s_libraryLoaded)
            {
                try
                {
                    //finalizeInterop.Ping();
                    string[] commandArgs = Environment.GetCommandLineArgs();
                    foreach (var commandArg in commandArgs)
                    {
                        //search for a line with the prefix "-opencv:"
                        if (commandArg.ToLower().Contains("-opencv:"))
                        {
                            //Split line to get the library.
                            int splitLoc = commandArg.IndexOf(':');
                            string file = commandArg.Substring(splitLoc + 1);

                            //If the file exists, just return it so dlopen can load it.
                            if (File.Exists(file))
                            {
                                s_libraryLocation = file;
                                s_useCommandLineFile = true;
                            }
                        }
                    }

                    const string resourceRoot = "CameraServer.Native.Libraries.";


                    if (File.Exists("/usr/local/frc/bin/frcRunRobot.sh"))
                    {
                        Console.WriteLine("On RoboRIO");
                        NativeLoader = new NativeLibraryLoader();
                        // RoboRIO
                        if (s_useCommandLineFile)
                        {
                            NativeLoader.LoadNativeLibrary<NativeMethods>(new RoboRioLibraryLoader(), s_libraryLocation, true);
                        }
                        else
                        {
                            NativeLoader.LoadNativeLibrary<NativeMethods>(new RoboRioLibraryLoader(), "/usr/local/frc/lib/libOpenCvSharpExtern.so", true);
                            s_libraryLocation = NativeLoader.LibraryLocation;
                        }
                    }
                    else
                    {
                        NativeLoader = new NativeLibraryLoader();
                        NativeLoader.AddLibraryLocation(OsType.Windows32,
                            resourceRoot + "x86.cscore.dll");
                        NativeLoader.AddLibraryLocation(OsType.Windows64,
                            resourceRoot + "amd64.cscore.dll");
                        NativeLoader.AddLibraryLocation(OsType.Linux32,
                            resourceRoot + "x86.libcscore.so");
                        NativeLoader.AddLibraryLocation(OsType.Linux64,
                            resourceRoot + "amd64.libcscore.so");
                        NativeLoader.AddLibraryLocation(OsType.MacOs32,
                            resourceRoot + "x86.libcscore.dylib");
                        NativeLoader.AddLibraryLocation(OsType.MacOs64,
                            resourceRoot + "amd64.libcscore.dylib");

                        if (s_useCommandLineFile)
                        {
                            NativeLoader.LoadNativeLibrary<NativeMethods>(new RoboRioLibraryLoader(), s_libraryLocation, true);
                        }
                        else
                        {
                            NativeLoader.LoadNativeLibrary<NativeMethods>();
                            s_libraryLocation = NativeLoader.LibraryLocation;
                        }
                    }

                    NativeDelegateInitializer.SetupNativeDelegates<NativeMethods>(NativeLoader);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    Environment.Exit(1);
                }
                s_runFinalizer = true;
                s_libraryLoaded = true;
            }

            // call cv to enable redirecting 
            TryPInvoke();
        }

        /// <summary>
        /// Load DLL files dynamically using Win32 LoadLibrary
        /// </summary>
        /// <param name="additionalPaths"></param>
        public static void LoadLibraries(IEnumerable<string> additionalPaths = null)
        {
            if (IsUnix())
                return;

            string[] ap = EnumerableEx.ToArray(additionalPaths);
            List<string> runtimePaths = new List<string> (ap);
            runtimePaths.Add(Environment.GetFolderPath(Environment.SpecialFolder.System));
            
            foreach (string dll in RuntimeDllNames)
            {
                //WindowsLibraryLoader.Instance.LoadLibrary(dll, runtimePaths);
            }
            foreach (string dll in OpenCVDllNames)
            {
                //WindowsLibraryLoader.Instance.LoadLibrary(dll + Version, ap);
            }

            // calib3d, contrib, core, features2d, flann, highgui, imgproc, legacy,
            // ml, nonfree, objdetect, photo, superres, video, videostab
            //WindowsLibraryLoader.Instance.LoadLibrary(DllExtern, ap);

            // Redirection of error occurred in native library 
            IntPtr zero = IntPtr.Zero;
            IntPtr current = redirectError(ErrorHandlerThrowException, zero, ref zero);
            if (current != IntPtr.Zero)
            {
                ErrorHandlerDefault = (CvErrorCallback)Marshal.GetDelegateForFunctionPointer(
                    current,
                    typeof(CvErrorCallback)
                );
            }
            else
            {
                ErrorHandlerDefault = null;
            }
        }

        /// <summary>
        /// Checks whether PInvoke functions can be called
        /// </summary>
        private static void TryPInvoke()
        {
            if (tried)
                return;
            tried = true;

            try
            {
                core_Mat_sizeof();
            }
            catch (DllNotFoundException e)
            {
                var exception = PInvokeHelper.CreateException(e);
                try{Console.WriteLine(exception.Message);}
                catch{}
                try{Debug.WriteLine(exception.Message);}
                catch{}
                throw exception;
            }
            catch (BadImageFormatException e)
            {
                var exception = PInvokeHelper.CreateException(e);
                try { Console.WriteLine(exception.Message); }
                catch { }
                try { Debug.WriteLine(exception.Message); }
                catch { }
                throw exception;
            }
            catch (Exception e)
            {
                Exception ex = e.InnerException ?? e;
                try{ Console.WriteLine(ex.Message); }
                catch{}
                try{ Debug.WriteLine(ex.Message); }
                catch{}
                throw;
            }
        }

        /// <summary>
        /// Returns whether the OS is Windows or not
        /// </summary>
        /// <returns></returns>
        public static bool IsWindows()
        {
            return !IsUnix();
        }

        /// <summary>
        /// Returns whether the OS is *nix or not
        /// </summary>
        /// <returns></returns>
        public static bool IsUnix()
        {
            var p = Environment.OSVersion.Platform;
            return (p == PlatformID.Unix ||
                    p == PlatformID.MacOSX ||
                    (int)p == 128);
        }

        /// <summary>
        /// Returns whether the runtime is Mono or not
        /// </summary>
        /// <returns></returns>
        public static bool IsMono()
        {
            return (Type.GetType("Mono.Runtime") != null);
        }

        #region Error redirection
        /// <summary>
        /// Custom error handler to be thrown by OpenCV
        /// </summary>
        public static readonly CvErrorCallback ErrorHandlerThrowException =
            delegate(ErrorCode status, string funcName, string errMsg, string fileName, int line, IntPtr userdata)
            {
                try
                {
                    //cvSetErrStatus(CvStatus.StsOk);
                    return 0;
                }
                finally
                {
                    throw new OpenCVException(status, funcName, errMsg, fileName, line);
                }
            };

        /// <summary>
        /// Custom error handler to ignore all OpenCV errors
        /// </summary>
        public static readonly CvErrorCallback ErrorHandlerIgnorance =
            delegate(ErrorCode status, string funcName, string errMsg, string fileName, int line, IntPtr userdata)
            {
                //cvSetErrStatus(CvStatus.StsOk);
                return 0;
            };

        /// <summary>
        /// Default error handler
        /// </summary>
        public static CvErrorCallback ErrorHandlerDefault;
        #endregion
    }
}
