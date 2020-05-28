using System;

namespace Octogram.Contracts.Commands
{
	/// <summary>
	/// Command to send a message
	/// </summary>
	public interface IMessageSendCommand
	{
		/// <summary>
		/// Returns message identifier
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
