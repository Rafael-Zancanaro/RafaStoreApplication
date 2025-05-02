using MediatR;

namespace RafaStore.Core.Messages;

public class Event : Message, INotification;