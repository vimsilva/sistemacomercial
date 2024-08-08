using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaldoConsolidado.Infrastructure.RabbitMQ;
public class RabbitMQSettings
{
    public string HostName { get; set; }
    public string QueueName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
