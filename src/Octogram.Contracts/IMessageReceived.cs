using System;

namespace Octogram.Contracts
{
	public interface IMessageReceived
	{
		public Guid ChatId { get; }

		public Guid MessageId { get; }
	}
}
