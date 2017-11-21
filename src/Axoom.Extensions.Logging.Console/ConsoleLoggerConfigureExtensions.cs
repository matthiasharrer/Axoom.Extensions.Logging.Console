﻿using System;
using System.Reflection;
using Axoom.Extensions.Logging.Console.LayoutRenderers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.LayoutRenderers;

namespace Axoom.Extensions.Logging.Console
{
    [PublicAPI]
    public static class ConsoleLoggerConfigureExtensions
    {
        /// <summary>
        /// Adds a console logger to the <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        public static ILoggingBuilder AddConsole(this ILoggingBuilder builder) => AddConsole(builder, new ConsoleLoggerOptions());

        /// <summary>
        /// Adds a console logger with the given <paramref name="options"/> to the <paramref name="builder"/>.
        /// </summary>
        public static ILoggingBuilder AddConsole([NotNull] this ILoggingBuilder builder, [NotNull] ConsoleLoggerOptions options)
        {
            SetupLayoutRenderers();
            builder.AddNLog();
            SetupNlogConfig(options);

            return builder;
        }
        
        /// <summary>
        /// Adds a console logger to the <paramref name="factory"/>.
        /// </summary>
        public static ILoggerFactory AddConsole(this ILoggerFactory factory) => AddConsole(factory, new ConsoleLoggerOptions());

        /// <summary>
        /// Adds a console logger with the given <paramref name="options"/> to the <paramref name="factory"/>.
        /// </summary>
        public static ILoggerFactory AddConsole([NotNull] this ILoggerFactory factory, [NotNull] ConsoleLoggerOptions options)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (options == null) throw new ArgumentNullException(nameof(options));

            SetupLayoutRenderers();
            factory.AddNLog();
            SetupNlogConfig(options);

            return factory;
        }

        private static void SetupNlogConfig(ConsoleLoggerOptions options)
        {
            LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging")));
            LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging.Abstractions")));
            LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("NLog.Extensions.Logging")));
            LogManager.AddHiddenAssembly(typeof(ConsoleLoggerConfigureExtensions).GetTypeInfo().Assembly);
            LogManager.Configuration = new NlogConfigurationFactory().Create(options);
            LogManager.Configuration.Reload();
            LogManager.ReconfigExistingLoggers();
        }

        private static void SetupLayoutRenderers()
        {
            LayoutRenderer.Register<SysLogLevelLayoutRenderer>("sysloglevel");
            LayoutRenderer.Register<UnixTimeLayoutRenderer>("unixtime");
        }
    }
}