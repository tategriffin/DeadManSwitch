using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data.SqlRepository
{
    /// <summary>
    /// Entity framework will find this class (because it inherits from DbConfiguration),
    /// and instantiate it at runtime.
    /// 
    /// Adding config settings in the config file will override these settings.
    /// </summary>
    public class EntityFrameworkConfiguration : DbConfiguration
    {
        public EntityFrameworkConfiguration()
        {
            AddInterceptor(new LoggingInterceptor());
        }
    }
}
