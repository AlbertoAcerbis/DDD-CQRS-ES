using System;
using Common.Logging;
using FourSolid.Cqrs.EventsDispatcher.Logging.Abstracts;

namespace FourSolid.Cqrs.EventsDispatcher.Logging.Concretes
{
    public class LogService : ILogService, IDisposable
    {
        private readonly ILog _log = LogManager.GetLogger("4Solid.EventsDispatcher");

        public void LoggerTrace(string message)
        {
            this._log.Trace(message);
        }

        public void ErrorTrace(string procedureName, Exception ex)
        {
            if (ex.InnerException != null)
                this.ErrorTrace(procedureName, ex.InnerException);

            this._log.Error(ex.Message);
        }

        public void WarnTrace(string message)
        {
            this._log.Warn(message);
        }

        #region Dispose
        private bool _disposed;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {

                }
            }
            this._disposed = true;
        }
        #endregion
    }
}