using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
	public interface IAssembler<TSource, TDestination>
	{
		TDestination Build(TSource source);
	}
}
