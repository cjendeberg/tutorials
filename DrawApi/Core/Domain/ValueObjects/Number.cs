using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zero99Lotto.SRC.Services.Draws.API.Core.Domain.Exceptions;

namespace Zero99Lotto.SRC.Services.Draws.API.Core.Domain.ValueObjects
{
    public class Number : ValueObject
    {
        public int Value { get; private set; }
        public bool Extra { get; private set; }

        protected Number(int value, bool extra)
        {
            SetValue(value);
            SetExtra(extra);
            CheckRangeForExtra();
        }

        protected void SetExtra(bool extra)
            => Extra = extra;

        protected void SetValue(int value)
            => Value = value >= 0 && value <= 99 ? value : 
                throw new DomainException($"{nameof(Value)} out of range ({Value})!");

        protected void CheckRangeForExtra()
        {
            if(Extra)
                if(Value < 1 || Value > 20)
                    throw new DomainException($"{nameof(Value)} out of range for extra ({Value})!");
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Extra;
        }

        public static Number CreateNumber(int value, bool extra)
            => new Number(value, extra);
    }
}
