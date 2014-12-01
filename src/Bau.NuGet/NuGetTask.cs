﻿// <copyright file="NuGetTask.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using BauCore;

    public class NuGetTask : BauTask
    {
        private readonly List<Command> commands = new List<Command>();

        public IEnumerable<Command> Commands
        {
            get { return this.commands.ToArray(); }
        }

        public TCommand Add<TCommand>(TCommand command) where TCommand : Command
        {
            if (command != null)
            {
                this.commands.Add(command);
            }

            return command;
        }

        public Restore Restore(string solutionOrPackagesConfig, Action<Restore> configure = null)
        {
            var restore = new Restore().File(solutionOrPackagesConfig);
            if (configure != null)
            {
                configure(restore);
            }

            return this.Add(restore);
        }

        public IEnumerable<Restore> Restore(
            IEnumerable<string> solutionsOrPackagesConfigs, Action<Restore> configure = null)
        {
            Guard.AgainstNullArgument("solutionsOrPackagesConfigs", solutionsOrPackagesConfigs);

            return solutionsOrPackagesConfigs
                .Select(solutionOrPackagesConfig => this.Restore(solutionOrPackagesConfig, configure))
                .ToArray();
        }

        public Pack Pack(string nuspecOrProject, Action<Pack> configure = null)
        {
            var pack = new Pack().File(nuspecOrProject);
            if (configure != null)
            {
                configure(pack);
            }

            return this.Add(pack);
        }

        public IEnumerable<Pack> Pack(IEnumerable<string> nuspecsOrProjects, Action<Pack> configure = null)
        {
            Guard.AgainstNullArgument("nuspecsOrProjects", nuspecsOrProjects);

            return nuspecsOrProjects.Select(projectOrNuSpec => this.Pack(projectOrNuSpec, configure)).ToArray();
        }

        public Push Push(string package, Action<Push> configure = null)
        {
            var push = new Push().File(package);
            if (configure != null)
            {
                configure(push);
            }

            return this.Add(push);
        }

        public IEnumerable<Push> Push(IEnumerable<string> packages, Action<Push> configure = null)
        {
            Guard.AgainstNullArgument("packages", packages);

            return packages.Select(package => this.Push(package, configure)).ToArray();
        }

        protected override void OnActionsExecuted()
        {
            string fileName = null;
            foreach (var processStartInfo in this.commands.Select(command => new ProcessStartInfo
            {
                FileName = command.NuGetExePathOverride ?? fileName ?? (fileName = NuGetFileFinder.FindFile().FullName),
                Arguments = string.Join(" ", command.CreateCommandLineArguments()),
                WorkingDirectory = command.WorkingDirectory,
                UseShellExecute = false
            }))
            {
                processStartInfo.Run();
            }
        }
    }
}
