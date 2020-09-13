using System;

namespace Octogram.Contracts
{
	public interface IMessageSent
	{
		public Guid ChatId { get; }

		public string Sender { get; }
		
		public Guid MessageId { get; }

		public string Content { get; }
	}
}
