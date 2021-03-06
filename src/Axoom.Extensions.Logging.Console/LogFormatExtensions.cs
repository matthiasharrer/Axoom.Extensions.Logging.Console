﻿using System;
using Axoom.Extensions.Logging.Console.Layouts;
using NLog.Layouts;

namespace Axoom.Extensions.Logging.Console
{
    internal static class LogFormatExtensions
    {
        private static readonly Lazy<GelfLayout> _gelfLayout = new Lazy<GelfLayout>(() => new GelfLayout());
        private static readonly Lazy<GelfLayout> _gelfLayoutWithContext = new Lazy<GelfLayout>(() => new GelfLayout(true));
        private static readonly Lazy<Layout> _plainLayout = new Lazy<Layout>(() => new PlainLayout());

        public static Layout GetLayout(this LogFormat format, bool includeMappedDiagnosticContext)
        {
            switch (format)
            {
                case LogFormat.Gelf when includeMappedDiagnosticContext:
                    return _gelfLayoutWithContext.Value;
                case LogFormat.Gelf:
                    return _gelfLayout.Value;
                case LogFormat.Plain:
                    return _plainLayout.Value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format));
            }
        }
    }
}