using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.UI
{
    public class ViewUserActionModel
    {
        public ViewUserActionModel()
        {

        }

        public ViewUserActionModel(int id, int step, string description)
        {
            this.Id = id;
            this.StepNumber = step;
            this.StepDescription = description;
        }

        public int Id { get; set; }
        public int StepNumber { get; set; }
        public string StepDescription { get; set; }

    }
}
