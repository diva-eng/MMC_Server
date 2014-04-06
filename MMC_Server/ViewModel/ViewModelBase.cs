using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMC_Server.ViewModel
{
    /// <summary>
    /// Base class for all view models in the project.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Fires PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propName"></param>
        protected void FirePropertyChanged(String propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        /// <summary>
        /// Event that gets fired when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        public abstract void Dispose();
    }
}
