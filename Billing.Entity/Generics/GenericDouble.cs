
namespace Billing.Entity.Generics
{
    using System.Collections.Generic;
    
    public class GenericDouble<T, TQ> where T : class, new()
    {
        public GenericDouble(T value, IList<TQ> list)
        {
            Value = value;
            List = list;
        }

        public T Value { get; set; }

        public IList<TQ> List { get; set; }
    }
}
