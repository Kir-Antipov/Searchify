using System.Numerics;
using System.Runtime.CompilerServices;

namespace Searchify.Collections.Generic;

[InlineArray(48)]
internal struct BKTreeNodeBuffer<TValue, TDistance>
    where TDistance : notnull, INumber<TDistance>, IMinMaxValue<TDistance>
{
    internal BKTreeNode<TValue, TDistance> _node0;
}
