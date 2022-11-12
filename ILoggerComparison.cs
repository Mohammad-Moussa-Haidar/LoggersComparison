using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace LoggersComparison
{
    public interface ILoggerComparison : IHostedService
    {
        public Task Compare();
    }
}   