using System;

namespace Octogram.Contracts.Events
{
	/// <summary>
	/// Event that rise when a message is read
	/// </summary>
	public interface IMessageReadEvent : IEvent
	{
		/// <summary>
		/// Returns message identity
		/// </summary>
		Guid MessageId { get; }
		
		/// <summary>
		/// Returns chat identity
		/// </summary>
		Guid ChatId { get; }
	}
}
