using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finalv2
{
    internal class parabolaTimer
    {
        float _decrease;
        public float _value;
        float _valuechange = 0;
        float _min;
        public parabolaTimer(float min, float max, float decrease) //constructor
        {
            this._min = min;
            this._value = min;
            this._decrease = decrease;
        }

        public void increase(float change)
        {
            if (_value <= _min)
            {
                _valuechange += change;
            }
            //_value = change;
        }

        public void update()
        {
            _valuechange -= _decrease;
            _value += _valuechange;
            _value = Math.Max(_value, _min);
        }
        public float value() { return _value; }
    }
}
