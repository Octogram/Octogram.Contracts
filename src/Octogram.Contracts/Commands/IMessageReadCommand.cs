using System;

namespace Octogram.Contracts.Commands
{
	/// <summary>
	/// Command to read a message
	/// </summary>
	public interface IMessageReadCommand
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
