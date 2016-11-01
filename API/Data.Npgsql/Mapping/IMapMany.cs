using System.Collections.Generic;

namespace Data.Npgsql.Mapping
{
    public interface IMapMany<in TIn, out TOut>
    {
        /// <summary>
        /// Maps multiple inputs into multiple outputs.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<TOut> Map(IEnumerable<TIn> source);
    }
}