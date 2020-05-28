using System;

namespace Octogram.Contracts.Events
{
	/// <summary>
	/// Event that rise when a message is sent
	/// </summary>
	public interface IMessageSentEvent
	{
		/// <summary>
		/// Returns message identity
		/// </summary>
		Guid MessageId { get; }
		
		/// <summary>
		/// Returns chat identity
		/// </summary>
		Guid ChatId { get; }
		
		/// <summary>
		/// Returns message content
		/// </summary>
		string Content { get; }
	}
}
