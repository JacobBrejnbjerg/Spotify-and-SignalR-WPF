using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Progress : NotifyBase
    {
        private int _progress_ms = 0;
        private int _duration_ms = 0;

        public int Progress_Ms
        {
            get { return _progress_ms; }
            set
            {
                _progress_ms = value;
                OnPropertyChanged();
            }
        }

        public int Duration_Ms
        {
            get { return _duration_ms; }
            set
            {
                _duration_ms = value;
                OnPropertyChanged();
            }
        }
    }
}
