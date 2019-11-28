using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Models
{
    public class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // CallerMemberName automatically gets the name of the changed property.
        // If the name for some reason is not found it becomes null.
        public void OnPropertyChanged([CallerMemberName()] string callerName = null)
        {
            // If caller name was not found, then it does nothing.
            if (callerName != null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
        }
    }
}
