using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDoor.Controllers
{
    class Unsubscriber<MasterController> : IDisposable
    {
        private List<IObserver<MasterController>> _observers;
        private IObserver<MasterController> _observer;

        internal Unsubscriber(List<IObserver<MasterController>> observers, IObserver<MasterController> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}
