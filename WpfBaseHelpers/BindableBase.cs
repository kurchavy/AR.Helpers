using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AR.WPF.Helpers
{
    /// <summary>
    /// Base class that implements INPC
    /// </summary>
    public class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Set property value
        /// </summary>
        /// <typeparam name="T">Underlying field type</typeparam>
        /// <param name="member">Field to set</param>
        /// <param name="val">Value</param>
        /// <param name="propertyName">Name of property</param>
        /// <returns></returns>
        protected virtual bool SetProperty<T>(ref T member, T val,
            [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return false;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// INPC Implementation
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
