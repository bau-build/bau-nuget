﻿// <copyright file="CommandExtensions.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    public static class CommandExtensions
    {
        public static T WithWorkingDirectory<T>(this T command, string workingDirectory) where T : Command
        {
            command.WorkingDirectory = workingDirectory;
            return command;
        }

        public static T WithNuGetExePathOverride<T>(this T command, string nugetExePath) where T : Command
        {
            command.NuGetExePathOverride = nugetExePath;
            return command;
        }

        public static T WithVerbosity<T>(this T command, string verbosity) where T : Command
        {
            command.Verbosity = verbosity;
            return command;
        }

        public static T WithVerbosityDetailed<T>(this T command) where T : Command
        {
            return command.WithVerbosity("detailed");
        }

        public static T WithVerbosityQuiet<T>(this T command) where T : Command
        {
            return command.WithVerbosity("quiet");
        }

        public static T WithVerbosityNormal<T>(this T command) where T : Command
        {
            return command.WithVerbosity("normal");
        }

        public static T WithConfigFile<T>(this T command, string configFilePath) where T : Command
        {
            command.ConfigFile = configFilePath;
            return command;
        }
    }
}