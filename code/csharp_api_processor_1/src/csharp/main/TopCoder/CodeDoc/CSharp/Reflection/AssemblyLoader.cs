// AssemblyLoader.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Reflection;
using System.IO;
using TopCoder.XML.CmdLineProcessor;
using TopCoder.LoggingWrapper;

namespace TopCoder.CodeDoc.CSharp.Reflection
{
    /// <summary>
    /// This internal class represents an assembly loader that is used by the ReflectionEngine. We need this class
    /// because ReflectionEngine provides the ability to search for the desired assemblies in the given paths besides
    /// the default locations (e.g. the current directory and GAC etc.). To use this AssemblyLoader, we first create an
    /// instance of it with the array of paths where to search for the assemblies. Then we call InstallAssemblyResolver
    /// method to register the ResovleAssembly handler for the ResovleAssembly event of the current app domain. When an
    /// assembly name can not be resolved in the default locations, the ResolveAssembly hanlder is called and the
    /// handler will search for the desired assembly in the given paths. Finally we should call UnintallAssemblyREsolver
    /// method to unregiser the handler for the ResolveAssembly event to stop loading assemblies from the custom paths.
    /// </summary>
    ///
    /// <threadsafety>
    /// <para>Thread Safety: This class is immutable and thread safe.</para>
    /// </threadsafety>
    ///
    /// <author>urtks</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal class AssemblyLoader
    {
        /// <summary>
        /// <para>Represents the MBRLogger instance used to log infos, warnings or errors.
        /// Initialized in the constructor. Reference not changed afterwards.
        /// Can be null indicating no logger is used.</para>
        /// </summary>
        private readonly MBRLogger logger;

        /// <summary>
        /// <para>Represents the array of paths where to search for the desired assembly.
        /// Initialized in the constructor. Reference not changed afterwards.
        /// Can be null indicating that no custom paths are searched.
        /// Can not contain null or empty elements.</para>
        /// </summary>
        private readonly string[] referencePaths;

        /// <summary>
        /// <para>Creates a new instance of AssemblyLoader with the given set of custom paths where to search
        /// the desired assembly and no logger.
        /// referencePaths can be null indicating that no custom paths are searched.</para>
        /// </summary>
        /// <param name="referencePaths">
        /// The paths where to search for the dependencies of a particular assembly.
        /// </param>
        /// <exception cref="ArgumentException">If referencePaths has null or empty elements.</exception>
        public AssemblyLoader(string[] referencePaths)
        {
            Helper.ValidateArray(referencePaths, "referencePaths", false, false, true, true);
            
            //Shallow copy
            if (referencePaths != null)
            {
                this.referencePaths = new string[referencePaths.Length];
                Array.Copy(referencePaths, this.referencePaths, referencePaths.Length);
            }
        }

        /// <summary>
        /// <para>Creates a new instance of AssemblyLoader with the given set of custom paths
        /// where to search the desired assembly and the given MBRLogger instance used to log infos,
        /// warnings or errors. referencePaths can be null indicating that no custom paths are searched.</para>
        /// </summary>
        /// <param name="logger">MBRLogger instance used to log infos, warnings or errors</param>
        /// <param name="referencePaths">set of custom paths where to search the dependent assemblies</param>
        /// <exception cref="ArgumentNullException">if logger is null</exception>
        /// <exception cref="ArgumentException">If referencePaths contain null or empty elements.</exception>
        public AssemblyLoader(string[] referencePaths, MBRLogger logger) : this(referencePaths)
        {
            Helper.ValidateNotNull(logger, "logger");
            this.logger = logger;
        }

        /// <summary>
        /// <para>Registers the AssemblyResolver handler for the AssemblyResolve event of the current app domain.</para>
        /// </summary>
        public void InstallAssemblyResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveAssembly);
        }

        /// <summary>
        /// <para>
        /// Unregisters the AssemblyResolver handler for the AssemblyResolve event of the current app domain.
        /// </para>
        /// </summary>
        public void UninstallAssemblyResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(ResolveAssembly);
        }

        /// <summary>
        /// <para>Loads an Assembly from the given fileName. This method first checks if the assembly to load is already
        /// loaded. If so the loaded assembly is returned directly. Otherwise, it calls Assembly.Load to load a new
        /// assembly from the given file. Never returns null.</para>
        /// </summary>
        /// <param name="fileName">The path of the assembly to load.</param>
        /// <exception cref="ArgumentNullException">if fileName is null</exception>
        /// <exception cref="ArgumentException">if fileName is empty</exception>
        /// <exception cref="XmlProcessorException">if unable to load the assembly.</exception>
        /// <returns>The loaded assembly instance.</returns>
        public Assembly LoadAssembly(string fileName)
        {
            Helper.ValidateNotNullNotEmpty(fileName, "fileName");

            try
            {
                //Iterate over all the loaded assemblies in the current app domain
                //and check if the assembly to load is already there.
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(fileName);
                foreach (Assembly loadedAssembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assemblyName.FullName == loadedAssembly.FullName)
                    {
                        return loadedAssembly;
                    }
                }

                return Assembly.Load(assemblyName);
            }
            catch (Exception e)
            {
                throw new XmlProcessorException("Unable to load assembly from fileName: " + fileName, e);
            }
        }

        /// <summary>
        /// <para>Loads an Assembly with the given fullName (fully qualified assembly name)
        /// from the given fileName in one of the custom paths. This methods iterates over the custom paths and
        /// combines the current path with the fileName to form a full path
        /// and use that full path to the load the assembly from. The given fullName is used to ensure that the loaded
        /// assembly is as desired. Any warning or error (except ArgumentNullException and ArgumentException when
        /// fileName is null or empty) is logged using the referenced MBRLogger instance. This method returns null if
        /// unable to load the assembly.</para>
        /// </summary>
        /// <param name="fileName">the name of the file to load the assembly from</param>
        /// <param name="fullName">the full name of the assembly to load.</param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        /// <exception cref="ArgumentException">if any arugment is empty.</exception>
        private Assembly LoadAssemblyFromReferencePaths(string fullName, string fileName)
        {
            Helper.ValidateNotNullNotEmpty(fullName, "fullName");
            Helper.ValidateNotNullNotEmpty(fileName, "fileName");

            if (referencePaths == null)
            {
                return null;
            }

            //Look for assembly in each reference path
            foreach (string path in referencePaths)
            {
                if (Directory.Exists(path))
                {
                    string fullPath = Path.Combine(path, fileName);
                    if (File.Exists(fullPath))
                    {
                        try
                        {
                            //Get the qualified name of the assembly found and compare it to the fullName
                            AssemblyName assyName = AssemblyName.GetAssemblyName(fullPath);
                            if (assyName.FullName == fullName)
                            {
                                return LoadAssembly(fullPath);
                            }
                        }
                        //Ignore exceptions. Just log
                        catch (Exception e)
                        {
                            if (logger != null)
                            {
                                logger.Log(Level.ERROR, "File {0} not a valid assembly.", fullPath);
                                logger.Log(Level.ERROR, "\tException : {0}", e.ToString());
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// <para>This method is the handler of the AssemblyResolve event of the current app domain.
        /// This method is called by the CLR when type resolution for an assembly fails for example when
        /// a referenced assembly is not present in the current path. This method
        /// tries to search the file with suffix '.dll' or '.exe' in the reference paths if the
        /// assembly is not already loaded. Returns null if unable to find the desired assembly.</para>
        /// </summary>
        /// <param name="args">A ResolveEventArgs that contains the assembly data.</param>
        /// <param name="sender">The source of the event.</param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        protected virtual Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            Helper.ValidateNotNull(sender, "sender");
            Helper.ValidateNotNull(args, "args");

            //Return assembly if alreasy loaded in the current domain
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly loadedAssembly in assemblies)
            {
                if (loadedAssembly.FullName == args.Name)
                {
                    return loadedAssembly;
                }
            }

            if (logger != null)
            {
                logger.Log(Level.INFO, "Attempting to resolve assembly {0}.", args.Name);
            }

            string fileName;
            //Split the full name and use the first entry
            string[] assemblyInfo = args.Name.Split(new char[] { ',' });

            Assembly assembly = null;
            
            //Try a dll extension
            fileName = assemblyInfo[0] + ".dll";
            assembly = LoadAssemblyFromReferencePaths(args.Name, fileName);
            if (assembly != null)
            {
                return assembly;
            }

            //Try an exe extension
            fileName = assemblyInfo[0] + ".exe";
            assembly = LoadAssemblyFromReferencePaths(args.Name, fileName);

            return assembly;
        }

    }
}
