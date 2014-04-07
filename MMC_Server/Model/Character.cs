using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MMC_Server.Model
{
    /// <summary>
    /// Data model for a character.
    /// </summary>
    public class Character
    {

        public String Name { get; set; }

        public String ModelCode { get; set; }

        public String MotionCode { get; set; }

        public ImageSource Preview { get; set; }

        public Boolean SpecifyModel { get; set; }

        public Boolean SpecifyMotion { get; set; }

        public Boolean SpecifyVideo { get; set; }


        public Boolean IsValid
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Name))
                    return false;
                if (String.IsNullOrWhiteSpace(ModelCode))
                    return false;
                if (String.IsNullOrWhiteSpace(MotionCode))
                    return false;
                return true;
            }
        }
    }
}
