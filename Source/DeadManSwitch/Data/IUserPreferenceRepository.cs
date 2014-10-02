using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Data
{
    public interface IUserPreferenceRepository
    {
        UserPreferences FindById(int userId);

        void Save(UserPreferences preferences);
    }
}
