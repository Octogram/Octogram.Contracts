using System;

namespace Octogram.Contracts
{
	public interface IMessageRead
	{
		public Guid ChatId { get; }

		public Guid MessageId { get; }

		public string Reader { get; }
	}
}
