using System;

namespace Octogram.Contracts
{
	public interface IMessageDelivered
	{
		public Guid ChatId { get; }
		
		public Guid MessageId { get; }
	}
}
