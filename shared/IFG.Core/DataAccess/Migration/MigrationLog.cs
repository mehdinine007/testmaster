using Castle.Core.Logging;

namespace IFG.Core.DataAccess.Migration
{
    public class MigrationLog
    {
        public ILogger Logger { get; set; }

        public MigrationLog()
        {
            Logger = NullLogger.Instance;
        }

        public void Write(string text)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + text);
            Logger.Info(text);
        }
    }
}
