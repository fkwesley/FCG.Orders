using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IServiceBusPublisher
    {
        Task PublishMessageAsync(string topicName, object message, IDictionary<string, object>? customProperties = null);
    }
}
