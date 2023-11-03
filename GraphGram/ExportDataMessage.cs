using CommunityToolkit.Mvvm.Messaging.Messages;

namespace GraphGram;
public class ExportDataMessage : ValueChangedMessage<float?[,]> {
    public ExportDataMessage(float?[,] data) : base(data) { }
}
