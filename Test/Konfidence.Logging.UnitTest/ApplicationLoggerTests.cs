using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using FluentAssertions;
using Konfidence.Logging.Inspection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;
using Serilog.Events;

// ReSharper disable ExplicitCallerInfoArgument

namespace Konfidence.Logging.UnitTest;

[TestClass]
public sealed class ApplicationLoggerTests
{
    private sealed class TestService;

    private sealed class ThrowingObject
    {
        public string Name => throw new InvalidOperationException("Serialization failed");
    }

    [TestMethod]
    public void DefaultConstructor_UsesConfiguredGlobalLogger()
    {
        // Arrange
        ILogger originalLogger = Log.Logger;
        CollectingSink sink = new();

        try
        {
            Log.Logger = BuildSerilogLogger(sink);
            ApplicationLogger logger = new();

            // Act
            logger.InformationRaw("default logger");

            // Assert
            LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
            logEvent.RenderMessage().Should().Contain("default logger");
            logEvent.Properties["SourceContext"].ToString().Should().Contain(nameof(ApplicationLogger));
        }
        finally
        {
            Log.Logger = originalLogger;
        }
    }

    [TestMethod]
    public void InspectingSink_StoresOptionsAndRenderedMessage()
    {
        // Arrange
        CollectingSink sink = new();
        FileSinkOptions options = new()
        {
            Path = "test.log",
            OutputTemplate = "{Message:lj}"
        };
        InspectingSink inspectingSink = new(BuildSerilogLogger(sink), options);
        ILogger logger = new LoggerConfiguration()
            .WriteTo.Sink(inspectingSink)
            .CreateLogger();

        // Act
        logger.Information("inspected message");

        // Assert
        inspectingSink.Options.Should().BeSameAs(options);
        inspectingSink.RenderedMessage.Should().Be("inspected message");
        sink.Events.Should().ContainSingle();
    }

    [TestMethod]
    public void AddLoggingServices_WithApplicationName_HidesSourceContextForInformationConsoleOutput()
    {
        // Arrange
        ILogger originalLogger = Log.Logger;
        TextWriter originalOutput = Console.Out;
        using StringWriter output = new();

        try
        {
            Console.SetOut(output);
            ServiceCollection services = new();

            // Act
            services.AddLoggingServices("UnitTest");
            Log.ForContext<TestService>().Information("application logging console restore");

            // Assert
            output.ToString().Should().Contain("application logging console restore");
            output.ToString().Should().NotContain("SourceContext");
            output.ToString().Should().NotContain(nameof(TestService));
        }
        finally
        {
            Log.Logger = originalLogger;
            Console.SetOut(originalOutput);
        }
    }

    [TestMethod]
    public void AddLoggingServices_WithApplicationName_ShowsSourceContextForErrorConsoleOutput()
    {
        // Arrange
        ILogger originalLogger = Log.Logger;
        TextWriter originalError = Console.Error;
        using StringWriter errorOutput = new();

        try
        {
            Console.SetError(errorOutput);
            ServiceCollection services = new();

            // Act
            services.AddLoggingServices("UnitTest");
            Log.ForContext<TestService>().Error("application logging error context");

            // Assert
            errorOutput.ToString().Should().Contain("application logging error context");
            errorOutput.ToString().Should().Contain("SourceContext");
            errorOutput.ToString().Should().Contain(nameof(TestService));
        }
        finally
        {
            Log.Logger = originalLogger;
            Console.SetError(originalError);
        }
    }

    [TestMethod]
    public void AddLoggingServices_WithApplicationName_ShowsSourceContextForDebugConsoleOutput()
    {
        // Arrange
        ILogger originalLogger = Log.Logger;
        TextWriter originalOutput = Console.Out;
        using StringWriter output = new();

        try
        {
            Console.SetOut(output);
            ServiceCollection services = new();

            // Act
            services.AddLoggingServices("UnitTest");
            Log.ForContext<TestService>().Debug("application logging debug context");

            // Assert
            output.ToString().Should().Contain("application logging debug context");
            output.ToString().Should().Contain("SourceContext");
            output.ToString().Should().Contain(nameof(TestService));
        }
        finally
        {
            Log.Logger = originalLogger;
            Console.SetOut(originalOutput);
        }
    }

    [TestMethod]
    public void CreateLogger_WithoutSerilogLogger_UsesConfiguredGlobalLogger()
    {
        // Arrange
        ILogger originalLogger = Log.Logger;
        CollectingSink sink = new();

        try
        {
            Log.Logger = BuildSerilogLogger(sink);
            ApplicationLoggerFactory loggerFactory = new();

            // Act
            IApplicationLogger logger = loggerFactory.CreateLogger(typeof(TestService), LogEventLevel.Information);
            logger.InformationRaw("global logger");

            // Assert
            LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
            logEvent.RenderMessage().Should().Contain("global logger");
            logEvent.Properties["SourceContext"].ToString().Should().Contain(nameof(TestService));
        }
        finally
        {
            Log.Logger = originalLogger;
        }
    }

    [TestMethod]
    public void CreateLogger_WithSerilogLogger_UsesProvidedLogger()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLoggerFactory loggerFactory = new();

        // Act
        IApplicationLogger logger = loggerFactory.CreateLogger(
            typeof(TestService),
            BuildSerilogLogger(sink),
            LogEventLevel.Information);
        logger.InformationRaw("provided logger");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.RenderMessage().Should().Contain("provided logger");
        logEvent.Properties["SourceContext"].ToString().Should().Contain(nameof(TestService));
    }

    [TestMethod]
    public void GetFileOnlyLogger_WritesToCacheFile()
    {
        // Arrange
        string cacheLocation = CreateTempDirectory();

        try
        {
            ILogger logger = ApplicationLoggerFactory.GetFileOnlyLogger("UnitTest", cacheLocation);

            // Act
            logger.Information("cache logging message");
            DisposeLogger(logger);

            // Assert
            string logFile = WaitForLogFile(cacheLocation);
            ReadAllTextSharedEventually(logFile, "cache logging message").Should().Contain("cache logging message");
        }
        finally
        {
            Directory.Delete(cacheLocation, true);
        }
    }

    [TestMethod]
    public void GetLogger_WithCustomLocation_RoutesMatchingSourceToCustomFile()
    {
        // Arrange
        string customLocation = CreateTempDirectory();

        try
        {
            ILogger logger = ApplicationLoggerFactory.GetLogger("UnitTest", customLocation, "Custom.Source");

            // Act
            logger.ForContext(Constants.SourceContextPropertyName, "Custom.Source.Service")
                .Information("custom source message");
            DisposeLogger(logger);

            // Assert
            string logFile = WaitForLogFile(customLocation);
            ReadAllTextSharedEventually(logFile, "custom source message").Should().Contain("custom source message");
        }
        finally
        {
            TryDeleteDirectory(customLocation);
        }
    }

    [TestMethod]
    public void GetLogger_WithCustomLocation_RoutesOtherSourceToApplicationConsole()
    {
        // Arrange
        string customLocation = CreateTempDirectory();
        TextWriter originalOutput = Console.Out;
        using StringWriter output = new();

        try
        {
            Console.SetOut(output);
            ILogger logger = ApplicationLoggerFactory.GetLogger("UnitTest", customLocation, "Custom.Source");

            // Act
            logger.ForContext(Constants.SourceContextPropertyName, "Other.Source.Service")
                .Information("application source message");
            DisposeLogger(logger);

            // Assert
            output.ToString().Should().Contain("application source message");
        }
        finally
        {
            Console.SetOut(originalOutput);
            TryDeleteDirectory(customLocation);
        }
    }

    [TestMethod]
    public void GetLogger_WithCustomLocation_RoutesMissingSourceContextToApplicationConsole()
    {
        // Arrange
        string customLocation = CreateTempDirectory();
        TextWriter originalOutput = Console.Out;
        using StringWriter output = new();

        try
        {
            Console.SetOut(output);
            ILogger logger = ApplicationLoggerFactory.GetLogger("UnitTest", customLocation, "Custom.Source");

            // Act
            logger.Information("missing source context message");
            DisposeLogger(logger);

            // Assert
            output.ToString().Should().Contain("missing source context message");
        }
        finally
        {
            Console.SetOut(originalOutput);
            TryDeleteDirectory(customLocation);
        }
    }

    [TestMethod]
    public void GetLogger_WithCustomLocationAndEmptySource_UsesDefaultApplicationLogger()
    {
        // Arrange
        string customLocation = CreateTempDirectory();
        TextWriter originalOutput = Console.Out;
        using StringWriter output = new();

        try
        {
            Console.SetOut(output);
            ILogger logger = ApplicationLoggerFactory.GetLogger("UnitTest", customLocation, string.Empty);

            // Act
            logger.Information("default application logger message");
            DisposeLogger(logger);

            // Assert
            output.ToString().Should().Contain("default application logger message");
        }
        finally
        {
            Console.SetOut(originalOutput);
            TryDeleteDirectory(customLocation);
        }
    }

    [TestMethod]
    public void Error_WhenLogLevelHigherThanError_DoesNotLog()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Fatal);

        // Act
        logger.Error(new InvalidOperationException("boom"), "message", "MyMethod");

        // Assert
        sink.Events.Should().BeEmpty();
    }

    [TestMethod]
    public void Error_WithMessage_LogsErrorWithExceptionAndSourceContext()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Error);
        ArgumentNullException exception = new();

        // Act
        logger.Error(exception, "the message", "CallerName");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.Level.Should().Be(LogEventLevel.Error);
        logEvent.Exception.Should().BeSameAs(exception);
        logEvent.RenderMessage().Should().Contain("CallerName");
        logEvent.RenderMessage().Should().Contain("the message");
        logEvent.Properties["SourceContext"].ToString().Should().Contain(nameof(TestService));
    }

    [TestMethod]
    public void Error_WithoutMessage_LogsMethodOnly()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Error);
        InvalidOperationException exception = new();

        // Act
        logger.Error(exception, method: "CallerName");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.Exception.Should().BeSameAs(exception);
        logEvent.RenderMessage().Should().Contain("CallerName");
    }

    [TestMethod]
    public void Verbose_WhenLogLevelHigherThanVerbose_DoesNotLog()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Information);

        // Act
        logger.Verbose(new { Name = "x" }, LogAction.None, "M");

        // Assert
        sink.Events.Should().BeEmpty();
    }

    [TestMethod]
    public void Verbose_Object_LogsVerbose()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Verbose);

        // Act
        logger.Verbose(new { Name = "x" }, LogAction.Start, "M");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.Level.Should().Be(LogEventLevel.Verbose);
        logEvent.RenderMessage().Should().Contain("M");
        logEvent.RenderMessage().Should().Contain(LogAction.Start.ToString());
        logEvent.RenderMessage().Should().Contain("Name");
    }

    [TestMethod]
    public void Verbose_Object_WithNoAction_LogsWithoutActionText()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Verbose);

        // Act
        logger.Verbose(new { Name = "x" }, LogAction.None, "M");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.Level.Should().Be(LogEventLevel.Verbose);
        logEvent.RenderMessage().Should().NotContain($"({LogAction.Start})");
    }

    [TestMethod]
    public void Verbose_Object_WhenSerializationFails_LogsError()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Verbose);

        // Act
        logger.Verbose(new ThrowingObject(), LogAction.Start, "M");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.Level.Should().Be(LogEventLevel.Error);
        logEvent.RenderMessage().Should().Contain("UNABLE TO SERIALIZE OBJECT");
    }

    [TestMethod]
    public void Verbose_ActionOverload_LogsVerbose()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Verbose);

        // Act
        logger.Verbose(LogAction.Stop, "M");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.Level.Should().Be(LogEventLevel.Verbose);
        logEvent.RenderMessage().Should().Contain(LogAction.Stop.ToString());
    }

    [TestMethod]
    public void Verbose_StringOverload_LogsVerboseWhenAllowed()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Verbose);

        // Act
        logger.Verbose("a message", "M");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.Level.Should().Be(LogEventLevel.Verbose);
        logEvent.RenderMessage().Should().Contain("a message");
    }

    [TestMethod]
    public void Verbose_StringOverload_WhenLogLevelHigherThanVerbose_DoesNotLog()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Information);

        // Act
        logger.Verbose("a message", "M");

        // Assert
        sink.Events.Should().BeEmpty();
    }

    [TestMethod]
    public void Information_ActionOverload_LogsInformation()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Information);

        // Act
        logger.Information(LogAction.Start, "CallerMethod");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.Level.Should().Be(LogEventLevel.Information);
        logEvent.RenderMessage().Should().Contain(LogAction.Start.ToString());
    }

    [TestMethod]
    public void Information_String_LogLevelWarning_DoesNotLog()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Warning);

        // Act
        logger.Information("a message", "MethodName");

        // Assert
        sink.Events.Should().BeEmpty();
    }

    [TestMethod]
    public void Information_String_LogLevelInformation_LogsInformation()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Information);

        // Act
        logger.Information("a message", "MethodName");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.Level.Should().Be(LogEventLevel.Information);
        logEvent.RenderMessage().Should().Contain("MethodName");
        logEvent.RenderMessage().Should().Contain("a message");
    }

    [TestMethod]
    public void Information_Object_LogsInformation()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Information);

        // Act
        logger.Information(new { Name = "x" }, LogAction.Start, "M");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.Level.Should().Be(LogEventLevel.Information);
        logEvent.RenderMessage().Should().Contain(LogAction.Start.ToString());
        logEvent.RenderMessage().Should().Contain("Name");
    }

    [TestMethod]
    public void Information_Object_WithNoAction_LogsWithoutActionText()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Information);

        // Act
        logger.Information(new { Name = "x" }, LogAction.None, "M");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.Level.Should().Be(LogEventLevel.Information);
        logEvent.RenderMessage().Should().NotContain($"({LogAction.Start})");
    }

    [TestMethod]
    public void Information_Object_WhenSerializationFails_LogsError()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Information);

        // Act
        logger.Information(new ThrowingObject(), LogAction.Start, "M");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.Level.Should().Be(LogEventLevel.Error);
        logEvent.RenderMessage().Should().Contain("UNABLE TO SERIALIZE OBJECT");
    }

    [TestMethod]
    public void Information_Object_WhenLogLevelHigherThanInformation_DoesNotLog()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Warning);

        // Act
        logger.Information(new { Name = "x" }, LogAction.Start, "M");

        // Assert
        sink.Events.Should().BeEmpty();
    }

    [TestMethod]
    public void InformationRaw_LogLevelInformation_LogsInformation()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Information);

        // Act
        logger.InformationRaw("important");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.Level.Should().Be(LogEventLevel.Information);
        logEvent.RenderMessage().Should().Contain("important");
    }

    [TestMethod]
    public void InformationRaw_LogLevelWarning_DoesNotLog()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = CreateLogger(sink, LogEventLevel.Warning);

        // Act
        logger.InformationRaw("important");

        // Assert
        sink.Events.Should().BeEmpty();
    }

    [TestMethod]
    public void InformationRaw_WhenSetupLoggerWasNotCalled_LogsWithDefaultSourceContext()
    {
        // Arrange
        CollectingSink sink = new();
        ApplicationLogger logger = new(
            typeof(ApplicationLogger),
            BuildSerilogLogger(sink),
            LogEventLevel.Information);

        // Act
        logger.InformationRaw("important");

        // Assert
        LogEvent logEvent = sink.Events.Should().ContainSingle().Subject;
        logEvent.Level.Should().Be(LogEventLevel.Information);
        logEvent.Properties["SourceContext"].ToString().Should().Contain(nameof(ApplicationLogger));
    }

    private static ApplicationLogger CreateLogger(
        CollectingSink sink,
        LogEventLevel logEventLevel)
    {
        return new(
            typeof(TestService),
            BuildSerilogLogger(sink),
            logEventLevel);
    }

    private static ILogger BuildSerilogLogger(ILogEventSink sink)
    {
        return new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Sink(sink)
            .CreateLogger();
    }

    private static string CreateTempDirectory()
    {
        string directory = Path.Combine(Path.GetTempPath(), $"Konfidence.Logging.UnitTest.{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);

        return directory;
    }

    private static void DisposeLogger(ILogger logger)
    {
        if (logger is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private static void TryDeleteDirectory(string directory)
    {
        try
        {
            Directory.Delete(directory, true);
        }
        catch (IOException)
        {
        }
    }

    private static string ReadAllTextShared(string path)
    {
        using FileStream stream = new(
            path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.ReadWrite | FileShare.Delete);
        using StreamReader reader = new(stream, Encoding.UTF8);

        return reader.ReadToEnd();
    }

    private static string ReadAllTextSharedEventually(string path, string expectedText)
    {
        string text = string.Empty;

        for (int i = 0; i < 20; i++)
        {
            text = ReadAllTextShared(path);

            if (text.Contains(expectedText))
            {
                return text;
            }

            Thread.Sleep(50);
        }

        return text;
    }

    private static string WaitForLogFile(string directory)
    {
        for (int i = 0; i < 20; i++)
        {
            string[] files = Directory.GetFiles(directory, "DayTrading-*.log", SearchOption.AllDirectories);

            if (files.Length == 1)
            {
                return files[0];
            }

            Thread.Sleep(50);
        }

        return Directory.GetFiles(directory, "DayTrading-*.log", SearchOption.AllDirectories)
            .Should()
            .ContainSingle()
            .Subject;
    }

    private sealed class CollectingSink : ILogEventSink
    {
        public List<LogEvent> Events { get; } = [];

        public void Emit(LogEvent logEvent)
        {
            Events.Add(logEvent);
        }
    }
}
