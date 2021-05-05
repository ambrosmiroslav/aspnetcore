using System;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.HttpLogging;
using System.Diagnostics;
using System.Collections.Generic;

namespace Microsoft.Extensions.Logging.W3C
{
    internal sealed class W3CLogger : ILogger
    {

        private readonly string _name;
        private readonly W3CLoggerProcessor _messageQueue;
        private readonly IOptionsMonitor<W3CLoggerOptions> _options;
        private readonly bool _isActive;

        internal W3CLogger(string name, IOptionsMonitor<W3CLoggerOptions> options)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            _name = name;
            _options = options;

            // If the info isn't coming from HttpLoggingMiddleware, no-op (don't log anything)
            if (name == "Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware")
            {
                _isActive = true;
                _messageQueue = new W3CLoggerProcessor(_options);
            }
        }

        // TODO - do we need to do anything here?
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _isActive && logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            System.Console.WriteLine(eventId.Name);
            System.Console.WriteLine(eventId.Id);

            if (state is IReadOnlyCollection<KeyValuePair<string, object>> statePropertyObjects)
            {
                foreach (KeyValuePair<string, object> item in statePropertyObjects)
                {
                    _messageQueue.EnqueueMessage(item.ToString());
                }
            }
            else if (state is IReadOnlyCollection<KeyValuePair<string, string>> statePropertyStrings)
            {
                foreach (KeyValuePair<string, string> item in statePropertyStrings)
                {
                    _messageQueue.EnqueueMessage(item.ToString());
                }
            }
        }
    }
}
